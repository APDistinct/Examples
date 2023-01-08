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
    public class GetGroup : IObjectedHandlerStrategy<string, GroupInfo>
    {
        private readonly bool _getAll;

        public GetGroup(bool getAll = false)
        {
            _getAll = getAll;
        }

        public bool IsReusable => true;

        public GroupInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input)
        {
            if (Guid.TryParse(input, out Guid id))
                return ProcessRequest(entities, currUserInfo.UserId, id, _getAll);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {input} not found"));
        }

        public GroupInfo ProcessRequest(ChatEntities entities, Guid userId, Guid id, bool getAll = false)
        {
            Group group = entities.Group
                .Where(u => (u.Id == id) && (getAll || !u.IsDeleted))
                .Include(t => t.Members)
                //.Include("Transports.TransportType")
                .SingleOrDefault();
            if (group != null)
                return new GroupInfo(group);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {id} not found"));
        }
    }
    
}
