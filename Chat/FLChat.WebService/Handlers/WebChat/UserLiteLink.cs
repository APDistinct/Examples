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
    public class UserLiteLink : LiteLink, IObjectedHandlerStrategy<string, DeepLinkResponse>
    {        
        public IDeepLinkGenerator Generator { get; }

        public UserLiteLink(IDeepLinkStrategy strategy = null, IDeepLinkGenerator generator = null) : base(strategy)
        {
            Generator = generator ?? new LiteDeepLinkStrategy(); 
        }

        public new DeepLinkResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input)
        {
            if (!int.TryParse(input, out int number))
                throw new ErrorResponseException((int)HttpStatusCode.BadRequest,
                    new ErrorResponse(ErrorResponse.Kind.input_data_error, $"Bad user's number <{input}> "));

            string code = Generator.Generate(new DAL.Model.User() { FLUserNumber = number });
            return base.ProcessRequest(entities, currUserInfo, code);
        }
    }
}
