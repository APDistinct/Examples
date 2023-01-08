using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace FLChat.WebService.Handlers
{

    /// <summary>
    /// Return User by Id
    /// </summary>
    public class LeaveGroup : IObjectedHandlerStrategy<string, object>
    {
        public bool IsReusable => true;
        private readonly bool _getAll;

        public LeaveGroup(bool getAll = false)
        {
            _getAll = getAll;
        }        

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
            DAL.Model.User user = entities.User
                .Where(u => (u.Id == userId) && (getAll || u.Enabled))
                //.Include(t => t.UserAvatar)
                .SingleOrDefault();
            if (user != null)
            {
                entities.GroupMember.Where(t => t.GroupId == id && t.UserId == userId).Delete();
                entities.SaveChanges();                    
                
                return;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {id} not found"));
        }
    }
}
