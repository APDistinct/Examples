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
using Z.EntityFramework.Plus;

namespace FLChat.WebService.Handlers
{
    /// <summary>
    /// Return User by Id
    /// </summary>
    public class DelGroup : /*GetUserInfoBase,*/ IObjectedHandlerStrategy<string, object>
    {
        private readonly bool _getAll;

        public DelGroup(bool getAll = false)
        {
            _getAll = getAll;
        }

        public bool IsReusable => throw new NotImplementedException();

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input)
        {
            if (Guid.TryParse(input, out Guid id))
            {
                ProcessRequest(entities, currUserInfo.UserId, id, _getAll);
                return null;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {input} not found"));
        }

        public void ProcessRequest(ChatEntities entities, Guid userId, Guid id, bool getAll = false)
        {
            Group group = entities.Group
                .Where(u => (u.Id == id) && (getAll || !u.IsDeleted))
                .Include(t => t.Members)
                //.Include("Transports.TransportType")
                .SingleOrDefault();
            if (group != null)
            {
                //entities.GroupMember.Where(t => t.GroupId == id).Delete();
                group.IsDeleted = true;
                entities.SaveChanges();
                return;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {id} not found"));
        }
    }

}
