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
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.User
{
    /// <summary>
    /// Return User by Id
    /// </summary>
    public class SetUserInfo : IObjectedHandlerStrategy<JObject, UserProfileInfo>
    {
        private readonly static Dictionary<string, string> dicNames;

        private readonly bool _getAll;
        public const string KeyFieldName = "id";

        public bool IsReusable => true;

        static SetUserInfo() {
            string[] fields = new string[] {
                nameof(UserProfileInfo.FullName),
                nameof(UserProfileInfo.Phone),
                nameof(UserProfileInfo.Email),
                nameof(UserProfileInfo.DefaultTransportTypeId),
                nameof(UserProfileInfo.Comment),
                nameof(UserProfileInfo.OwnerUserId),
                nameof(UserProfileInfo.BroadcastProhibition),
                nameof(UserProfileInfo.PersonalProhibition),
                nameof(UserProfileInfo.Rank)
            };

            dicNames = typeof(UserProfileInfo).GetJsonPropertyName(fields);
        }


        public SetUserInfo(bool getAll = false)
        {
            _getAll = getAll;
        }

        public UserProfileInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, JObject input)
        {
            string sid = (string)input[KeyFieldName];
            //if (input == null)
            if (string.IsNullOrEmpty(sid))
                return ProcessRequest(entities, currUserInfo, currUserInfo.UserId, input, _getAll);
            
            //string sid = (string)input[KeyFieldName];
            if (Guid.TryParse(sid, out Guid id))
                return ProcessRequest(entities, currUserInfo, id, input, _getAll);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {sid} not found"));
        }

        private static UserProfileInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, Guid userId, JObject json, bool getAll = false) {
            DAL.Model.User user = entities.User
                .Where(u => (u.Id == userId) && (getAll || u.Enabled))
                .SingleOrDefault();
            if (user != null)
                return Apply(entities, currUserInfo, user, json);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {userId} not found"));
        }

        public static UserProfileInfo Apply(ChatEntities entities, IUserAuthInfo currUserInfo, DAL.Model.User user, JObject json) {
            ApplyChanges(entities, currUserInfo, user, json);
            entities.SaveChanges();
            return GetUserInfo.MakeProfileInfo(entities, currUserInfo, user.Id, false);
        }

        public static void ApplyChanges(ChatEntities entities, IUserAuthInfo currUserInfo, DAL.Model.User user, JObject jsonObject) {
            foreach (var property in jsonObject.Properties()) {
                string propName = property.Name;
                try {
                    if (dicNames.ContainsKey(propName)) {
                        var fieldname = dicNames[propName];
                        switch (fieldname) {
                            case nameof(UserProfileInfo.FullName) /*"full_name"*/:
                                user.FullName = (string)property.Value;
                                break;
                            case nameof(UserProfileInfo.Phone) /*"phone"*/:
                                user.Phone = (string)property.Value;
                                break;
                            case nameof(UserProfileInfo.Email) /*"email"*/:
                                user.Email = (string)property.Value;
                                break;
                            case nameof(UserProfileInfo.DefaultTransportTypeId):
                                SetDefaultTransport(entities, user, property.Value);
                                break;
                            case nameof(UserProfileInfo.Comment):
                                SetComment(entities, currUserInfo, user, (string)property.Value);
                                break;
                            case nameof(UserProfileInfo.OwnerUserId):
                                SetOwner(entities, currUserInfo, user, (Guid?)property.Value);
                                break;
                            case nameof(UserProfileInfo.BroadcastProhibition):
                                SetBroadcastProhibition(entities, currUserInfo, user, (bool)property.Value);
                                break;
                            case nameof(UserProfileInfo.PersonalProhibition):
                                SetPersonalProhibition(entities, currUserInfo, user, (bool)property.Value);
                                break;
                            case nameof(UserProfileInfo.Rank):
                                SetRank(entities, currUserInfo, user, (string)property.Value);
                                break;
                        }
                    }
                } catch (ErrorResponseException) {
                    throw;
                } catch (Exception e) {
                    throw new ErrorResponseException(
                        HttpStatusCode.BadRequest,
                        new ErrorResponse(ErrorResponse.Kind.input_data_error, $"Input data field {propName} are not correct", e.ToString()));
                }
            }            
        }

        private static void SetDefaultTransport(ChatEntities entities, DAL.Model.User user, JToken value) {
            TransportKind tk = (TransportKind)Enum.Parse(typeof(TransportKind), (string)value);

            DAL.Model.TransportType tt = entities.TransportType.Where(t => t.Id == (int)tk && t.Enabled).SingleOrDefault();
            if (tt == null)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    $"Transport {(string)value} has not found");

            if (tt.CanSelectAsDefault == false || tt.VisibleForUser == false)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    $"Transport {(string)value} can't be select as default");

            DAL.Model.Transport transport = entities.Transport.Where(t => t.Enabled && t.TransportTypeId == (int)tk).FirstOrDefault();
            if (transport == null)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    $"User has't has not this type of transport");

            user.DefaultTransportTypeId = tt.Id;
        }

        private static void SetComment(ChatEntities entities, IUserAuthInfo currUserInfo, DAL.Model.User user, string cmtText) {
            Comment cmt = user.CommentsOnMe.Where(c => c.UserId == currUserInfo.UserId).FirstOrDefault();

            if (String.IsNullOrEmpty(cmtText)) {
                if (cmt != null)
                    entities.Entry(cmt).State = EntityState.Deleted;
            } else {
                if (currUserInfo.UserId == user.Id)
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_support, "Can't set comment for himself");

                //new comment text is not empty
                if (cmt == null) {
                    entities.Comment.Add(new Comment() {
                        Text = cmtText,
                        UserId = currUserInfo.UserId,
                        UponUserId = user.Id
                    });
                } else
                    cmt.Text = cmtText;
            }
        }

        private static void SetOwner(ChatEntities entities, IUserAuthInfo userAuthInfo, DAL.Model.User user, Guid? newOwnerId) {
            if (newOwnerId.HasValue) {
                if (entities
                    .User_GetParents(newOwnerId.Value, null)
                    .Select(r => r.UserId)
                    .ToArray()
                    .Contains(user.Id))
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Can't assign owner id, because new owner contains in user structure");
                user.OwnerUserId = newOwnerId.Value;
            } else {
                user.OwnerUserId = null;
                user.OwnerUser = null;
            }
        }

        private static void SetBroadcastProhibition(ChatEntities entities, IUserAuthInfo userAuthInfo, DAL.Model.User user, 
            bool broadcastProhibition) {
            DAL.Model.User authUser = entities.User.Where(u => u.Id == userAuthInfo.UserId).Single();
            if (broadcastProhibition) {
                if (!user.BroadcastProhibitionBy.Select(u => u.Id).Contains(userAuthInfo.UserId))                    
                    user.BroadcastProhibitionBy.Add(authUser);
            } else {
                if (user.BroadcastProhibitionBy.Select(u => u.Id).Contains(userAuthInfo.UserId))
                    user.BroadcastProhibitionBy.Remove(authUser);
            }
        }

        private static void SetPersonalProhibition(ChatEntities entities, IUserAuthInfo userAuthInfo, DAL.Model.User user, bool personalProhibition)
        {
            DAL.Model.User authUser = entities.User.Where(u => u.Id == userAuthInfo.UserId).Single();
            if (personalProhibition)
            {
                if (!user.PersonalProhibitionMain.Select(u => u.Id).Contains(authUser.UserId))
                    user.PersonalProhibitionMain.Add(authUser);
            }
            else
            {
                if (user.PersonalProhibitionMain.Select(u => u.Id).Contains(authUser.UserId))
                    user.PersonalProhibitionMain.Remove(authUser);
            }
        }

        private static void SetRank(ChatEntities entities, IUserAuthInfo userAuthInfo, DAL.Model.User user, string rank)
        {
            if (string.IsNullOrWhiteSpace(rank))
            {
                user.RankId = null;
                return;
            }

            DAL.Model.Rank tt = entities.Rank.Where(t => t.Name == rank ).SingleOrDefault();
            user.Rank = tt ?? throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    $"Rank {rank} has not found");
        }
    }
}
