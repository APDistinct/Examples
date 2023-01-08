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
    public class SendingConveyor : ISendingConveyor
    {
        private readonly IMessageSender _client;
        //private readonly TransportKind _transport;
        private readonly ITransportIdSaver _idSaver;
        private readonly IMessageTextCompiler _msgCompiler;
        private readonly IMessageLoader _msgLoader;
        private readonly bool _waiting;

        public int MessagePerSecond { get; set; } = 30;

        //public SendingConveyor(IMessageSender client, TransportKind transport,
        //    ITransportIdSaver idSaver = null, IMessageTextCompiler msgCompiler = null,
        //    IMessageLoader msgLoader = null)
        public SendingConveyor(IMessageSender client, TransportKind transport,
           ITransportIdSaver idSaver = null, IMessageTextCompiler msgCompiler = null, bool waiting = true)
            : this(client, new MessageLoader(transport), idSaver, msgCompiler, waiting)
        {
        }

        public SendingConveyor(IMessageSender client, IMessageLoader msgLoader,
            ITransportIdSaver idSaver = null, IMessageTextCompiler msgCompiler = null, bool waiting = true)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            //_transport = transport;
            _idSaver = idSaver;
            _msgCompiler = msgCompiler ?? new SimpleMsgTextCompiler();
            _msgLoader = msgLoader;
            _waiting = waiting;
        }

        public void Send(CancellationToken ct)
        {
            using (ChatEntities entities = new ChatEntities())
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    MessageToUser[] msgs = _msgLoader.Load(entities);

                        //entities.MessageToUser
                        //.Where(mt =>
                        //       mt.ToTransportTypeId == (int)_transport
                        //    && mt.IsSent == false
                        //    && mt.IsFailed == false
                        //    && mt.Message.IsDeleted == false)
                        //.Include(mt => mt.Message)
                        //.Include(mt => mt.ToTransport)
                        //.Include(mt => mt.ToTransport.User)
                        //.Include(mt => mt.Message.FromTransport)
                        //.Include(mt => mt.Message.FromTransport.User)
                        //.ToArray();

                    if (msgs.Length == 0)
                        return;

                    //List<Tuple<Task<int>, MessageToUser>> tasks = new List<Tuple<Task<int>, MessageToUser>>(MessagePerSecond);
                    Task<SentMessageInfo>[] tasks = new Task<SentMessageInfo>[MessagePerSecond];
                    MessageToUser[] msgInWork = new MessageToUser[MessagePerSecond];

                    int sentCount = 0;
                    foreach (MessageToUser msg in msgs)
                    {
                        ct.ThrowIfCancellationRequested();

                        try
                        {
                            Task<SentMessageInfo> task = _client.Send(msg, _msgCompiler.MakeText(msg), ct);
                            tasks[sentCount] = task;
                            msgInWork[sentCount] = msg;
                            sentCount += 1;
                        }
                        catch (TaskCanceledException)
                        {
                        }
                        catch (Exception e)
                        {
                            msg.IsFailed = true;
                            InsertMsgError(entities, msg, e);
                            entities.SaveChanges();
                        }

                        if (sentCount == MessagePerSecond)
                        {
                            int index = Task.WaitAny(tasks, ct);
                            SentMessageInfo tuple = OnSent(entities, tasks[index], msgInWork[index], true);
                            msgInWork[index] = msgInWork[MessagePerSecond - 1];
                            tasks[index] = tasks[MessagePerSecond - 1];
                            tasks[MessagePerSecond - 1] = null;
                            msgInWork[MessagePerSecond - 1] = null;
                            sentCount -= 1;

                            if (tuple != null && _waiting)
                            {
                                int waitTm = (int)(1000 - (DateTime.UtcNow - tuple.SentTime).TotalMilliseconds);
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
                        OnSent(entities, tasks[i], msgInWork[i], true);
                    }
                }
                finally
                {
                    entities.SaveChanges();
                }
            }
        }

        private SentMessageInfo OnSent(ChatEntities entities, Task<SentMessageInfo> t, MessageToUser msg, bool save)
        {
            SentMessageInfo result = null;
            switch (t.Status)
            {
                case TaskStatus.RanToCompletion:
                    msg.IsSent = true;
                    if (t.Result.MessageIds != null)
                        _idSaver?.SaveTo(entities, t.Result.MessageIds, msg);
                    else if (t.Result.MessageId != null)
                        _idSaver?.SaveTo(entities, t.Result.MessageId, msg);
                    result = t.Result;
                    break;

                case TaskStatus.Faulted:
                    msg.IsFailed = true;
                    foreach (Exception e in t.Exception.InnerExceptions)
                        InsertMsgError(entities, msg, e);
                    break;

                case TaskStatus.Canceled:
                    break;

                default:
                    throw new ApplicationException($"Invalid task status: {t.Status.ToString()}");

            }
            if (save) {
                try {
                    entities.SaveChanges();
                } catch (Exception e) {
                    RollBack(entities);
                    msg.IsFailed = true;
                    InsertMsgError(entities, msg, e);
                    entities.SaveChanges();
                }
            }
            return result;
        }

        private void InsertMsgError(ChatEntities entities, MessageToUser msg, Exception e)
        {
            entities.MessageError.Add(new MessageError()
            {
                MsgId = msg.MsgId,
                ToUserId = msg.ToUserId,
                ToTransportTypeId = msg.ToTransportTypeId,
                Type = e.GetType().Name,
                Descr = e.Message,
                Trace = e.ToString(),
            });
        }

        private void RollBack(ChatEntities entities) {
            var changedEntries = entities.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries) {
                switch (entry.State) {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
