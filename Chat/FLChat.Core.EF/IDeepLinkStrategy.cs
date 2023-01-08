using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core
{
    /// <summary>
    /// Process deep link
    /// </summary>
    public interface IDeepLinkStrategy
    {
        bool AcceptDeepLink(
            ChatEntities entities,
            IDeepLinkData message,
            out User user,
            out DAL.Model.Message answerTo,
            out object context,
            out IDeepLinkStrategy sender
            );

        void AfterAddTransport(ChatEntities entities,
            IDeepLinkData message,
            Transport transport,
            object context);
    }
}
