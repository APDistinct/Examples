using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public static class InstallHelper
    {
        public static string GetServiceNameAppConfig(Type type, string serviceName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(type).Location);
            return config.AppSettings.Settings[serviceName].Value;
        }
    }
}
