using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.Core.Algorithms;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.WebChat
{
    public class LiteLink : IObjectedHandlerStrategy<string, DeepLinkResponse>
    {
        private readonly IDeepLinkStrategy _strategy;

        public bool IsReusable => true;

        public LiteLink(IDeepLinkStrategy strategy = null) {
            _strategy = strategy ?? new LiteDeepLinkStrategy();
        }

        public DeepLinkResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            List<Transport> userTransport = new List<Transport>();

            if (!_strategy.AcceptDeepLink(entities, new FakeLinkData(input),
                out DAL.Model.User user, out DAL.Model.Message answ, out object context, out IDeepLinkStrategy sender))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.invalid_code, $"Invalid code {input}");

            return new DeepLinkResponse() {
                Code = input,
                User = user != null ? new UserProfileInfo(user) : null,
                InviteButtons = user.ToInviteLinks(entities, input)
            };
        }
    }
}
