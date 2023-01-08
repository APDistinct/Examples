using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.DAL;
using System.Data.SqlClient;

namespace FLChat.Core.Algorithms
{
    public class CallbackSelectAddressee : ICallbackDataProcessor
    {
        public void Process(ChatEntities entities, Transport transport, ICallbackData callbackData) {
            if (callbackData.IsChangeMsgAddressee())
                ChangeAddressee(entities, transport, callbackData.MsgAddressee());
        }

        public static void ChangeAddressee(ChatEntities entities, Transport transport, Guid? toAddr) {
            User user = null;
            if (toAddr.HasValue)
                user = entities
                            .User
                            .Where(u =>
                                u.Enabled
                                && u.Id == toAddr.Value
                                && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                                && u.IsBot == false
                                && u.IsTemporary == false)
                            .FirstOrDefault();
            transport.ChangeAddressee(entities, user);
            if (user != null)
                NotifyChangeAddressee(entities, transport, user);
            //    if (user != null) {
            //        if (user.Id == transport.User.OwnerUserId)
            //            RemoveAddressee(entities, transport); //transport.MsgAddressee = null;
            //        else
            //            transport.MsgAddressee = user;
            //        NotifyChangeAddressee(entities, transport, user);
            //    }
            //} else
            //    RemoveAddressee(entities, transport); //transport.MsgAddressee = null;
        }

        public static void NotifyChangeAddressee(ChatEntities entities, Transport transport, User user) {
            string message = Settings.Values.GetValue(SettingsDict.SettingNames.TEXT_CHANGE_MESSAGE_ADDRESSEE, user.FullName);
            Message msg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = entities.SystemBotTransport,
                Text = message.Replace("%FullName%", user.FullName),
                ToTransport = transport
            };
            entities.Message.Add(msg);
        }

        //private void RemoveAddressee(ChatEntities entities, Transport transport) {
        //    entities.Database.ExecuteSqlCommand("delete from [Usr].[MsgAddressee] where [UserId] = @uid",
        //                    new SqlParameter("@uid", transport.User.Id));
        //}
    }
}
