using System;
using System.Threading;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class SendMessageTest
    {
        //[TestMethod]
        //public void SendingConveyorTest()
        //{

        //    int delay = 1000;
        //    CancellationTokenSource _cts = new CancellationTokenSource();
        //    //try
        //    {
        //        //Task.Run(() =>
        //        {
        //            ISendingConveyor conv = new SendingConveyor(
        //                CreateClient(),
        //                DAL.TransportKind.VK,
        //                idSaver: new TransportIdSaver(),
        //                msgCompiler: new AuthorMsgSimpleTextCompiler()//.UniteWithHashCompiler()*/ MakeTextCompiller()
        //                );
        //            conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => { }/*log.Error("Exception in conveyor: ", e)*/);
        //            //log.Info("VK - Sending messages was stopped");
        //            _cts.Dispose();
        //        }
        //        //);
        //    }
        //}

        //[TestMethod]
        //public void CommonSendMessageTest()
        //{
        //    string transportOuterId = "534672230";  //
        //    //var client = new VKClient(token);

        //    MessageToUser msg = new MessageToUser()
        //    {
        //        MsgId = Guid.NewGuid(),
        //        ToUserId = Guid.NewGuid(),
        //        ToTransportTypeId = (int)TransportKind.VK,
        //    };
        //    msg.ToTransport = new Transport()
        //    {
        //        UserId = Guid.NewGuid(),
        //        TransportTypeId = (int)TransportKind.VK,
        //        TransportOuterId = transportOuterId,
        //        Enabled = true,
        //    };
        //    msg.ToTransport.User = new DAL.Model.User() { IsTemporary = false, Id = new Guid(), };
        //    string msgText = "Test - Тест";
        //    CancellationToken ct = new CancellationToken();
        //    var ret = Send(msg, msgText, ct);
        //    var rr = ret.Result;
        //}


        //[TestMethod]
        //public async Task CommonUpdateMessageTest()
        //{
        //    //CancellationTokenSource cts = new CancellationTokenSource();
        //    //var ct = cts.Token;
        //    CancellationToken ct = new CancellationToken();
        //    var ret = Update(ct);
        //    var rr = await ret;
        //}

        //[TestMethod]
        //public async Task CommonUpdateMessageTest()
        //{

        //    //CancellationTokenSource cts = new CancellationTokenSource();
        //    //var ct = cts.Token;
        //    CancellationToken ct = new CancellationToken();
        //    var ret = Update(ct);
        //    var rr = await ret;
        //}


        private async Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct)
        {
            var client = CreateClient();
            var ret = await client.Send(msg, msgText, ct);
            return ret;
        }

        private async Task<Update[]> Update( CancellationToken ct)
        {
            string token = "49fbc63ef10dc50c92455fcf35537aa2f2722aeafb9d06d216aeb3bcd9c2e19b5546301bdfdd8529abcb3";  
            var client = new VKBotClient.VKBotClient(token);
            var ret = await client.MakeUpdatesAsync(179649792, ct);
            return ret;
        }

        private VKClient CreateClient()
        {
            string token = "0be1ca4d1f355216067f88a68a3f57a5a8267d3b2b336e5fea1cf6235880203f118c9e346dbf173f07271";
                //"49fbc63ef10dc50c92455fcf35537aa2f2722aeafb9d06d216aeb3bcd9c2e19b5546301bdfdd8529abcb3";  //
            return new VKClient(token);
        }
    }
}
