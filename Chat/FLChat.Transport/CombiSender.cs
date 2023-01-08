using FLChat.Core;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.Transport
{
    public class CombiSender : IMessageSender
    {
        private readonly IMessageSender LeftHandSender;
        private readonly IMessageSender RightHandSender;        
        Func<MessageToUser, bool> Func;

        public CombiSender(IMessageSender leftHandSender, IMessageSender rightHandSender, Func<MessageToUser, bool> func)
        {
            LeftHandSender = leftHandSender;
            RightHandSender = rightHandSender;
            Func = func;
        }

        public Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct)
        {
            if(Func(msg))
                return LeftHandSender.Send(msg, msgText, ct);
            else
                return RightHandSender.Send(msg, msgText, ct);
        }
    }
}
