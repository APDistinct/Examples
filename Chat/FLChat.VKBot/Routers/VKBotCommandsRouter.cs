using FLChat.Core;
using FLChat.Core.Routers;
using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBot.Routers
{
    public class VKBotCommandsRouter : BotCommandsRouterByCmd
    {
        public class CommandExtrator : ICommandExtractor
        {
            public string ExtractCommandValue(ChatEntities entities, IOuterMessage msg)
            {                
                return (msg as VKMessageAdapter)?.Command;
            }
        }

        public VKBotCommandsRouter() : base(new CommandExtrator())
        {
        }        
    }    
}
