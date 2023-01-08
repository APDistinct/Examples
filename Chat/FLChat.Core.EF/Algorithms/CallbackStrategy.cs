using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Algorithms
{
    public class CallbackStrategy : ICallbackQueryStrategy<ChatEntities>
    {
        private readonly ICallbackDataProcessor _processor;

        public CallbackStrategy(ICallbackDataProcessor processor) {
            _processor = processor;
        }

        public void Process(ChatEntities entities, ICallbackData callbackData) {
            Transport fromTransport = entities
                .Transport
                .GetTransportByOuterId(callbackData.TransportKind, callbackData.FromUserId, null);
            if (fromTransport != null) {
                DAL.Model.Message msg = RegisterCallbackMsg(entities, fromTransport, callbackData);
                try {
                    _processor.Process(entities, fromTransport, callbackData);
                } catch (Exception e) {
                    msg.MessageError.Add(e.ToMessageError());
                }
                entities.SaveChanges();
            }
        }

        private Message RegisterCallbackMsg(ChatEntities entities, Transport fromTransport, ICallbackData callbackData) {
            DAL.Model.Message msg = new Message() {
                Kind = DAL.MessageKind.Personal,
                FromTransport = fromTransport,
                Text = String.Concat(callbackData.Data),
                Specific = String.Concat("CALLBACK=", callbackData.Id),
                ToTransport = entities.SystemBotTransport,
            };
            //msg.MessageTransportId.Add(new MessageTransportId() {
            //    TransportId = callbackData.Id,
            //    TransportTypeId = (int)callbackData.TransportKind
            //});
            return entities.Message.Add(msg);
        }

        
    }
}
