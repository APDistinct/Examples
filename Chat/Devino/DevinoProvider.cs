using FLChat.Devino.DvTypes;
using FLChat.Devino.SMS;
using FLChat.Devino.EMAIL;
using RestApiClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Devino;
using Devino.Args;
using Devino.Logger;
using Newtonsoft.Json;
using Devino.Viber;
using Devino.Exceptions;

namespace FLChat.Devino
{
    public class DevinoProvider : IDevinoProvider
    {
        string Login { set; get; }                              // Логин для доступа
        string Password { set; get; }                           // Пароль для доступа
        string Sender { set; get; }                             // Имя отправителя
        string SmsSender { set; get; }                          // Имя отправителя для SMS
        string EmailSender { set; get; }                        // Имя отправителя для Email
        string Email { set; get; }                              // Email отправителя
        string Subject { set; get; }                            // Тема Email отправителя
        private bool ViberResendSms { set; get; }               // Признак переотправки для SMS

        private readonly string Ok = "ok";

        private readonly IDevinoLogger _logger;

        public DevinoProvider(IDevinoLogger logger)
        {
            _logger = logger;
            Init();
        }
        public DevinoProvider(IDevinoLogger logger, DevinoSettings settings)
        {
            _logger = logger;
            Init(settings);
        }

        private void Init()
        {
            Login = ConfigurationManager.AppSettings["DevinoLogin"];
            Password = ConfigurationManager.AppSettings["DevinoPassword"];
            Sender = ConfigurationManager.AppSettings["DevinoSender"];
            Email = ConfigurationManager.AppSettings["DevinoSenderEmail"];
            EmailSender = ConfigurationManager.AppSettings["DevinoSenderEmailName"];
            Subject = ConfigurationManager.AppSettings["DevinoEmailSubject"];
            if (string.IsNullOrWhiteSpace(Subject))
            {
                Subject = "Соощение от " + Sender;
            }
            string sss = ConfigurationManager.AppSettings["ViberResendSms"] ?? "0";
            ViberResendSms = int.TryParse(sss, out int ret) ? ret > 0 : false;
        }

        private void Init(DevinoSettings settings)
        {
            Login = settings.Login;
            Password = settings.Password;
            Sender = settings.Sender;
            SmsSender = settings.SmsSender;
            Email = settings.Email;
            EmailSender = settings.EmailSender;
            Subject = settings.Subject;
            ViberResendSms = settings.ViberResendSms;
            //if (string.IsNullOrWhiteSpace(settings.Subject))
            //{
            //    Subject = $"Соощение от {settings.Sender}";
            //}

        }


        //public string SmsSend(List<string> destinationAddresses, string data, DateTime? sendDate = null, int validity = 0)
        //{
        //    var sms = new Sms();

        //    //try
        //    {
        //        string sessionId = sms.GetSessionId(Login, Password);
        //        decimal balance = sms.GetBalance(sessionId);
        //        List<string> bulkIds = sms.SendMessagesBulk(sessionId, Sender, destinationAddresses, data, sendDate, validity);
        //        return bulkIds[0];
        //    }
        //    //catch
        //    {

        //    }
        //    //return null;
        //}

        public async Task<SentAllMessageInfo> SmsSend(string destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0)
        {
            List<string> list = new List<string>() { destinationAddresses};

            var retlist = await SmsSend(list, data, subject , sendDate , validity );
            var ret = retlist.FirstOrDefault();
            return ret;
        }

        public async Task<IEnumerable<SentAllMessageInfo>> SmsSend(List<string> destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0)
        {
            var service = new SmsService(_logger);

            //var sessionId = (await service.GetSessionId(Login, Password))?.SessionId;
            //var ret = await service.SmsSend(destinationAddresses, data, Sender, sessionId);
            var timesent = DateTime.UtcNow;
            var ret = await service.SmsSend(destinationAddresses, data, SmsSender, Login, Password);
            var list = ret.Select(x => new SentAllMessageInfo(x, timesent));
            //var list = ret.Result.Select(x => new SentMessageInfo(x, DateTime.UtcNow));
            return list;
        }

        public Task<IEnumerable<SentAllMessageInfo>> EmailSend(List<string> destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0)
        {
            DvEmailRequest request = new DvEmailRequest()
            {
                Sender = new Sender() { Address = Email, Name = EmailSender },
                Recipients = destinationAddresses.Select(x => new Recipient() { Address = x }),
                //Body = new Body() { Html/* PlainText */ = data + " [Unsubscribe]" },
                Body = new Body() { Html = /*"< a href =\"[Unsubscribe]\">Отписаться</a> " + */data, PlainText = " [Unsubscribe]" },
                Subject = !string.IsNullOrEmpty(subject) ? subject : Subject,
            };
            var ret = EmailSend(request);
            return ret;

        }

        public Task<SentAllMessageInfo> EmailSend(string destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0)
        {
            var list = new List<string>() { destinationAddresses };
            var ret = EmailSendAsync(destinationAddresses, data, subject, sendDate, validity);

            return ret;
        }

