using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.SmsBott
{
    public class SmsBot : ISmsBot
    {
        private User _smsBot = null;
        private Transport _smsBotTransport = null;
        private ChatEntities entities = new ChatEntities();
        /// <summary>
        /// Bot user
        /// </summary>
        public User SmsBotUser
        {
            get
            {                
                if (_smsBot == null)
                    _smsBot = entities.User.Where(u => u.Id == Global.SmsBotId).Single();
                return _smsBot;
            }
        }

        /// <summary>
        /// Bot Sms transport
        /// </summary>
        public Transport SmsBotTransport
        {
            get
            {
                if (_smsBotTransport == null)
                    _smsBotTransport = SmsBotUser.Transports.Get(TransportKind.Sms);
                return _smsBotTransport ?? throw new NullReferenceException("Sms bot transport is null");
            }
        }

        public string GetAddressee(MessageToUser mtu)
        {
            string addr = null;
            if(mtu.ToTransport.User?.UserId == Global.SmsBotId/*SmsBotUser.UserId*/)
            {
                addr = mtu.Message.Specific;
            }
            else
            {
                addr = mtu.ToTransport.User.Phone;
            }
            return addr;
        }

        public Message SendSmsMessage(string addressee, string text, bool change = false)
        {
            Message smsMessage = new Message()
            {
                Kind = MessageKind.Personal,
                FromTransport = entities.SystemBotTransport,
                //AnswerTo = dbmsg,
                Text = text,
                Specific = addressee,
                ToUsers = new MessageToUser[] {
                        new MessageToUser() {
                            ToUserId = Global.SmsBotId,
                            ToTransport = SmsBotTransport,
                        }
                },
                //ScenarioStepId = step,
                NeedToChangeText = change,
            };
            entities.Message.Add(smsMessage);
            entities.SaveChanges();
            return smsMessage;
        }
    }
}
