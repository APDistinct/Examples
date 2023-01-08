using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.TelegramBot.Algorithms
{
    public class AskPhoneRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            User usr = dbmsg.FromTransport.User;
            if (usr.IsTemporary && usr.Phone == null) {
                if (message.PhoneNumber != null) {
                    AcceptPhoneNumber(entities, message, dbmsg);
                    return null;
                } else {
                    return AskPhoneMessage(entities, dbmsg.FromTransport);
                }
            } else
                return null;
        }

        private Guid AskPhoneMessage(ChatEntities entities, Transport to) {
            Message msg = new Message() {
                Kind = MessageKind.Personal,
                FromUserId = Global.SystemBotId,
                FromTransportKind = TransportKind.FLChat,
                Text = "Хотелось бы узнать ваш номер телефона",
                IsPhoneButton = true
            };
            msg.ToUsers.Add(new MessageToUser() {
                ToUserId = to.UserId,
                ToTransportTypeId = to.TransportTypeId
            });
            entities.Message.Add(msg);
            return msg.FromUserId;
        }

        /// <summary>
        /// Accepted information about user's phone
        /// If user with that phone already exists, then Merge users.
        /// If user with that phone not exists, then update phone field and redirect user to operator master
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        private void AcceptPhoneNumber(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            User user = entities.User.Where(u => u.Enabled && u.Phone == message.PhoneNumber).SingleOrDefault();
            User from = entities.User.Where(u => u.Id == dbmsg.FromUserId).SingleOrDefault();
            if (from == null || from.IsBot || from.IsTemporary == false)
                throw new Exception($"Can't merge user {dbmsg.FromUserId.ToString()}");

            if (user != null) {
                Guid[] messages = entities.MergeUsers(user.Id, dbmsg.FromUserId).Where(g => g.HasValue).Select(g => g.Value).ToArray();
                dbmsg.FromUserId = user.Id;
                dbmsg.FromTransport = entities.Transport
                    .Where(t => t.Enabled && t.UserId == user.Id && t.TransportTypeId == dbmsg.FromTransportTypeId)
                    .Single();
            } else {
                dbmsg.FromTransport.User.Phone = message.PhoneNumber;
            }
        }
    }
}
