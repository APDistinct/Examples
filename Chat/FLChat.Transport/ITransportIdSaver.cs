using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Transport
{
    /// <summary>
    /// Save outer transport id interface
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface ITransportIdSaver<TId>
    {
        /// <summary>
        /// save transport id for sending message to user
        /// </summary>
        /// <param name="id">outer id</param>
        /// <param name="msg">message</param>
        void Save(ChatEntities entities, TId id, MessageToUser msg);
    }
}
