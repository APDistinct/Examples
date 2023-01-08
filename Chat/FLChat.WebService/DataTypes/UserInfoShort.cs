using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using FLChat.WebService.Utils;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class UserInfoShort : UserInfoBase
    {
        public UserInfoShort(User user, MessageInfo lastMessage, int unreadCnt, User requestedUser = null) : base(user) {
            LastMessage = lastMessage;
            UnreadCount = unreadCnt;
            if (requestedUser != null)
            {
                BroadcastProhibition = requestedUser.BroadcastProhibition.Contains(user);
                PersonalProhibition = requestedUser.PersonalProhibitionMain.Contains(user);
            }
        }

        [JsonProperty(PropertyName = "last_message")]
        public MessageInfo LastMessage { get; }

        [JsonProperty(PropertyName = "unread_count")]
        public int UnreadCount { get; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// True if requested user was set BroadcastProhibition for this user's structure
        /// </summary>
        [JsonProperty(PropertyName = "broadcast_prohibition")]
        public bool BroadcastProhibition { get; }

        [JsonProperty(PropertyName = "personal_prohibition")]
        public bool PersonalProhibition { get; }

        /// <summary>
        /// User includes in broadcast prohibition user's structure
        /// </summary>
        [JsonProperty(PropertyName = "broadcast_prohibition_structure", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? BroadcastProhibitionStructure { get; set; }
    }

    public static class UserInfoShortExtentions
    {
        /// <summary>
        /// Make short info for single user
        /// </summary>
        /// <param name="user">user database object</param>
        /// <param name="currUserId">current user id</param>
        /// <param name="lastMessages">dictionary of last messages</param>
        /// <returns>ShortUserInfo for <paramref name="user"/></returns>
        public static UserInfoShort ToUserInfoShort(
            this User user, 
            User currUser,
            Dictionary<Guid, MessageToUser> lastMessages,
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags = null,
            HashSet<Guid> broadcastProhibitionStructure = null,
            HashSet<Guid> hasChilds = null) {

            int cnt = unread != null ? unread.GetValueOrDefault(user.Id) : 0;

            MessageToUser lastMessage = null;
            lastMessages?.TryGetValue(user.Id, out lastMessage);

            List<string> tag = null;
            tags?.TryGetValue(user.Id, out tag);
            UserInfoShort ui = new UserInfoShort(user, lastMessage?.ToPersonalMessageInfo(currUser.Id), cnt, currUser) {
                Tags = tag,
                BroadcastProhibitionStructure = broadcastProhibitionStructure != null
                    ? broadcastProhibitionStructure.Contains(user.Id)
                    : (bool?)null,
            };
            if (hasChilds != null)
                ui.HasChilds = hasChilds.Contains(user.Id);
            return ui;
        }

        /// <summary>
        /// Make list of user's short info
        /// </summary>
        /// <param name="users">List of user database object</param>
        /// <param name="currUserId">Current user Id</param>
        /// <param name="lastMessages">dictionary of last messages</param>
        /// <returns>List of UserInfoShort</returns>
        public static IEnumerable<UserInfoShort> ToUserInfoShort(
            this IEnumerable<User> users, 
            User currUser, 
            Dictionary<Guid, MessageToUser> lastMessages,
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags = null,
            HashSet<Guid> broadcastProhibitionStructure = null,
            HashSet<Guid> hasChilds = null) {

            return users.Select(x => x.ToUserInfoShort(currUser, lastMessages, unread, tags, 
                broadcastProhibitionStructure, hasChilds));
        }
    }
}
