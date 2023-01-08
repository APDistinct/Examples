using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    /// <summary>
    /// Route message to user, which contains in table [Usr].[UserSentry]
    /// </summary>
    public class SentryRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            UserSentry sentry = entities.UserSentry.FirstOrDefault();
            if (sentry != null)
                return sentry.UserId;
            else
                return null;
        }
    }
}