        public async Task<SentAllMessageInfo> EmailSendAsync(string destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0)
        {
            var list = new List<string>() { destinationAddresses };
            var retlist = await EmailSend(list, data, subject, sendDate, validity);
            var ret = retlist.FirstOrDefault();

            return ret;
        }

        //public DvEmailResponse EmailSend(DvEmailRequest dvEmailRequest)
        //{
        //    var emailService = new EmailService(Login, Password);

        //    try
        //    {
        //        var result = emailService.SendEmail(dvEmailRequest, new CancellationToken(false));
        //        //var json = JsonConvert.SerializeObject(dvEmailRequest);
        //        //string sessionId = email.GetSessionId(login, password);
        //        //decimal balance = email.GetBalance(sessionId);
        //        //List<string> bulkIds = email.SendMessagesBulk(sessionId, sender, destinationAddresses, data, sendDate, validity);
        //        //return bulkIds[0];
        //    }
        //    catch
        //    { }
        //    return null;
        //}

        public async Task<IEnumerable<SentAllMessageInfo>> EmailSend(DvEmailRequest dvEmailRequest)
        {
            var emailService = new EmailService(Login, Password, _logger);
            var timesent = DateTime.UtcNow;
            var result = await emailService.SendEmail(dvEmailRequest, new CancellationToken(false));
            var ret = result.Result.Select(x => new SentAllMessageInfo(new List<string>() { x.MessageId }, timesent));
            return ret;
        }

        public async Task<SentAllMessageInfo> ViberSend(ViberMessageInfo vmInfo)
        {
            List<ViberMessageInfo> list = new List<ViberMessageInfo>() { vmInfo };

            var timesent = DateTime.UtcNow;
            var retlist = await ViberSend(list);
            var ret = retlist.FirstOrDefault();
            if(ret.Code != Ok)
            {
                throw new BadResponseException($"Devino error: {ret.Code}");
            }
            
            return new SentAllMessageInfo(new List<string>() { ret.ProviderId }, timesent);
        }

        public Task<IEnumerable<MessageResponse>> ViberSend(List<ViberMessageInfo> vmInfoList)
        {
            ViberSendMessage request = new ViberSendMessage()
            {
                ResendSms = ViberResendSms,
                //Sender = new Sender() { Address = Email, Name = Sender },
                //Recipients = destinationAddresses.Select(x => new Recipient() { Address = x }),
                ////Body = new Body() { Html/* PlainText */ = data + " [Unsubscribe]" },
                //Body = new Body() { Html = /*"< a href =\"[Unsubscribe]\">Отписаться</a> " + */data, PlainText = " [Unsubscribe]" },
                //Subject = !string.IsNullOrEmpty(subject) ? subject : Subject,
            };
            List<ViberMessage> list = new List<ViberMessage>();
            foreach (var vmInfo in vmInfoList)
            {
                var vm = new ViberMessage()
                {
                    Subject = Sender, // Подпись отправителя - из конфигурации
                    Priority = "high",		// Приоритет сообщения ? возможно из конфигурации
                    ValidityPeriodSec = 3600,   // Время ожидания доставки Viber сообщения в секундах - из конфигурации
                    Comment = "", //? пока не будет
                    Type = "viber",     // Без вариантов
                    ContentType = "button", // именно так
                    Content = new MessageContent()
                    {
                        Text = vmInfo.Text,  // Текст сообщения - предварительно обработан
                        Caption = vmInfo.Caption,  // Подпись на кнопке
                        Action = vmInfo.Action,  // ссылка для перехода                        
                    },
                    Address  = vmInfo.Address,
                    SmsText = vmInfo.SmsText,
                    SmsSrcAddress = SmsSender,  // Отправитель СМС - из конфигурации
                    SmsValidityPeriodSec = 5000, // Время ожидания доставки СМС сообщения в секундах - из конфигурации
                };
                if (!string.IsNullOrEmpty(vmInfo.ImageUrl))
                    vm.Content.ImageUrl = vmInfo.ImageUrl;  // Адрес картинки
                list.Add(vm);
                //request.
            }
            request.Messages = list;
            var ret = ViberSend(request);
            return ret;
        }

        public async Task<IEnumerable<MessageResponse>> ViberSend(ViberSendMessage viberSendMessage)
        {
            var viberService = new ViberService(Login, Password, _logger);
            var result = await viberService.Send(viberSendMessage, new CancellationToken(false));
            if (result.Status != Ok)
            {                
                throw new BadResponseException($"Devino error: {result.Status }");
            }
            var ret = result.Messages; //.Select(x => new SentAllMessageInfo(new List<string>() { x.ProviderId }, DateTime.UtcNow));
            return ret;
        }

        public async Task<GetStatusResponse> ViberGetStatus(GetStatusMessage message, CancellationToken ct )
        {
            var viberService = new ViberService(Login, Password, _logger);
            var result = await viberService.GetStatus(message, ct);
            return result;
        }

        public async Task<GetStatusResponse> ViberGetStatus(GetStatusMessage message)
        {            
            return await ViberGetStatus(message, CancellationToken.None);
        }
    }
}
