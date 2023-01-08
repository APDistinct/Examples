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
    public class CreateGroup : SetGroupBase, IObjectedHandlerStrategy<GroupInfoSet, GroupInfo>
    {
        private readonly bool _getAll;

        public CreateGroup(bool getAll = false)
        {
            _getAll = getAll;
        }

        //public bool IsReusable => true;

        public GroupInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, GroupInfoSet input)
        {
            //if (Guid.TryParse(input, out Guid id))
            {
                Group group = GetNew(entities, currUserInfo);
                return ProcessRequest(entities, input, group);
            }

            //throw new ErrorResponseException(
            //    (int)HttpStatusCode.NotFound,
            //    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Group with id {input} not found"));
        }

        Group GetNew(ChatEntities entities, IUserAuthInfo currUserInfo)
        {
            Group group = new Group()
            {
                CreatedByUserId = currUserInfo.UserId,
            };
            entities.Group.Add(group);
            entities.SaveChanges();
            Guid guid = group.Id;
            /*GroupMember groupMember = */entities.GroupMember.Add(new GroupMember()
            {
                GroupId = guid,
                UserId = currUserInfo.UserId,
                IsAdmin = true,
            });
            entities.SaveChanges();
            group = entities.Group.Where(g => g.Id == guid)
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
