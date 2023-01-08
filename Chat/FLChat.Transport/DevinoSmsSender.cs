using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devino;
using Devino.Logger;
using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.Devino;
using FLChat.SmsBott;

namespace FLChat.Transport
{
    public class DevinoSmsSender : DevinoSender
    {
        //protected override IDevinoProvider Sender { get; }
        protected override DAL.TransportKind transportKind { get; } = DAL.TransportKind.Sms;
        protected ISmsBot _smsBot;

        public DevinoSmsSender(IDevinoProvider sender = null, DevinoSettings settings = null, ISmsBot smsBot = null) : base(sender, settings)
        {
            _smsBot = smsBot ?? new SmsBot();
            //  Sender = sender ?? new DevinoProvider(DevinoLogger.GetLogger(DAL.TransportKind.Sms));
        }
       
        public override string GetAddressee(MessageToUser mtu)
        {
            return _smsBot.GetAddressee(mtu);  //  mtu.ToTransport.User.Phone;
        }

        public override async Task<SentMessageInfo> Send(MessageToUser msg, string addressees, string text)
        {
            var ret = await Sender.SmsSend(addressees, text);
            return new SentMessageInfo(ret.ProviderId, ret.SentTime);            
        }
    }
}
