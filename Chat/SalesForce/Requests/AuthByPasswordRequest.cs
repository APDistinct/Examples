using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesForce.Types;

namespace SalesForce.Requests
{    
    public class AuthByPasswordRequest : RequestBase<object, AuthResponse>
    {
        public AuthByPasswordRequest(string clientId, string secret, string userName, string password) 
            : base("/services/oauth2/token", System.Net.Http.HttpMethod.Post) {
            QueryParams = new Dictionary<string, string>() {
                { "client_id", clientId },
                { "client_secret", secret },
                { "grant_type", "password" },
                { "username", userName },
                { "password", password }
            };
        }

        public override object RequestBody => null;

        public override Dictionary<string, string> QueryParams { get; }
    }
}
