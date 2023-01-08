using FLChat.Devino;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devino.Logger;
using FLChat.Devino.SMS;
using FLChat.Devino.DvTypes;
using FLChat.Devino.EMAIL;
using FLChat.Core;
using Devino.Viber;
using FLChat.DAL;
using Devino;

namespace DevinoTest
{
    public partial class Form1 : Form
    {
        DevinoProvider provider = new DevinoProvider(DevinoLogger.GetLogger(TransportKind.Email));
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonSMS_ClickAsync(object sender, EventArgs e)
        {
            var vs = textBoxSMS.Lines;
            string str = textBoxMess.Text;
            if (vs.Count() > 0 && str.Any())
                await provider.SmsSend(vs.ToList(), str);
            else
            {
                MessageBox.Show("Недопустимые параметры");
            }
        }

        private void buttonTestEmail_Click(object sender, EventArgs e)
        {
            EmailSmtpClient client = new EmailSmtpClient();
            client.Send();
        }

        private const string UserLogin = "Faberlic_test1";
        private const string Password = "D342ksoobxY!12";
        private const string Email = "faberlicchat@faberlic.com";
        private const string Sender = "Faberlic";
        private const string SmsSender = "FaberliChat";
        private const string EmailSender = "Faberlic Chat";

        private IDevinoLogger GetLogger(TransportKind transportKind)
        {
            var writer = new DevinoLogWriter(true, transportKind);
            var log = new DevinoLogger();
            log.MakingApiRequest += writer.Request;
            log.ApiResponseReceived += writer.Response;
            log.ApiRequestException += writer.Exception;

            return log;
        }

        private async void ButtonEmail_Click(object sender, EventArgs e)
        {
            var vs = textBox2.Lines;
            string str = textBoxMess.Text;
            IEnumerable<SentAllMessageInfo> smiList = null;
            if (vs.Count() > 0 && str.Any())
                smiList = await provider.EmailSend(vs.ToList(), str + " [Unsubscribe]");
            else
            {
                MessageBox.Show("Недопустимые параметры");
            }

            //var service = new DevinoEmailService.EmailService(UserLogin, Password);
            //var result = await service.SourceAddresses();
            //label3.Text = result;

        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            var service = new EmailService(UserLogin, Password, GetLogger(TransportKind.Email));

            var destinationAddresses = new List<string> {"dvikdvik@gmail.com", "apdapdapdapdapd@gmail.com" };
            var dvEmail = new DvEmailRequest()
            {
                Sender = new Sender() { Address = "faberlicchat@faberlic.com", Name = EmailSender },
                Recipients = destinationAddresses.Select(x => new Recipient() { Address = x }),
                Body = new Body() { Html/* PlainText*/ = "Ждем вас завтра! [Unsubscribe]" },
                Subject = "Привет",
            };
            var result = await service.SendEmail(dvEmail, new CancellationToken());

            label3.Text = result.Description;
        }

        private async void Button3_Click(object sender, EventArgs e)
        {
            var service = new SmsService(GetLogger(TransportKind.Sms)); 
            //var sessionId = (await service.GetSessionId(UserLogin, Password))?.SessionId;
                        
            var destinationAddresses = new List<string>(){"89146198203", };
            //string basenum = "791412";
            //for (int i = 0; i < 1001; ++i)
            //{
            //    destinationAddresses.Add(basenum + (1000 + i).ToString());
            //}
            //string str = "---Test---";
            string str = "--- Очень длинный тест русскими буквами для проверки отправки и ответа --- --- Очень длинный тест русскими буквами для проверки отправки и ответа ---";

            var ret =  await service.SmsSend(destinationAddresses, str, Sender, UserLogin, Password);
            //var ret =  await service.SmsSend(destinationAddresses, str, "DTSMS", sessionId);

            label3.Text = string.Join(" ", string.Join(",", ret));
        }

        private async void buttonViber_ClickAsync(object sender, EventArgs e)
        {
            var vs = textBoxSMS.Lines;
            string text = textBoxMess.Text;
            string textV = textBoxViber.Text;
            if (!(vs.Count() > 0 && text.Any() && textV.Any()))            
            {
                MessageBox.Show("Недопустимые параметры");
                return;
            }

            var service = new ViberService(UserLogin, Password, GetLogger(TransportKind.Sms));
            ViberSendMessage request = new ViberSendMessage()
            {
                ResendSms = true,
            };
            List<ViberMessage> list = new List<ViberMessage>();
            foreach (var mess in textBoxSMS.Lines)
            {
                var vm = new ViberMessage()
                {
                    //  Вопрос о разных именах для СМС и вайбера ?
                    Subject = Sender, // Подпись отправителя - из конфигурации
                    Priority = "high",      // Приоритет сообщения ? возможно из конфигурации
                    ValidityPeriodSec = 3600,   // Время ожидания доставки Viber сообщения в секундах - из конфигурации
                    Comment = "", //? пока не будет
                    Type = "viber",     // Без вариантов
                    ContentType = "button", // именно так
                    Content = new MessageContent()
                    {
                        Text = textV,  // Текст сообщения - предварительно обработан
                        Caption = textBoxButton.Text,  // Подпись на кнопке
                        Action = textBoxAction.Text,  // ссылка для перехода
                        ImageUrl = textBoxImage.Text,  // Адрес картинки
                    },
                    Address = mess,
                    SmsText = text,
                    SmsSrcAddress = SmsSender,  // Отправитель СМС - из конфигурации
                    SmsValidityPeriodSec = 5000, // Время ожидания доставки СМС сообщения в секундах - из конфигурации
                };
                list.Add(vm);
            }
            request.Messages = list;
            var result = await service.Send(request, new CancellationToken());

            label3.Text = result.Status;
            if(result.Messages != null && result.Messages.Count() > 0)
            {
                textBoxGetStatus.Text = "";
                foreach (var mess in result.Messages)
                {
                    textBoxGetStatus.Text = textBoxGetStatus.Text + Environment.NewLine + mess.ProviderId.ToString();
                }
            }
        }

        private async void buttonGetStatus_Click(object sender, EventArgs e)
        {
            var message = new GetStatusMessage {Messages = textBoxGetStatus.Lines.ToList()};

            var result = await provider.ViberGetStatus(message);

        }
    }
}
