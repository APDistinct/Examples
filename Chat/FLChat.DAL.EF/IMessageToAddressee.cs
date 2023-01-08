using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public interface IMessageToAddressee
    {
        Message Message { get; set; }
    }
}
