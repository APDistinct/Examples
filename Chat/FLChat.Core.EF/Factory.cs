using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core.Algorithms;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public static class Factory
    {
        public static IDeepLinkStrategy CreateDeepLinkStrategy(bool InviteLinkCreateUser = true, bool CommonLinkCreateUser = true)
        {
            List<IDeepLinkStrategy> linkStrategys = new List<IDeepLinkStrategy>();
            if (Settings.IsInviteLinkWork)
            {
                linkStrategys.Add(new InviteLinkStrategy(InviteLinkCreateUser));
            }
            linkStrategys.Add(new LiteDeepLinkStrategy());
            linkStrategys.Add(new WebChatDeepLinkStrategy());
            if (Settings.IsCommonLinkWork)
            {
                linkStrategys.Add(new CommonLinkStrategy(CommonLinkCreateUser));
            }
            
            return new ComplexDeepLinkStrategy(linkStrategys.ToArray());

            //return new ComplexDeepLinkStrategy(new IDeepLinkStrategy[] {
            //    new InviteLinkStrategy(InviteLinkCreateUser),
            //    new LiteDeepLinkStrategy(),
            //    new WebChatDeepLinkStrategy(),
            //    new CommonLinkStrategy(CommonLinkCreateUser)
            //});
        }
    }
}
