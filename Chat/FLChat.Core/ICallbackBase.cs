using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;

namespace FLChat.Core
{
    public interface ICallbackBase
    {
        /// <summary>
        /// Kind of transport
        /// </summary>
        TransportKind TransportKind { get; }
    }
}
