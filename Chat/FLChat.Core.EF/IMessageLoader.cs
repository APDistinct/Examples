using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IMessageLoader
    {
        MessageToUser[] Load(ChatEntities entities);
    }
}
