using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devino
{
    public class DevinoSettingsReader : DevinoSettings
    {
        public DevinoSettingsReader()
        {
            //Init();
        }
        
        public void BaseInit()
        {
            Login = ConfigurationManager.AppSettings["DevinoLogin"] ?? throw new ConfigurationErrorsException("Configuration value DevinoLogin is invalid");
            Password = ConfigurationManager.AppSettings["DevinoPassword"] ?? throw new ConfigurationErrorsException("Configuration value DevinoPassword is invalid");
            Sender = ConfigurationManager.AppSettings["DevinoSender"] ?? throw new ConfigurationErrorsException("Configuration value DevinoSender is invalid");
            SmsSender = ConfigurationManager.AppSettings["DevinoSenderSmsName"]/* ?? throw new ConfigurationErrorsException("Configuration value DevinoSender is invalid")*/;
            string sss = ConfigurationManager.AppSettings["ViberResendSms"] ?? "0" ;
            ViberResendSms = int.TryParse(sss, out int ret) ? ret > 0 : false;

            if (string.IsNullOrWhiteSpace(SmsSender))
            {
                SmsSender = Sender;
            }
        }

        public void EmailInit()
        {
            BaseInit();
            Email = ConfigurationManager.AppSettings["DevinoSenderEmail"] ?? throw new ConfigurationErrorsException("Configuration value DevinoSenderEmail is invalid");
            EmailSender = ConfigurationManager.AppSettings["DevinoSenderEmailName"] ?? Sender;
            Subject = ConfigurationManager.AppSettings["DevinoEmailSubject"];
            if (string.IsNullOrWhiteSpace(Subject))
            {
                Subject = $"Соощение от {Sender}";
            }
        }
    }
}
