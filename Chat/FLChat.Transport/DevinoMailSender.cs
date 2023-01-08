using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devino;
using Devino.Logger;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Devino;

namespace FLChat.Transport
{
    public class DevinoMailSender : DevinoSender
    {
        private readonly IMessageTextCompiler MsgCompiler;
        private readonly string Pattern;
        protected override DAL.TransportKind transportKind { get; } = TransportKind.Email;

        //protected override IDevinoProvider Sender { get; } = new DevinoProvider(DevinoLogger.GetLogger(DAL.TransportKind.Email));

        public DevinoMailSender(IMessageTextCompiler msgCompiler = null, string pattern = "", IDevinoProvider sender = null, DevinoSettings settings = null) : base(sender, settings)
        {
            Pattern = pattern;
            MsgCompiler = msgCompiler ?? new SimpleMsgTextCompiler();
        }

        public override string GetAddressee(MessageToUser mtu)
        {
            return mtu.ToTransport.User.Email;
        }
        
        public override async Task<SentMessageInfo> Send(MessageToUser msg, string addressees, string text)
        {
            string subject = //"Вам письмо";
                MsgCompiler.MakeText(msg, "");
            var ret = await Sender.EmailSend(addressees, text, subject);
            return new SentMessageInfo(ret.ProviderId, ret.SentTime);
        }        
    }
}
