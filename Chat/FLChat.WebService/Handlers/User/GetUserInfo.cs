using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.InviteLink;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers
{
    /// <summary>
    /// Return User by Id
    /// </summary>
    public class GetUserInfo : IObjectedHandlerStrategy<string, UserProfileInfo>
    {
        private readonly bool _getAll;

        public bool IsReusable => true;

        public GetUserInfo(bool getAll = false)
        {
            _getAll = getAll;
        }        

        public UserProfileInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input)
        {
            if (input == null)
                return MakeProfileInfo(entities, currUserInfo, currUserInfo.UserId, _getAll);
            else if (Guid.TryParse(input, out Guid id))
                return MakeProfileInfo(entities, currUserInfo, id, _getAll);                

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound, 
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {input} not found"));
        }

        public static UserProfileInfo MakeProfileInfo(ChatEntities entities, IUserAuthInfo currUserInfo, Guid userId, bool getAll = false) {
            UserProfileInfo.UserExt user = entities.User
                .Where(u => (u.Id == userId) && (getAll || u.Enabled))
                .Include(t => t.Transports)
                .Include("Transports.TransportType")
                .Include(t => t.Rank)
                .Include(u => u.City)
                .Include(u => u.City.Region)
                .Include(u => u.City.Region.Country)
                //.Include(u => u.BroadcastProhibitionBy)
                .Select(u => new UserProfileInfo.UserExt() {
                    User = u,
                    Comment = currUserInfo.UserId == userId ? null :
                        (u.CommentsOnMe
                            .Where(c => c.UserId == currUserInfo.UserId)
                            .Select(c => c.Text)
                            .FirstOrDefault() ?? ""),
                    BroadcastProhibition = u.BroadcastProhibitionBy.Select(bp => bp.Id).Contains(currUserInfo.UserId),
                    PersonalProhibition = u.PersonalProhibitionMain.Select(bp => bp.Id).Contains(currUserInfo.UserId),
                    HasChilds = u.ChildUsers.Any()
                })
                .SingleOrDefault();
            if (user != null)
            {
                string code = userId.InviteLinkCode(); //"1A2B3C4D5E";
                string urlPattern = Settings.Values.GetValue("INVITE_LINK", "https://chat.faberlic.com/external/%code%");
                var upi = new UserProfileInfo(user, entities, currUserInfo.UserId) {
                    InviteLink = UserProfileInfo.Invite.Make(entities, code, urlPattern)
                };
                return upi;
            }
            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {userId} not found"));
        }
    }
}
