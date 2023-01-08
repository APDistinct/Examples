using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

using FLChat.DAL.Model;

namespace FLChat.TelegramBot
{
    /// <summary>
    /// Handle telegram updates interface
    /// </summary>
    public interface ITelegramUpdateHandler
    {
        void MakeUpdate(ChatEntities entities, Update update);
    }

    public static class ITelegramUpdateHandlerExtentions
    {
        /// <summary>
        /// Create ChatEntities and perform update
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="update"></param>
        public static void MakeUpdate(this ITelegramUpdateHandler handler, Update update) {
            using (ChatEntities entities = new ChatEntities()) {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)) {
                    handler.MakeUpdate(entities, update);
                    transaction.Commit();
                }
            }
        }
    }
}
