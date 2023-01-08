using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Bot
{
    public interface IViberUpdateHandler
    {
        object MakeUpdate(ChatEntities entities, CallbackData callbackData);
    }

    public static class IViberUpdateHandlerExtentions
    {
        /// <summary>
        /// Create ChatEntities and perform update
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="update"></param>
        public static object MakeUpdate(this IViberUpdateHandler handler, CallbackData callbackData) {
            using (ChatEntities entities = new ChatEntities()) {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)) {
                    object result = handler.MakeUpdate(entities, callbackData);
                    transaction.Commit();
                    return result;
                }
            }
        }
    }
}
