using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Handlers.User
{
    public class CreateUser : IObjectedHandlerStrategy<JObject, UserProfileInfo>
    {
        public bool IsReusable => true;

        public UserProfileInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, JObject input) {
            //using (var trans = entities.Database.BeginTransaction()) {
                DAL.Model.User u = new DAL.Model.User() {
                    IsConsultant = true,
                    Enabled = true,
                };
                entities.User.Add(u);
                SetUserInfo.ApplyChanges(entities, currUserInfo, u, input);

                u.OwnerUserId = currUserInfo.UserId;
                entities.SaveChanges();

                //trans.Commit();
                return new UserProfileInfo(new UserProfileInfo.UserExt() { User = u });
            //}
        }
    }
}
