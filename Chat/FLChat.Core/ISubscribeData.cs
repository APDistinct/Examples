using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface ISubscribeData : ICallbackBase
    {
        string UserId { get; }
        string UserName { get; }
    }
}
