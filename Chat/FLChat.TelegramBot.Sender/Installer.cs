using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FLChat.TelegramBot.Sender
{

    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        //public string GetServiceNameAppConfig(string serviceName)
        //{
        //    var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(Installer)).Location);
        //    return config.AppSettings.Settings[serviceName].Value;
        //}

        public Installer() {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller {
                Account = ServiceAccount.LocalSystem
            };

            //string name = GetServiceNameAppConfig("ProjectName") ?? "";
            string name = FLChat.Core.InstallHelper.GetServiceNameAppConfig(typeof(Installer), "ProjectName") ?? "";
            
            //string name = ConfigurationManager.AppSettings["ProjectName"] ?? "";
            serviceInstaller = new ServiceInstaller {
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true,
                ServiceName = name + "TGBotSender",
                DisplayName = name + " FLChat Telegram bot sender",
                Description = name + " Sending outcome messages for telegram bot"
            };
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
