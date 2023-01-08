using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VkWidget
{
    public interface IVKWidgetUpdateHandler
    {
        object MakeUpdate(ChatEntities entities, VkWidgetCallbackData callbackData);
    }

    public static class IVKUpdateHandlerExtentions
    {
        public static object MakeUpdate(this IVKWidgetUpdateHandler handler, VkWidgetCallbackData callbackData)
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
