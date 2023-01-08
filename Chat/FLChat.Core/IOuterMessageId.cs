using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IOuterMessageId : ICallbackBase
    {
        /// <summary>
        /// Message Id
        /// </summary>
        string MessageId { get; }
    }
}
