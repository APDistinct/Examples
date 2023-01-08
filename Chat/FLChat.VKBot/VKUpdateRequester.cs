using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using FLChat.VKBotClient.Types;

//using Telegram.Bot;
//using Telegram.Bot.Types;

namespace FLChat.VKBot
{
    public class VKUpdateRequester
    {
        private readonly VKClient _client;
        private readonly IVKUpdateHandler _handler;

        public VKUpdateRequester(VKClient client, IVKUpdateHandler handler)
        {
            _client = client;
            _handler = handler;
        }

        public int Limit { get; set; } = 0;
        public int Timeout { get; set; } = 0;
        public int DelayTimeMs { get; set; } = 100;

        public async Task<int> ReceiveUpdates(int offset, CancellationToken ct, EventHandler<Exception> onException)
        {
            while (!ct.IsCancellationRequested)
            {
                using (ChatEntities entities = new ChatEntities())
                {
                    try
                    {
                        int groupId = 179649792;
                        var callbacks = await _client.Client.MakeUpdatesAsync(groupId, cancellationToken: ct);
                        if (callbacks.Length > 0)
                        {
                            // Переделать под ВК
                            foreach (var upd in callbacks)
                            {
                                try
                                {
                                    //_handler.MakeUpdate(entities, upd);
                                }
                                catch (Exception e)
                                {
                                    onException?.Invoke(this, e);
                                }
                            }
                            offset = offset + 1;
                        }
                    }
                    catch (OperationCanceledException e)
                    {
                        if (ct.IsCancellationRequested)
                            return offset;
                        onException?.Invoke(this, e);
                        throw;
                    }
                    catch (Exception e)
                    {
                        onException?.Invoke(this, e);
                    }
                }

                await Task.Delay(DelayTimeMs);
            }
            return offset;
        }
    }
}
