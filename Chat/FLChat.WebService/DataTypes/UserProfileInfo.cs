using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.Utils;
using Newtonsoft.Json.Serialization;

namespace FLChat.WebService.DataTypes
{   
    public class UserProfileInfo : UserInfoBase
    {
        public class UserExt
        {
            public User User { get; set; }
            public string Comment { get; set; }
            public bool BroadcastProhibition { get; set; }
            public bool PersonalProhibition { get; set; }
            public bool? HasChilds { get; set; }
        }

        public UserProfileInfo(UserExt user, ChatEntities entities, Guid requestedUserId) : this(user/*, entities*/)
        {
            IsMe = user.User.Id == requestedUserId;
            //User requestedUser = user.User;
            if (IsMe.Value == false)
            {
                //requestedUser = entities.User.Where(u => u.Id == requestedUserId).FirstOrDefault();
                ParentDepth = entities
                        .User_GetParents(requestedUserId, null)
                        .Where(u => u.UserId == user.User.Id)
                        .Select(u => (int?)Math.Abs(u.Deep))
                        .SingleOrDefault();
                if (ParentDepth.HasValue == false) {
                    ChildDepth = entities
                        .User_GetParents(user.User.Id, null)
                        .Where(u => u.UserId == requestedUserId)
                        .Select(u => (int?)Math.Abs(u.Deep))
                        .SingleOrDefault();
                }
            }

            PersonalProhibitionList = user.User.PersonalProhibitionSlave.Select(a => new UserInfoSimple(a));
            BroadcastProhibitionList = user.User.BroadcastProhibition.Select(a => new UserInfoSimple(a));
        }

        public UserProfileInfo(UserExt user/*, ChatEntities entities = null*/) : base(user.User/*, entities*/)
        {
            Comment = user.Comment;
            BroadcastProhibition = user.BroadcastProhibition;
            PersonalProhibition = user.PersonalProhibition;
            HasChilds = user.HasChilds;            
        }

        public UserProfileInfo(User user) : base(user/*, null*/)
        {
            Comment = null;
        }

        //public UserProfileInfo(User user, ChatEntities entities = null) : base(user)
        //{
        //    var _entities = entities ?? new ChatEntities();
        //    string code = "1A2B3C4D5E";
        //    InviteLink = new Invite()
        //    {
        //        Code = code,
        //        Url = Settings.Values.GetValue("INVITE_LINK", "https://chat.faberlic.com/external/%code%").Replace("%code%", code),
        //        InviteButtons = MakeInviteLinks(_entities, code),
        //    };
        //}

