using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jose;
using Newtonsoft.Json;

namespace FLChat.WebService.Handlers.Auth
{
    /// <summary>
    /// Token factory
    /// Generate JWT token
    /// for more information see    https://github.com/dvsekhvalnov/jose-jwt
    ///                             https://jwt.io/
    /// </summary>
    public class AuthTokenFactory : ITokenRecoverFactory<TokenPayload>
    {
        private byte []secretKey = new byte[] { 128, 95, 94, 86, 211, 13, 155, 89, 15, 186, 241, 26, 147, 49, 198, 69, 138, 37, 232, 86, 115, 41, 37, 162, 115, 221, 54, 59, 86, 165, 22, 92 };

        public string Gen(Guid id, DateTime issueDate, int expireBy) {
            var payload = new Dictionary<string, object>() {
                { "id", id },
                { "iss", issueDate.ToString("o") },
                { "exp", expireBy }
            };
            return JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        }

        public TokenPayload Decode(string token) {
            string json = JWT.Decode(token, secretKey);
            return JsonConvert.DeserializeObject<TokenPayload>(json);
        }
    }
}
