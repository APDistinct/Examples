using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using FLChat.Devino;
using FLChat.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Transport.Tests
{
    [TestClass]
    public class SmsSendTest
    {
        //int delay;

        [TestMethod]
        public void SmsSendOneTest()
        {
            //string phone = "79145671450", text = "Первый блин";
            //var smsSender = new SmsSender();
            //smsSender.Send(phone, text);

        }

        //[TestMethod]
        //public void DevinoProviderTest()
        //{
        //    var sms = new DevinoProvider();
        //    Assert.IsNotNull(sms);
        //}

        //[TestMethod]
        //public void SmsSendBulkTest()
        //{
        //    var _cts = new CancellationTokenSource();
        //    try
        //    {
        //        if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out delay))
        //            throw new ConfigurationErrorsException("Configuration value delay_ms is invalid");
        //        if (!int.TryParse(ConfigurationManager.AppSettings["sms_sender_kind"] ?? "0", out int kind))
        //            throw new ConfigurationErrorsException("Configuration value sms_sender_kind is invalid");
        //        //Task.Run(() => 
        //        {
        //            ISendingConveyor conv = GetConveyor(kind);
        //            //new SendingConveyor(
        //            //    CreateClient(),
        //            //    DAL.TransportKind.Sms,
        //            //    idSaver: null);
        //            conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => { }/*log.Error("Exception in conveyor: ", e)*/);
        //            //log.Info("Sending messages was stopped");
        //            _cts.Dispose();
        //        }
        //        //);
        //    }
        //    catch (AggregateException e)
        //    {
        //        //log.Fatal("Service exception:", e);
        //    }
        //}

        private FLChat.Transport.SmsSender CreateClient()
        {
            FLChat.Transport.SmsSender client = new FLChat.Transport.SmsSender();
            return client;
        }

        private FLChat.Transport.SmsSenderBulk CreateBulkClient()
        {
            FLChat.Transport.SmsSenderBulk client = new FLChat.Transport.SmsSenderBulk();
            return client;
        }

        private ISendingConveyor GetConveyor(int kind)
        {
            ISendingConveyor conv = null;
            switch (kind)
            {
                case 1:
                    conv = new SendingConveyor(
                        CreateClient(),
                        DAL.TransportKind.Sms,
                        idSaver: null);
                    //conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    break;
                case 0:
                default:
                    conv = new SendingConveyorBulk(
                        CreateBulkClient(),
                        DAL.TransportKind.Sms,
                        idSaver: null);
                    //conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    break;
            }
            return conv;
        }
    }
}
