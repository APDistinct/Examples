using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Core.MsgCompilers;

namespace FLChat.Core
{
    public class SendingConveyorBulk : ISendingConveyor
    {
        private readonly IMessageBulkSender _client;
        private readonly TransportKind _transport;
        private readonly ITransportIdSaver _idSaver;
        private readonly IMessageBulkTextCompiler _msgCompiler;

        public int MessagePerSecond { get; set; } = 30;

        public SendingConveyorBulk(IMessageBulkSender client, TransportKind transport,
            ITransportIdSaver idSaver = null, IMessageBulkTextCompiler msgCompiler = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _transport = transport;
            _idSaver = idSaver;
            _msgCompiler = msgCompiler ?? new SimpleMsgTextCompiler();
        }

        public void Send(CancellationToken ct)
        {
            using (ChatEntities entities = new ChatEntities())
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    //Message[] msgs = entities.Message
                    //    //.Where(m => m.ToUsers.GetToSend((int)_transport).Any())
                    //    .Where(m => m.ToUsers.Where(mt =>
                    //           mt.ToTransportTypeId == (int)_transport
                    //        && mt.IsSent == false
                    //        && mt.IsFailed == false
                    //        && mt.Message.IsDeleted == false).Any())

                    Message[] msgs = entities.MessageToUser
                        .Where(mt =>
                               mt.ToTransportTypeId == (int)_transport
                            && mt.IsSent == false
                            && mt.IsFailed == false
                            && mt.Message.IsDeleted == false)
                        //.Include(mt => mt.Message)
                        .Include(mt => mt.ToTransport)
                        .Include(mt => mt.ToTransport.User)
                        .Include(mt => mt.Message.FromTransport)
                        .Include(mt => mt.Message.FromTransport.User)
                        .Select(mtu => mtu.Message).Distinct()
                        .ToArray();
                    if (msgs.Length == 0)
                        return;

                    //List<Tuple<Task<int>, MessageToUser>> tasks = new List<Tuple<Task<int>, MessageToUser>>(MessagePerSecond);
                    Task<IEnumerable<SentMessageInfo>>[] tasks = new Task<IEnumerable<SentMessageInfo>>[MessagePerSecond];
                    Message[] msgInWork = new Message[MessagePerSecond];

                    int sentCount = 0;
                    foreach (Message msg in msgs)
                    {
                        ct.ThrowIfCancellationRequested();

                        Task<IEnumerable<SentMessageInfo>> task = _client.Send(msg, _msgCompiler.MakeText(msg), ct);
                        tasks[sentCount] = task;
                        msgInWork[sentCount] = msg;
                        sentCount += 1;

                        if (sentCount == MessagePerSecond)
                        {
                            int index = Task.WaitAny(tasks, ct);
                            IEnumerable<SentMessageInfo> tuple = OnSent(entities, tasks[index], msgInWork[index], true);
                            msgInWork[index] = msgInWork[MessagePerSecond - 1];
                            tasks[index] = tasks[MessagePerSecond - 1];
                            tasks[MessagePerSecond - 1] = null;
                            msgInWork[MessagePerSecond - 1] = null;
                            sentCount -= 1;

                            if (tuple != null)
                            {
                                var dt = tuple.Max(x => x.SentTime);
                                int waitTm = (int)(1000 - (DateTime.UtcNow - dt/*tuple.SentTime*/).TotalMilliseconds);
                                if (waitTm > 0)
                                    Task.Delay(waitTm).Wait(); //Thread.Sleep(waitTm);
                            }
                        }
                    }

                    try
                    {
                        Task.WaitAll(sentCount < tasks.Length ? tasks.Take(sentCount).ToArray() : tasks, ct);
                    }
                    catch (AggregateException) { }

                    for (int i = 0; i < sentCount; ++i)
                    {
                        OnSent(entities, tasks[i], msgInWork[i], false);
                    }
                }
                //catch (Exception e)
                //{
                //    string s = e.Message;
                //}
                finally
                {
                    entities.SaveChanges();
                }
            }
        }

        private IEnumerable<SentMessageInfo> OnSent(ChatEntities entities, Task<IEnumerable<SentMessageInfo>> t, Message msg, bool save)
        {
             IEnumerable < SentMessageInfo > result = null;
            var mtuList = msg.ToUsers.Where(mt =>
                              mt.ToTransportTypeId == (int)_transport
                           && mt.IsSent == false
                           && mt.IsFailed == false
                           && mt.Message.IsDeleted == false);
            switch (t.Status)
            {                
                case TaskStatus.RanToCompletion:
                    //  Фиксация ВСЕХ в сообщении
                    var arr = t.Result.ToArray();
                    int i = 0;
                    foreach (var mtu in mtuList)
                    {
                        mtu.IsSent = true;
                        _idSaver?.SaveTo(entities, arr[i].MessageId, mtu);
                        i++;
                    }
                    result = t.Result;
                    break;

                case TaskStatus.Faulted:
                    //  Фиксация ВСЕХ в сообщении
                    foreach (var mtu in mtuList)
                    {
                        mtu.IsFailed = true;
                        foreach (Exception e in t.Exception.InnerExceptions)
                            entities.MessageError.Add(new MessageError()
                            {
                                MsgId = mtu.MsgId,
                                ToUserId = mtu.ToUserId,
                                ToTransportTypeId = mtu.ToTransportTypeId,
                                Type = e.GetType().Name,
                                Descr = e.Message,
                                Trace = e.ToString(),
                            });
                    }
                    break;

                case TaskStatus.Canceled:
                    break;

                default:
                    throw new ApplicationException($"Invalid task status: {t.Status.ToString()}");

            }
            if (save)
                entities.SaveChanges();
            return result;
        }
    }
}
