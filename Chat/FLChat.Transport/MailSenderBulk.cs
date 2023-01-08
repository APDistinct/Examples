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
    public class MailSenderBulk : SenderBulk/*, ITextSender*/, IMessageBulkSender
    {
        DevinoProvider _sender { get; } = new DevinoProvider(DevinoLogger.GetLogger(TransportKind.Email));
        public MailSenderBulk()
        {
            _transport = (int)TransportKind.Email;
        }

        //public void Send(string addressee, string text)
        //{
        //    //string ret = sms.Send(contractPhone, addressee, text);
        //    //  Далее анализ ответа и реакция.

        //}

        public override string GetAddressee(MessageToUser mtu)
        {
            return mtu.ToTransport.User.Email;
        }

        //public override void Send(List<string> addressees, string text)
        public override async Task<IEnumerable<SentMessageInfo>> Send(List<string> addressees, string text)
        {
            //var ret = _sender.EmailSend(addressees, text);
            var ret = await _sender.EmailSend(addressees, text);
            return ret.Select( x =>  new SentMessageInfo(x.ProviderId, x.SentTime));
        }
    }
}
