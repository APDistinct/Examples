using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Information about deep link
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DeepLinkResponse
    {
        [JsonProperty(PropertyName = "code", Required = Required.Always)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "user", Required = Required.AllowNull)]
        public UserProfileInfo User { get; set; }

        public class InviteLink
        {
            [JsonProperty(PropertyName = "transport", Required = Required.Always)]
            [JsonConverter(typeof(StringEnumConverter))]
            public TransportKind Transport { get; set; }

            [JsonProperty(PropertyName = "url", Required = Required.Always)]
            public string Url { get; set; }

            [JsonProperty(PropertyName = "status", Required = Required.Always)]            
            [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
            public TransportStatus Status { get; set; }
        }

        [JsonProperty(PropertyName = "invite_buttons", Required = Required.Always)]
        public IEnumerable<InviteLink> InviteButtons { get; set; }

        public static TransportStatus GetStatus(bool? state)
        {
            TransportStatus status = TransportStatus.None;

            switch (state)
            {
                case true:
                    status = TransportStatus.Subscribed;
                    break;
                case false:
                    status = TransportStatus.Unsubscribed;
                    break;
                case null:
                default:
                    status = TransportStatus.None;
                    break;
            }
            return status;
        }
    }

    public static class DeepLinkResponseExtentions
    {
        public static DeepLinkResponse.InviteLink[] ToInviteLinks(this User user, ChatEntities entities, string code) {
            if (user != null)
                entities.Entry(user).Collection(u => u.Transports).Load();

            return entities
                .TransportType
                .Where(tt => tt.Enabled && tt.DeepLink != null)
                .ToArray()
                .Select(tt => new DeepLinkResponse.InviteLink() {
                    Transport = tt.Kind,
                    Url = tt.DeepLink.Replace("%code%", code),
                    Status = user != null
                        ? DeepLinkResponse.GetStatus(user.Transports.Where(x => x.TransportTypeId == (int)tt.Kind).Select(x => (bool?)x.Enabled).FirstOrDefault())
                        : TransportStatus.None,
                })
                .ToArray();
        }
    }
}
