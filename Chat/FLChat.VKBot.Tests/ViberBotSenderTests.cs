using System;
using System.Threading;
using FLChat.Core;
using FLChat.Viber.Bot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class ViberBotSenderTests
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    ISendingConveyor conv = new SendingConveyor(
        //                //new SendingConveyor(
        //                CreateClient(),
        //                DAL.TransportKind.Viber,
        //                idSaver: new TransportIdSaver(),
        //                //msgCompiler: new SimpleMsgTextCompiler().UniteWithHashCompiler());
        //                msgCompiler: ViberFactory.CreateCompiler());
        //    var _cts = new CancellationTokenSource();
        //    conv.Send(_cts.Token);
        //}

        //private ViberSender CreateClient()
        //{
        //    string token = "4aa4a0c7e6e7d33e - dce713a8b1c0962c - 5394caf897487b0a";
        //        //ConfigurationManager.AppSettings["token"] ?? throw new ConfigurationErrorsException("Configuration value for telegram token must be present");
        //    ViberSender client = new ViberSender(token);
        //    //client.Log.OnLogError += (s, e) => {
        //    //    log.Error("Write transport log error: ", e);            };
        //    return client;
        //}
    }
}
