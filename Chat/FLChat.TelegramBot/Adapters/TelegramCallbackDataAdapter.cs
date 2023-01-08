using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using FLChat.Core;
using FLChat.DAL;

namespace FLChat.TelegramBot.Adapters
{
    public class TelegramCallbackDataAdapter : ICallbackData
    {
        private readonly CallbackQuery _data;

        public TelegramCallbackDataAdapter(CallbackQuery data) {
            _data = data;
        }

        public TransportKind TransportKind => TransportKind.Telegram;

        public string FromMessageId => _data.Message.MessageId.ToString();

        public string FromUserId => _data.From.Id.ToString();

        public string Data => _data.Data;

        public string Id => _data.Id;
    }
}
