using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

using FLChat.DAL;

namespace FLChat.WebService.Handlers.Auth
{
    public class VerifyTokenStrategy : CheckAuthHttpHandler.IVerifyTokenStrategy
    {
        private ITokenRecoverFactory<TokenPayload> _factory;
        private bool _bot;

        public VerifyTokenStrategy(ITokenRecoverFactory<TokenPayload> factory, bool bot = false) {
            _factory = factory;
            _bot = bot;
        }

        public VerifyTokenStrategy(bool bot = false) : this(new AuthTokenFactory(), bot) {
        }


        public IUserAuthInfo CheckToken(string authorizationHeader, ChatEntities entities, out bool isExpired) {
            TokenPayload token;
            try {
                token = _factory.Decode(authorizationHeader);
            } catch (Exception) {
                isExpired = false;
                return null;
            }

            if (token.IsExpired) {
                isExpired = true;
                return null;
            }

            isExpired = false;

            if (_bot != (token.UserId == Guid.Empty))
                return null;

            if (entities.AuthToken.Where(t => t.Token == authorizationHeader).Any())
                return token;

            return null;
        }
    }
}
