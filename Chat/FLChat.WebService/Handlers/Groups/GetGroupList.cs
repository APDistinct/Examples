using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers
{
    /// <summary>
    /// Return User by Id
    /// </summary>
    public class GetGroupList : /*GetUserInfoBase,*/ IObjectedHandlerStrategy<string, IEnumerable<GroupInfoShort>>
    {
        private readonly bool _getAll;

        public GetGroupList(bool getAll = false)
        {
            _getAll = getAll;
        }

        public bool IsReusable => throw new NotImplementedException();

        public IEnumerable<GroupInfoShort> ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input)
        {
            if (Guid.TryParse(input, out Guid id))
                return ProcessRequest(entities, currUserInfo.UserId, id, _getAll);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {input} not found"));
        }

        public IEnumerable<GroupInfoShort> ProcessRequest(ChatEntities entities, Guid userId, Guid id, bool getAll = false)
        {
            //User user = entities.User
            //   .Where(u => (u.Id == id) && (getAll || u.Enabled))
            //   .Include(t => t.Transports)
            //   .Include("Transports.TransportType")
            //   .SingleOrDefault();
            //if (user != null)
            {
                return entities.GroupMember
                .Where(u => (u.UserId == id)).Select(x => new GroupInfoShort(x.Group));
            }
            //throw new ErrorResponseException(
            //    (int)HttpStatusCode.NotFound,
            //    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {id} not found"));
        }        
    }    
}
