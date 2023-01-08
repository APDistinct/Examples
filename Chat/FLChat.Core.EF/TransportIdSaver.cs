using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public class TransportIdSaver : ITransportIdSaver
    {
        public void SaveTo(ChatEntities entities, string id, MessageToUser msg) {
            entities.MessageTransportId.Add(new MessageTransportId() {
                MessageToUser = msg,
                TransportId = id,
            });
        }

        public void SaveFrom(ChatEntities entities, string id, Message msg) {
            MessageTransportId tid = new MessageTransportId() {
                Message = msg,
                TransportTypeId = msg.FromTransportTypeId,
                TransportId = id,
            };
            entities.MessageTransportId.Add(tid);
        }

        /// <summary>
        /// Save many ids for one outer message
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="ids">list of id</param>
        /// <param name="msg">database message</param>
        public void SaveTo(ChatEntities entities, string[] ids, MessageToUser msg) {
            if (ids.Length == 0)
                throw new ArgumentOutOfRangeException("Array of outer message id is empty");
            if (ids.Length > Byte.MaxValue)
                throw new ArgumentOutOfRangeException("Too many outer message ids");

            for (byte index = 0; index < ids.Length; ++index) { 
                entities.MessageTransportId.Add(new MessageTransportId() {
                    MessageToUser = msg,
                    TransportId = ids[index],
                    Index = index,
                    Count = (byte)ids.Length
                });
            }
        }
    }
}
