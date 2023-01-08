using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SmsRu;

using FLChat.Core;
using FLChat.DAL.Model;
using System.Threading;
using Devino.Logger;
using FLChat.DAL;
using FLChat.Devino;

namespace FLChat.Transport
{
    public class SmsSenderBulk : SenderBulk, ITextSender, IMessageBulkSender
    {
        DevinoProvider _sender { get; } = new DevinoProvider(DevinoLogger.GetLogger(DAL.TransportKind.Sms));
        public SmsSenderBulk()
        {
            _transport = (int)TransportKind.Sms;
        }

        public void Send(string addressee, string text)
        {
            //string ret = sms.Send(contractPhone, addressee, text);
            //  Далее анализ ответа и реакция.

        }

        public override string GetAddressee(MessageToUser mtu)
        {
            return mtu.ToTransport.User.Phone;
        }

        public override async Task<IEnumerable<SentMessageInfo>> Send(List<string> addressees, string text)
        {            
            var ret = await _sender.SmsSend(addressees, text);
            return ret.Select(x => new SentMessageInfo(x.ProviderId, x.SentTime));            
        }       
    }
}