        //  Все свойства для передачи
        //  Базовые
        //[JsonProperty(PropertyName = "user_id")]
        //public Guid Id => _user.Id;        
        //[JsonProperty(PropertyName = "full_name")]
        //public string FullName => _user.FullName;
        //[JsonProperty(PropertyName = "is_consultant")]
        //public bool IsConsultant => _user.IsConsultant;
        //  Дополнительные
        [JsonProperty(PropertyName = "registration_date")]
        public DateTime? RegistrationDate => _user.RegistrationDate;        
        //[JsonProperty(PropertyName = "sign_up_date")]
        //public DateTime? SignUpDate => _user.SignUpDate;
        [JsonProperty(PropertyName = "phone")]
        public string Phone => _user.Phone;
        [JsonProperty(PropertyName = "email")]
        public string Email => _user.Email;
        //[JsonProperty(PropertyName = "login")]
        //public string Login => _user.Login;        
        [JsonProperty(PropertyName = "owner_user_id")]
        public Guid? OwnerUserId => _user.OwnerUserId;
        [JsonProperty(PropertyName = "default_transport_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Nullable<TransportKind> DefaultTransportTypeId => (TransportKind?)_user.DefaultTransportTypeId;

        [JsonProperty(PropertyName = "lo_bonus_scores")]
        public decimal? LoBonusScores => _user.LoBonusScores;

        [JsonProperty(PropertyName = "period_wo_lo")]
        public int? PeriodWoLo => _user.PeriodsWolo;

        [JsonProperty(PropertyName = "olg_bonus_scores")]
        public decimal? OlgBonusScores => _user.OlgBonusScores;

        [JsonProperty(PropertyName = "go_bonus_scores")]
        public decimal? GoBonusScores => _user.GoBonusScores;

        [JsonProperty(PropertyName = "cashback_balance")]
        public decimal? CashBackBalance => _user.CashBackBalance;

        [JsonProperty(PropertyName = "fl_club_points")]
        public decimal? FLClubPoints => _user.FLClubPoints;

        [JsonProperty(PropertyName = "fl_club_points_burn")]
        public decimal? FLClubPointsBurn => _user.FLClubPointsBurn;

        // Информация о доступном транспорте
        [JsonProperty(PropertyName = "transports")]
        public IEnumerable<string> Transports => _user.AvailableTransports .Select(x => x.ToString());
        //        [JsonConverter(typeof(StringEnumConverter))]
        //public IEnumerable<TransportKind> Transports => _user.GetTransports().Select(x => x.ToString());
        // Информация об активных сегментах
        [JsonProperty(PropertyName = "segments")]
        public IEnumerable<string> Segments => _user.SegmentNames;

        //  Что есть ещё, но не пересылается
        //public bool Enabled => _user.Enabled;
        //public Guid? PartnerId => _user.PartnerId;
        //public DateTime InsertDate => _user.InsertDate;
        //public string PswHash => _user.PswHash;    
        
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; }

        [JsonProperty(PropertyName = "location")]
        public LocationInfo Location => _user.CityId != null ? new LocationInfo(_user.City, _user.ZipCode) : null;

        [JsonProperty(PropertyName = "number")]
        public int? FLUserNumber => _user.FLUserNumber;

        [JsonProperty(PropertyName = "birthday")]
        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyy-MM-dd" })]
        public DateTime? Birthday => _user.Birthday;

        [JsonProperty(PropertyName = "is_director")]
        public bool IsDirectory => _user.IsDirector;

        [JsonProperty(PropertyName = "last_order_date")]
        [JsonConverter(typeof(DateTimeConverter), new object[] { "yyyy-MM-dd" })]
        public DateTime? LastOrderDate => _user.LastOrderDate;

        [JsonProperty(PropertyName = "broadcast_prohibition")]
        public bool BroadcastProhibition { get; }

        [JsonProperty(PropertyName = "personal_prohibition")]
        public bool PersonalProhibition { get; }

        /// <summary>
        /// Parent user depth, if user is parent of requested user, otherwise null
        /// </summary>
        [JsonProperty(PropertyName = "parent_depth")]
        public int? ParentDepth { get; }

        /// <summary>
        /// child user depth, if user is child of requested user, otherwise null
        /// </summary>
        [JsonProperty(PropertyName = "child_depth")]
        public int? ChildDepth { get; }

        /// <summary>
        /// true if that profile is profile of requested user
        /// </summary>
        [JsonProperty(PropertyName = "is_me")]
        public bool? IsMe { get; }

        /// <summary>
        /// invite_link 
        /// </summary>
        [JsonProperty(PropertyName = "invite_link")]
        public Invite InviteLink { get; set; }


        [JsonProperty(PropertyName = "personal_prohibition_list")]
        public IEnumerable<UserInfoSimple> PersonalProhibitionList { get; }


        [JsonProperty(PropertyName = "broadcast_prohibition_list")]
        public IEnumerable<UserInfoSimple> BroadcastProhibitionList { get; }


        public class InviteLinkButton
        {
            [JsonProperty(PropertyName = "transport", Required = Required.Default)]
            [JsonConverter(typeof(StringEnumConverter))]
            public TransportKind Transport { get; set; }

            [JsonProperty(PropertyName = "url", Required = Required.Default)]
            public string Url { get; set; }
        }

        public class Invite
        {
            [JsonProperty(PropertyName = "code", Required = Required.Default)]
            public string Code { get; set; }

            [JsonProperty(PropertyName = "url", Required = Required.Default)]
            public string Url { get; set; }

            [JsonProperty(PropertyName = "invite_buttons", Required = Required.Default)]
            public IEnumerable<InviteLinkButton> InviteButtons { get; set; }

            public static Invite Make(ChatEntities entities, string code, string urlPattern)
                => new Invite() {
                    Code = code,
                    Url = urlPattern.Replace("%code%", code),
                    InviteButtons = entities
                        .TransportType
                        .Where(tt => tt.Enabled && tt.DeepLink != null)
                        .ToArray()
                        .Select(tt => new InviteLinkButton() {
                            Transport = tt.Kind,
                            Url = tt.DeepLink.Replace("%code%", code),
                        })
                        .ToArray()
                };
        }
    }
}
