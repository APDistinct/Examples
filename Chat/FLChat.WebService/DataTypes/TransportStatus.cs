using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    [DataContract]
    public enum TransportStatus
    {
        //[EnumMember(Value = "Test")]
        /// <summary>
        /// неизвестен
        /// </summary>
        None = -1,

        /// <summary>
        /// пользователь известен в мессенджере, но отписан от бота
        /// </summary>
        Unsubscribed = 0,

        /// <summary>
        /// пользователь уже подписан в этом мессенджере
        /// </summary>
        Subscribed = 1,    
    }
}
