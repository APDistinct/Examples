using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Auth
{
    public class GetConfig : IObjectedHandlerStrategy<object, ConfigResponse>
    {
        public bool IsReusable => true;

        public ConfigResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, object input)
        {
            return new ConfigResponse() { Config = Settings.Values.GetValue("FRONT_CONFIG", null) };
        }
    }
}
