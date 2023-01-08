using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using FLChat.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.Email.Sender.Tests
{
    [TestClass]
    public class EmailSenderServiceTest
    {

        [TestMethod]
        public void EmptyTest()
        {
            var source = new List<string> { "a", "b" , "c" , "d" , "e" , "f" , "g" , "h" };

            var result = SplitMessages(source, 4);

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("a,b", result[0]);
            Assert.AreEqual("c,d", result[1]);
            Assert.AreEqual("e,f", result[2]);
            Assert.AreEqual("g,h", result[3]);
        }

        private List<string> SplitMessages(List<string> source, int count)
        {
            var size = source.Count / count;
            var result = source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(x => string.Join(",", x.Select(v => v.Value)))
                .ToList();

            return result;
        }

        //[TestMethod]
        //public void TestMethod1()
        //{

        //    var _cts = new CancellationTokenSource();

        //    {
        //        if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out int delay))
        //            throw new ConfigurationErrorsException("Configuration value delay_ms is invalid");

        //            ISendingConveyor conv = new SendingConveyor(
        //                CreateClient(),
        //                DAL.TransportKind.Email,
        //                idSaver: null);
        //            conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => { });
        //        //    log.Info("Sending messages was stopped");
        //            _cts.Dispose();

        //    }
        //}

        private FLChat.Transport.MailSender CreateClient()
        {
            FLChat.Transport.MailSender client = new FLChat.Transport.MailSender();
            return client;
        }

    }
}
