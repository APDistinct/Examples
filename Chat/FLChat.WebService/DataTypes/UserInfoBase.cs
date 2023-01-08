using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Another tables which used are: [Dir].[Rank]
    /// </summary>
    public abstract class UserInfoBase
    {
        [JsonIgnore]
        public readonly User _user;

        public UserInfoBase(User user)
        {
            _user = user;
        }

        //  Все свойства для передачи
        //  Базовые
        [JsonProperty(PropertyName = "user_id")]
        public Guid Id => _user.Id;
        [JsonProperty(PropertyName = "full_name")]
        public string FullName => _user.FullName;
        [JsonProperty(PropertyName = "is_consultant")]
        public bool IsConsultant => _user.IsConsultant;
        [JsonProperty(PropertyName = "avatar_upload_date")]
        public DateTime? AvatarUploadDate => _user.AvatarUploadDate;

        [JsonProperty(PropertyName = "rank")]
        public string Rank { get { return _user.Rank?.Name; } set { RankName = value; } }

        [JsonIgnore]
        public string RankName { get; set; }

        [JsonProperty(PropertyName = "online", Required = Required.AllowNull)]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserOnlineStatus? Online =>
            _user.LastGetEvents.HasValue 
                ? ((DateTime.UtcNow - _user.LastGetEvents.Value).TotalSeconds < Settings.Values.OnlinePeriodSec 
                    ? UserOnlineStatus.Online : UserOnlineStatus.Offline)
                : (UserOnlineStatus?)null;


        [JsonProperty(PropertyName = "has_childs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? HasChilds { get; set; }

        //  Дополнительные
        //[JsonProperty(PropertyName = "registration_date")]
        //public DateTime? RegistrationDate => _user.RegistrationDate;
        //[JsonProperty(PropertyName = "sign_up_date")]
        //public DateTime? SignUpDate => _user.SignUpDate;
        //[JsonProperty(PropertyName = "phone")]
        //public string Phone => _user.Phone;
        //[JsonProperty(PropertyName = "email")]
        //public string Email => _user.Email;
        //[JsonProperty(PropertyName = "login")]
        //public string Login => _user.Login;
        //[JsonProperty(PropertyName = "owner_user_id")]
        //public Guid? OwnerUserId => _user.OwnerUserId;

        //  Что есть ещё, но не пересылается
        //public bool Enabled => _user.Enabled;
        //public Guid? PartnerId => _user.PartnerId;
        //public DateTime InsertDate => _user.InsertDate;
        //public string PswHash => _user.PswHash;

        // Информация о доступном транспорте
        //public bool EnabledInnerTransport => _user.EnabledInnerTransport;
    }
}
