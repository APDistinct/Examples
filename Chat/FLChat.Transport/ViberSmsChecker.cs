using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Transport
{
    public static class ViberSmsChecker
    {
        public static bool Check(MessageToUser mtu)
        {
            string specific = "WEBCHAT";
            //  ForwardMsgId не пустое и в Specific содержится строка WEBCHAT
            if (mtu.Message.Specific == null)
                return false;
            return (mtu.Message.Specific.Contains(specific) && mtu.Message.ForwardMsgId != null);
            //return ((mtu.Message.Specific?.Contains(specific) ?? false) && mtu.Message.ForwardMsgId != null);
        }
    }
}
