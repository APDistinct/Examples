using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FLChat.TelegramBot
{
    public class TelegramUpdateRequester
    {
        private readonly TelegramClient _client;
        private readonly ITelegramUpdateHandler _handler;

        public TelegramUpdateRequester(TelegramClient client, ITelegramUpdateHandler handler) {
            _client = client;
            _handler = handler;
        }

        public int Limit { get; set; } = 0;
        public int Timeout { get; set; } = 0;
        public int DelayTimeMs { get; set; } = 100;

        public async Task<int> ReceiveUpdates(int offset, CancellationToken ct, EventHandler<Exception> onException) {
            while (!ct.IsCancellationRequested) {
                using (ChatEntities entities = new ChatEntities()) {
                    try {
                        Update[] updates = await _client.Client.GetUpdatesAsync(offset, Limit, Timeout, cancellationToken: ct);
                        if (updates.Length > 0) {
                            int prev_offset = offset;
                            offset = 0;
                            foreach (var upd in updates) {
                                offset = Math.Max(offset, upd.Id);
                                try {
                                    if (upd.Id >= prev_offset)
                                        _handler.MakeUpdate(entities, upd);
                                } catch (Exception e) {
                                    onException?.Invoke(this, e);
                                }
                            }
                            offset = offset + 1;
                        }
                    } catch (OperationCanceledException e) {
                        if (ct.IsCancellationRequested)
                            return offset;
                        onException?.Invoke(this, e);
                        throw;
                    }
                    catch (Exception e) {
                        onException?.Invoke(this, e);
                    }                    
                }

                await Task.Delay(DelayTimeMs);
            }
            return offset;
        }
    }
}
