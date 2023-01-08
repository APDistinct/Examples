using FLChat.Core;
using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.Transport
{
    public class SenderBulk : IMessageBulkSender
    {
        protected int _transport;

        public virtual Task<IEnumerable<SentMessageInfo>> Send(List<string> addressees, string text)
        {
            return null;
            //string ret = sms.SmsSend(addressees, text);
            //  Далее анализ ответа и реакция.
        }

        public virtual string GetAddressee(MessageToUser mtu)
        {
            return "";
        }

        public Task<IEnumerable<SentMessageInfo>> Send(Message msg, string text, CancellationToken ct)
        {
            List<string> vs = new List<string>();
            var mtuList = msg.ToUsers.Where(mt =>
                               mt.ToTransportTypeId == _transport
                            && mt.IsSent == false
                            && mt.IsFailed == false
                            && mt.Message.IsDeleted == false);
            //foreach (var mtu in msg.ToUsers.GetToSend((int)TransportKind.Sms))
            foreach (var mtu in mtuList)
            {
                vs.Add(GetAddressee(mtu));
            }
            return Send(vs, text);
            //return Task.Run<SentMessageInfo>(() => 
            //{
            //    Send(vs, text);
            //    return new SentMessageInfo(null, DateTime.Now);
            //});
        }
    }
}
