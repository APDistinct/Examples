using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core
{
    public interface ICallbackDataProcessor
    {
        void Process(ChatEntities entities, Transport transport, ICallbackData callbackData);
    }
}
