using FLChat.DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class GroupInfoShort
    {
        protected readonly Group _group;
        public GroupInfoShort(Group group)
        {
            _group = group;
        }

        //  Все свойства для передачи
        //  Базовые
        //      [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
        //[IsDeleted] bit NOT NULL default 0,
        //[Name] nvarchar(255) NULL,
        //[CreatedByUserId] uniqueidentifier NOT NULL,
        //[CreatedDate] datetime NOT NULL DEFAULT GETUTCDATE(),
        //[IsEqual]

        [JsonProperty(PropertyName = "group_id")]
        public Guid Id => _group.Id;
        [JsonProperty(PropertyName = "name")]
        public string Name => _group.Name;
        [JsonProperty(PropertyName = "created_by_user_id")]
        public Guid CreatedByUserId => _group.CreatedByUserId;
        [JsonProperty(PropertyName = "is_equal")]
        public bool IsEqual => _group.IsEqual;
        [JsonProperty(PropertyName = "created_date")]
        public DateTime CreatedDate => _group.CreatedDate;
    }
}
