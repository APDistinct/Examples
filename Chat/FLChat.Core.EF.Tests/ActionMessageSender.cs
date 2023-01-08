using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Tests
{
    public class ActionMessageSender : IMessageSender
    {
        private Func<MessageToUser, string, SentMessageInfo> _func;

        public ActionMessageSender(Func<MessageToUser, string, SentMessageInfo> func) {
            _func = func;
        }

        public Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct) {
            return Task.Run(() => _func(msg, msgText));
        }
    }
}
