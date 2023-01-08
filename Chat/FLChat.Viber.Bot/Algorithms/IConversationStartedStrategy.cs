using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.Viber.Client.Requests;
using FLChat.Viber.Client.Types;
using FLChat.Core;


namespace FLChat.Viber.Bot.Algorithms
{
    public interface IConversationStartedStrategy
    {
        SendMessageRequest Process(ChatEntities entities, CallbackData callbackData, DeepLinkResult deepLinkResult);
    }
}
