using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class StatGetConveyor : ISendingConveyor
    {
        public class WorkInfo
        {
            public WorkInfo(int ind, int wc)
            {
                Index = ind;
                Count = wc;
            }
            public int Index { get;/* private set; */}
            public int Count { get; /*private set; */}
        }

        private readonly IMessageStatusPerformer _client;
        private IStatusSaver _statusSaver;  //  Сохранение состояния всего списка        
        private readonly IMessageIdsLoader _msgLoader;  //  Получение списка для обработки

        private readonly int performCount;
        public int MessagePerSecond { get; set; } = 30;
        public EventHandler<WorkInfo> OnStartLog;
        public EventHandler<WorkInfo> OnEndLog;
        public EventHandler<WorkInfo> OnStartRowLog;
        public EventHandler<WorkInfo> OnEndRowLog;
        public EventHandler<Exception> OnErrorLog;

        public StatGetConveyor(IMessageStatusPerformer client, IMessageIdsLoader msgLoader,
            IStatusSaver statusSaver = null, int pCount = 1000)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _msgLoader = msgLoader ?? new MessageIdsLoader();
            performCount = pCount;
            _statusSaver = statusSaver ?? new StatusSaver();
        }

        public void Send(CancellationToken ct)
        {
            int mainCount = 0;
            using (ChatEntities entities = new ChatEntities())
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    string[] idss = _msgLoader.Load(entities);

                    if (idss.Length == 0)
                        return;

                    Task<IEnumerable<MessageStats>>[] tasks = new Task<IEnumerable<MessageStats>>[MessagePerSecond];
                    int[] numbers = new int[MessagePerSecond];
                    OnStartLog?.Invoke(this, new WorkInfo(-1, idss.Length));

                    int sentCount = 0;
                    int step = 0;
                    while (step * performCount < idss.Length && !ct.IsCancellationRequested)
                    {
                        var ids = idss.Skip(step * performCount).Take(performCount).ToArray();

                        //ct.ThrowIfCancellationRequested();

                        try
                        {
                            Task<IEnumerable<MessageStats>> task = _client.Perform(ids, ct);
                            tasks[sentCount] = task;
                            numbers[sentCount] = step;
                            OnStartRowLog?.Invoke(this, new WorkInfo(step, ids.Count()));
                            //OnLog?.Invoke(this, $"Part {step} : {ids.Count()} rows started");
                            sentCount += 1;
                            step++;
                        }
                        catch (TaskCanceledException)
                        {
                        }
                        catch (Exception e)
                        {
                            //OnLog?.Invoke(this, $"Part {step} : {ids.Count()} numbers");
                            //id.IsFailed = true;
                            //InsertMsgError(entities, id, e);
                            //entities.SaveChanges();
                        }

                        if (sentCount == MessagePerSecond)
                        {
                            int index = Task.WaitAny(tasks, ct);
                            mainCount += OnSent(entities, tasks[index], numbers[index]);
                            tasks[index] = tasks[MessagePerSecond - 1];
                            numbers[index] = numbers[MessagePerSecond - 1];
                            tasks[MessagePerSecond - 1] = null;
                            numbers[MessagePerSecond - 1] = -1;
                            sentCount -= 1;
                        }
                    }

                    try
                    {
                        Task.WaitAll(sentCount < tasks.Length ? tasks.Take(sentCount).ToArray() : tasks, ct);
                    }
                    catch (AggregateException) { }

                    for (int i = 0; i < sentCount; ++i)
                    {
                        mainCount += OnSent(entities, tasks[i], numbers[i]);
                    }
                }
                finally
                {
                    OnEndLog?.Invoke(this, new WorkInfo(-1, mainCount));
                    entities.SaveChanges();
                }
            }
        }

        private int OnSent(ChatEntities entities, Task<IEnumerable<MessageStats>> t, int num)
        {
            int ret = 0;
            switch (t.Status)
            {
                case TaskStatus.RanToCompletion:
                    try
                    {
                        var states = t.Result.ToArray();
                        if (states.Count() > 0)
                        {
                            _statusSaver.Save(states, entities);
                        }
                        OnEndRowLog?.Invoke(this, new WorkInfo(num, states.Count()));
                        ret = states.Count();
                        //OnLog?.Invoke(this, $"Part {num} : {states.Count()} rows performed");
                    }
                    catch (Exception e)
                    {
                        OnEndRowLog?.Invoke(this, new WorkInfo(num, 0));
                        OnErrorLog(this, e);
                    }
                    break;
                case TaskStatus.Faulted:
                    foreach (Exception e in t.Exception.InnerExceptions)
                        OnErrorLog(this, e);
                    break;

                case TaskStatus.Canceled:
                    OnEndRowLog?.Invoke(this, new WorkInfo(num, 0));
                    break;

                default:
                    OnErrorLog(this, new ApplicationException($"Invalid task status: {t.Status.ToString()}"));
                    break;

            }
            return ret;
        }
    }
}
