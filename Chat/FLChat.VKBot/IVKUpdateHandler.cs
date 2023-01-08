using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.VKBotClient.Types;

namespace FLChat.VKBot
{
    public interface IVKUpdateHandler
    {
        object MakeUpdate(ChatEntities entities, CallbackData callbackData);
    }

    public static class IVKUpdateHandlerExtentions
    {
        public static object MakeUpdate(this IVKUpdateHandler handler, CallbackData callbackData)
        {
            using (var entities = new ChatEntities())
            using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                var result = handler.MakeUpdate(entities, callbackData);
                transaction.Commit();
                return result;
            }
        }
    }
}
