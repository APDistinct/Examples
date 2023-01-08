using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public static class Global
    {
        /// <summary>
        /// System bot id
        /// </summary>
        public static Guid SystemBotId => Guid.Empty;
        public static Guid SmsBotId => Guid.Parse(Guid.Empty.ToString().Remove(Guid.Empty.ToString().Length -2,1) + "1");
    }
}
