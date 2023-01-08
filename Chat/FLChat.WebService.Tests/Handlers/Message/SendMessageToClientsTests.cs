using System;
using System.Linq;
using System.Threading;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.TelegramBot;
using FLChat.Viber.Bot;
using FLChat.VKBot;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.Message;
using FLChat.WebService.Handlers.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Tests.Handlers.Message
{
    [TestClass]
    public class SendMessageToClientsTests
    {
        ChatEntities entities;
        CreateUser handler;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new CreateUser();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        [Ignore("Игнорируем, поскольку тест содержит личные данные и производит отправку сообщения конкретному получателю. Должно выполняться вручную.")]
        public void SendMessageToViberTest()
        {
            DAL.Model.User sender = entities.GetUser(u => u.Enabled, null);
            var senderTra = sender.Transports.Get(TransportKind.FLChat);

            if (senderTra == null)
            {
                senderTra = new DAL.Model.Transport();
                sender.Transports.Add(senderTra);
                senderTra.TransportTypeId = (int)TransportKind.FLChat;
                senderTra.Enabled = true;
                entities.SaveChanges();
            }


            DAL.Model.User recipient = entities.User.FirstOrDefault(f => f.FullName == "Andrew Andrew");
            if (recipient == null)
            {
                JObject input = new JObject() {
                    ["phone"] = "380989913825",
                    ["full_name"] = "Andrew Andrew"
                };

                UserProfileInfo response = handler.ProcessRequest(entities, sender, input);
                recipient = entities.User.Where(u => u.Id == response.Id).FirstOrDefault();
            }

            DAL.Model.Transport t = recipient.Transports.Get(TransportKind.Viber);
            if (t == null) {
                t = new DAL.Model.Transport();
                recipient.Transports.Add(t);
                t.TransportTypeId = (int)TransportKind.Viber;
                t.Enabled = true;
                t.TransportOuterId = "zp0XHPA5tuXJIbxOWrQfuw==";
                entities.SaveChanges();

            }

            var sendMessageRequest = new SendMessageRequest() {Text = "Привет из вайбера", ToTransport = TransportKind.Viber };
            sendMessageRequest.ToUser = recipient.Id;
            new SendMessage().ProcessRequest(entities, sender, sendMessageRequest);


            var cnv = new SendingConveyor(
                //new SendingConveyor(
                CreateViberClient(),
                DAL.TransportKind.Viber,
                idSaver: new TransportIdSaver(),
                //msgCompiler: new SimpleMsgTextCompiler().UniteWithHashCompiler());
                msgCompiler: ViberFactory.CreateCompiler());

            cnv.Send(CancellationToken.None);
        }

        private ViberSender CreateViberClient() {
            string token = "4aa4a0c7e6e7d33e-dce713a8b1c0962c-5394caf897487b0a";
            ViberSender client = new ViberSender(token);
            client.Log.OnLogError += (s, e) => {
                Console.Write("Write transport log error: ", e);
            };
            return client;
        }
        
        [TestMethod]
        [Ignore("Игнорируем, поскольку тест содержит личные данные и производит отправку сообщения конкретному получателю. Должно выполняться вручную.")]
        public void SendMessageToTelegrammTest()
        {
            DAL.Model.User sender = entities.GetUser(u => u.Enabled, null);
            var senderTra = sender.Transports.Get(TransportKind.FLChat);

            if (senderTra == null)
            {
                senderTra = new DAL.Model.Transport();
                sender.Transports.Add(senderTra);
                senderTra.TransportTypeId = (int)TransportKind.FLChat;
                senderTra.Enabled = true;
                entities.SaveChanges();
            }


            DAL.Model.User recipient = entities.User.FirstOrDefault(f => f.FullName == "Andrew Andrew");
            if (recipient == null)
            {
                JObject input = new JObject() {
                    ["phone"] = "380989913825",
                    ["full_name"] = "Andrew Andrew"
                };

                UserProfileInfo response = handler.ProcessRequest(entities, sender, input);
                recipient = entities.User.Where(u => u.Id == response.Id).FirstOrDefault();
            }

            DAL.Model.Transport t = recipient.Transports.Get(TransportKind.Telegram);
            if (t == null) {
                t = new DAL.Model.Transport();
                recipient.Transports.Add(t);
                t.TransportTypeId = (int)TransportKind.Telegram;
                t.Enabled = true;
                t.TransportOuterId = "890198357";
                entities.SaveChanges();

            }

            var sendMessageRequest = new SendMessageRequest() {Text = "Привет из телеграма", ToTransport = TransportKind.Telegram };
            sendMessageRequest.ToUser = recipient.Id;
            new SendMessage().ProcessRequest(entities, sender, sendMessageRequest);


            var cnv = new SendingConveyor(
                //new SendingConveyor(
                CreateTelegramClient(),
                DAL.TransportKind.Telegram,
                idSaver: new TransportIdSaver(),
                //msgCompiler: new SimpleMsgTextCompiler().UniteWithHashCompiler());
                msgCompiler: new AuthorMsgTextCompiler().UniteWithHashCompiler());

            cnv.Send(CancellationToken.None);
        }

        private TelegramClient CreateTelegramClient()
        {
            string token = "986521594:AAH47EhhJT1yB013VJ_Lz3_s07cMhxPjM6M";
            string proxy = "185.17.120.252";
            var port = 1080;
            string usr = "tguser";
            string psw = "d72c76bcf19db9aa458a9862cdca9f3b";

            TelegramClient client = new TelegramClient(token, proxy, port, usr, psw);
            client.Log.OnLogError += (s, e) => {
                Console.Write("Write transport log error: ", e);
            };
            return client;
        }

        [TestMethod]
        [Ignore("Игнорируем, поскольку тест содержит личные данные и производит отправку сообщения конкретному получателю. Должно выполняться вручную.")]
        public void SendMessageToVKTest()
        {
            DAL.Model.User sender = entities.GetUser(u => u.Enabled, null);
            var senderTra = sender.Transports.Get(TransportKind.FLChat);

            if (senderTra == null)
            {
                senderTra = new DAL.Model.Transport();
                sender.Transports.Add(senderTra);
                senderTra.TransportTypeId = (int)TransportKind.FLChat;
                senderTra.Enabled = true;
                entities.SaveChanges();
            }


            DAL.Model.User recipient = entities.User.FirstOrDefault(f => f.FullName == "Andrew Andrew");
            if (recipient == null)
            {
                JObject input = new JObject() {
                    ["phone"] = "380989913825",
                    ["full_name"] = "Andrew Andrew"
                };

                UserProfileInfo response = handler.ProcessRequest(entities, sender, input);
                recipient = entities.User.Where(u => u.Id == response.Id).FirstOrDefault();
            }

            DAL.Model.Transport t = recipient.Transports.Get(TransportKind.VK);
            if (t == null) {
                t = new DAL.Model.Transport();
                recipient.Transports.Add(t);
                t.TransportTypeId = (int)TransportKind.VK;
                t.Enabled = true;
                t.TransportOuterId = "589103673";
                entities.SaveChanges();

            }

            var sendMessageRequest = new SendMessageRequest() {Text = "Привет из vk", ToTransport = TransportKind.VK };
            sendMessageRequest.ToUser = recipient.Id;
            new SendMessage().ProcessRequest(entities, sender, sendMessageRequest);


            var cnv = new SendingConveyor(
                //new SendingConveyor(
                CreateVKClient(),
                DAL.TransportKind.VK,
                idSaver: new TransportIdSaver(),
                //msgCompiler: new SimpleMsgTextCompiler().UniteWithHashCompiler());
                msgCompiler: MakeTextCompiller());

            cnv.Send(CancellationToken.None);
        }

        private VKClient CreateVKClient()
        {
            string token = "0be1ca4d1f355216067f88a68a3f57a5a8267d3b2b336e5fea1cf6235880203f118c9e346dbf173f07271";
            
            VKClient client = new VKClient(token);
            client.Log.OnLogError += (s, e) => {
                Console.Write("Write transport log error: ", e);
            };
            return client;
        }

        private IMessageTextCompiler MakeTextCompiller()
        {
            ////  Шаблон имени файла
            //string filen = Settings.Values.GetValue("MAINSERVER_NAME", "https://rvprj.ru:8443/FLChat/") +
            //Settings.Values.GetValue("COMMAND_GETFILE", "file/%id%");
            ////  Шаблон ссылки файла
            //string templ = Settings.Values.GetValue("VK_FILELINK_TEMPLATE", "https://vk.com/away.php?to=#FileLink&cc_key=#FileText");
            
            IMessageTextCompiler compiller = new AuthorMsgSimpleTextCompiler().UniteWithHashCompiler()/*.Add(new FileMsgTextCompiler(templ, filen))*/;
            return compiller;
        }

    }
}
