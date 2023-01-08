using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Algorithms;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public interface IMessageRouter//<TMessage> where TMessage : IOuterMessage
    {
        /// <summary>
        /// Rout message, search for message addressee
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="message">incoming message</param>
        /// <param name="dbmsg">database message object</param>
        /// <returns>returns adressee id or null</returns>
        Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg);
    }
}
