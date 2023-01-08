using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Info
{
    public class GetRank : IObjectedHandlerStrategy<object, RankGetResponse>
    {
        public bool IsReusable => true;

        public RankGetResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, object input)
        {
            return new RankGetResponse()
            {
                Ranks = entities.Rank.Select(r => r.Name).ToList()
            };
        }
    }
}
