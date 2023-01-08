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
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Handlers
{
    /// <summary>
    /// Return User by Id
    /// </summary>
    public class SetGroup : SetGroupBase, IObjectedHandlerStrategy<GroupInfoSet, GroupInfo>
    {
        private readonly bool _getAll;
        public const string KeyFieldName = "id";

        public SetGroup(bool getAll = false)
        {
            _getAll = getAll;
        }

        //public bool IsReusable => true;

        public GroupInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, GroupInfoSet input)
        {
            string sid = input.Id;
            if (Guid.TryParse(sid, out Guid id))                
            {
                Group group = GetExist(entities, id);
                if(group != null)
                  return ProcessRequest(entities, input, group);
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {input} not found"));
        }

        Group GetExist(ChatEntities entities, Guid id)
        {
            Group group = new Group();
            group = entities.Group.Where(g => g.Id == id)
                .Include(t => t.Members/*"GroupMember"*/)
                .FirstOrDefault();
            return group;
        }

        //public GroupInfo ProcessRequest(ChatEntities entities, Guid userId, Guid id, bool getAll = false)
        //{
        //    Group group = entities.Group
        //        .Where(u => (u.Id == id) && (getAll || !u.IsDeleted))
        //        .Include(t => t.Members)
        //        //.Include("Transports.TransportType")
        //        .SingleOrDefault();
        //    if (group != null)
        //        return new GroupInfo(group);

        //    throw new ErrorResponseException(
        //        (int)HttpStatusCode.NotFound,
        //        new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {id} not found"));
        //}
    }

}
