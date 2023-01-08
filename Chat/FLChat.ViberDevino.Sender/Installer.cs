using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FLChat.ViberDevino.Sender
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public Installer()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            string name = Core.InstallHelper.GetServiceNameAppConfig(typeof(Installer), "ProjectName") ?? "";
            serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true,
                ServiceName = name + "ViberDevinoSender",
                DisplayName = name + " FLChat Viber(Sms) messages sender via Devino",
                Description = name + " Sending outcome Viber(sms) messages via Devino"
            };
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
