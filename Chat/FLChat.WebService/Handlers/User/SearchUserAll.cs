using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.User
{
    public class SearchUserAll : IObjectedHandlerStrategy<SearchUserRequest, AdminUserSearchAllResponse>
    {
        public bool IsReusable => true;

        public int MaxCount { get; set; } = 100;

        public AdminUserSearchAllResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, SearchUserRequest input)
        {
            if (input == null || input.SearchValue == null)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Search parameter [search] must be present");
            if (input.SearchValue.Length < 3)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Length of search parameter [search] must be greater then 2");

            input.MaxCount = MaxCount;
            //
            //var qUsers = (from u in entities.User
            //              join c in entities.User_GetChilds(currUserInfo.UserId, null, null) on u.Id equals c.UserId
            //              select u);
            ////
            //String like = String.Concat("%", input.SearchValue, "%");
            //int? flUserNumber = null;
            //if (int.TryParse(input.SearchValue, out int number))
            //    flUserNumber = number;
            var qUsers = entities.User.FindString(input.SearchValue);                
            int? totalCount = null;
            if ((input.Offset ?? 0) == 0)
                totalCount = qUsers.Count();

            qUsers = qUsers
                .OrderByName()
                .TakePart(input);
            DAL.Model.User[] users = qUsers.ToArray();
            //Guid[] userIds = users.Select(u => u.Id).ToArray();
            
            return new AdminUserSearchAllResponse(users.ToArray(), partial: input)
            {
                TotalCount = totalCount
            };
        }
    }
}
