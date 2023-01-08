using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Algorithms.WebChat
{
    public interface IWebChatCodeGenerator
    {
        string Gen(MessageToUser mtu);
    }
}
