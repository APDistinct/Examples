using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.ViberSmsDevino.Sender;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Transport.Tests
{
    [TestClass]
    public class DevinoViberSendTests
    {
        [TestMethod]
        public void SendToNullAddressee()
        {
            DevinoViberSender dvs = new DevinoViberSender();
            MessageToUser msg = new MessageToUser()
            {
                ToTransport = new DAL.Model.Transport()
                {
                    Kind = TransportKind.WebChat,
                    User = new User()
                    {
                        Id = Guid.NewGuid(),
                        Phone = null,
                    }
                }
            };

            Assert.ThrowsException<ArgumentNullException>(() => dvs.Send(msg, "", CancellationToken.None));            
        }

        [TestMethod]
        public void TestMethod1()
        {
            DevinoViberSender dvs = new DevinoViberSender();
            //string pattern = "fl.im/mid";
            //string str1 = "dfdf s";
            //string str2 = "dfdfdfffdf";
            string pattern = "fl.im/mid";
            string str1 = "Ваш наставник test_token_user отправил вам сообщение. ";
            //Ваш наставник test_token_user отправил вам сообщение. fl.im/midPRXEzT0FDG5KFezS9cv9

            string str2 = "PRXEzT0FDG5KFezS9cv9";
            string str = str1 + pattern + str2 + "  dfdf ";
            var ret = dvs.FindPattern(str, pattern);
            Assert.AreEqual(pattern + str2, ret);

        }

        [TestMethod]
        public void TestMethod2()
        {
            DevinoViberSender dvs = new DevinoViberSender();
            string pattern = "fl.im/mid";
            string str1 = "Ваш наставник test_token_user отправил вам сообщение. ";
            //Ваш наставник test_token_user отправил вам сообщение. fl.im/midPRXEzT0FDG5KFezS9cv9

            string str2 = "PRXEzT0FDG5KFezS9cv9";
            string str = str1 + pattern + str2; // + "  dfdf ";
            var ret = dvs.FindPattern(str, pattern);
            Assert.AreEqual(pattern + str2, ret);
        }

        [TestMethod]
        public void TestMethod3()
        {
            string pattern = "fl.im/mid";
            string _pattern = pattern + "%code%";
            int pos = _pattern.IndexOf('%');
            if (pos < 0)
                pos = _pattern.Length - 1;
            _pattern = _pattern.Substring(0, pos);
            pos = _pattern.Length - 1;
            if (_pattern[pos] == '/')
                _pattern = _pattern.Substring(0, pos);
            Assert.IsTrue(pattern.Contains( _pattern));
        }

        //[TestMethod]
        //public void TestMethodAll()
        //{
        //    var _cts = new CancellationTokenSource();
        //    //try
        //    {
        //        //int delay = 1000;
        //        //log.Info("delay_ms");                
        //        //Task.Run(() =>
        //        {
        //            ISendingConveyor conv = new SendingConveyor(
        //                CreateClient(),
        //                DAL.TransportKind.Sms,
        //                idSaver: new TransportIdSaver(),
        //                msgCompiler: MakeTextCompiller());
        //            //log.Info("GetConveyor");
        //            conv.Send(_cts.Token);
        //            //conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => { } /*log.Error("Exception in conveyor: ", e)*/);
        //            //log.Info("Sending messages was stopped");
        //            _cts.Dispose();
        //        };
        //    }
        //}

        private IMessageSender CreateClient()
        {
            // Формирование ссылки по СМС-сообщению - на страницу выбора транспорта
            //string _pattern = Settings.Values.GetValue("WEB_CHAT_DEEP_URL", "https://chat.faberlic.com/external/%code%");
            //int pos = _pattern.IndexOf('%');
            //if (pos < 0)
            //    pos = _pattern.Length - 1;
            //_pattern = _pattern.Substring(0, pos);
            //pos = _pattern.Length - 1;
            //if(_pattern[pos] == '/')
            //    _pattern = _pattern.Substring(0, pos);

            // Формирование ссылки по WebChatDeepLink с переадрессацией в Viber-bot
            string _pattern = GetPatternLink(TransportKind.Viber);

            CombiSender client = new CombiSender(
                new DevinoViberSender(_pattern, ChangeParamTextCompiler.GetForwardMsg),
                new DevinoSmsSender(),
                ViberSmsChecker.Check);
            return client;
        }

        private IMessageTextCompiler MakeTextCompiller()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
               = new Dictionary<string, Func<MessageToUser, string>>()
               {
                    { "sendername", mtu => mtu.Message.FromTransport?.User?.FullName ?? "" },
               };
            //new TagReplaceTextCompiler(ReplaceDict)
            //    userFrom = mtu.FromTransport.User
            string _pattern = Settings.Values.GetValue("WEB_CHAT_VIBER",
                    "#sendername, ваш личный консультант Faberlic, отправил вам сообщение:");
            CombiCompiler compiler = new CombiCompiler(
               new DevinoViberSmsCompiler(_pattern)
               .Add(new ChangeParamTextCompiler(
                   new TagReplaceTextCompiler(ReplaceDict, true).AddStandartHashCompiler(),
                   ChangeParamTextCompiler.GetForwardMsg //(u => u.Message.ForwardMsg.ToUser)
                   )),
               new SimpleMsgTextCompiler(),
               ViberSmsChecker.Check
               );

            return compiler;
        }
        private string GetPatternLink(TransportKind kind)
        {
            string ret = null;
            using (ChatEntities entities = new ChatEntities())
            {
                ret = entities.TransportType.Where(t => t.Id == (int)kind).Select(x => x.DeepLink).FirstOrDefault();
            }
            return ret;
        }

    }
}
