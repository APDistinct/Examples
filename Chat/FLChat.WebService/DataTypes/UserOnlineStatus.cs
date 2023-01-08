using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public enum UserOnlineStatus
    {
        [EnumMember(Value = "offline")]
        Offline,

        [EnumMember(Value = "online")]
        Online
    }
}
