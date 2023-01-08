using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devino
{
    public class DevinoSettings
    {
        /// <summary>
        /// Логин для доступа
        /// </summary>
        public string Login { set; get; }

        /// <summary>
        /// Пароль для доступа
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// Имя отправителя
        /// </summary>
        public string Sender { set; get; }

        /// <summary>
        /// Имя отправителя для Email
        /// </summary>
        public string EmailSender { set; get; }

        /// <summary>
        /// Имя отправителя для Sms
        /// </summary>
        public string SmsSender { set; get; }

        /// <summary>
        /// Email отправителя
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// Тема Email отправителя
        /// </summary>
        public string Subject { set; get; }

        /// <summary>
        /// Признак переотправки для SMS
        /// </summary>
        public bool ViberResendSms { set; get; }
    }
}