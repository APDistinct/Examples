using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class Settings
    {
        private static Lazy<SettingsDict> _settings = new Lazy<SettingsDict>(SettingsLoader, true);

        public static SettingsDict Values => _settings.Value;

        private static SettingsDict SettingsLoader() {
            using (ChatEntities entities = new ChatEntities()) {
                return new SettingsDict(entities.Settings.ToDictionary(s => s.Name, s => s.Value));
            }
        }

        public static bool IsInviteLinkWork => CheckValue("INVITELINK_WORK", "1");
        public static bool IsCommonLinkWork => CheckValue("COMMONLINK_WORK", "1");
        public static bool IsGuardWork => !CheckValue("TMP_CACHE", "Simple");

        private static bool CheckValue(string param, string val)
        {
            bool ret = false;
            string sget = Values.GetValue(param, null);
            if(sget != null)
            {
                ret = sget == val;
            }
            return ret;
        }
    }
}
