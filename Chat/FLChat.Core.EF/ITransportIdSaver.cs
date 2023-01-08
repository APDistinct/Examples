using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core
{
    /// <summary>
    /// Save outer transport id interface
    /// </summary>
    public interface ITransportIdSaver
    {
        /// <summary>
        /// save id for outcoming message
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="id">outer id</param>
        /// <param name="msg">message addressee</param>
        void SaveTo(ChatEntities entities, string id, MessageToUser msg);

        /// <summary>
        /// save id for incoming message
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="id">outer id</param>
        /// <param name="msg">message</param>
        void SaveFrom(ChatEntities entities, string id, Message msg);

        /// <summary>
        /// Save many ids for one outer message
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="ids">list of id</param>
        /// <param name="msg">database message</param>
        void SaveTo(ChatEntities entities, string[] ids, MessageToUser msg);
    }
}
