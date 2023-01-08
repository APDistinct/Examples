using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Client.Requests;
using FLChat.DAL.Model;
using FLChat.Core.Algorithms;
using FLChat.DAL;
using FLChat.Core.Tests;

namespace FLChat.Viber.Bot.Algorithms.Tests
{
    [TestClass]
    public class ConversationStartedStrategyTests
    {
        private class FakeGreetingMessages : Core.Texts.IGreetingMessagesText
        {
            public string LiteLinkKnownUser { get; set; } = "LiteLinkKnownUser";

            public string LiteLinkRouted { get; set; } = "LiteLinkRouted " + Core.Texts.IGreetingMessagesTextExtentions.AddresseeCnst;

            public string LiteLinkUnrouted { get; set; } = "LiteLinkUnrouted";

            public string LiteLinkUnknownUser { get; set; } = "LiteLinkUnknownUser";

            public string LinkRejectedOrUnknown { get; set; } = "LinkRejectedOrUnknown";
        }

        FakeGreetingMessages fakeGreetingMessages;
        ConversationStartedStrategy strategy;
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            fakeGreetingMessages = new FakeGreetingMessages();
            strategy = new ConversationStartedStrategy(fakeGreetingMessages);
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void ConversationStartedStrategy_Subscribed() {
            CallbackData data = Helper.ReadJson("callback_conversation_started_subscribed");
            Assert.IsNull(strategy.Process(entities, data, null));
        }

        [TestMethod]
        public void ConversationStartedStrategy_NotSubscribed() {
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, null);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_Accepted() {
            WebChatDeepLink wcdl = entities.WebChatDeepLink
                .Where(wc => wc.ExpireDate > DateTime.UtcNow 
                    && wc.MessageToUser.Message.FileInfo == null
                    && wc.MessageToUser.Message.Text != null).FirstOrDefault();
            if (wcdl == null) {
                entities.WebChatDeepLink.OrderByDescending(w => w.Id).FirstOrDefault();
                wcdl.ExpireDate = DateTime.UtcNow.AddDays(10);
                entities.SaveChanges();
            }
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new WebChatDeepLinkStrategy.Context(wcdl)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendTextMessageRequest));
            SendTextMessageRequest tr = (SendTextMessageRequest)resp;
            Assert.AreEqual(wcdl.MessageToUser.Message.Text, tr.Text);
            Assert.IsTrue(wcdl.MessageToUser.Message.FromTransport.User.FullName.StartsWith(tr.Sender.Name));
        }

        [TestMethod]
        public void ConversationStartedStrategy_LiteLink_Accepted() {
            DAL.Model.User user = entities.GetUserQ(where: q => q.Where(u => u.FullName != null),
                transport: TransportKind.Test);
            DAL.Model.User userRoute = entities.GetUserQ(where: q => q.Where(u => u.FullName != null),
                transport: TransportKind.FLChat, notSameToUser: user.Id);

            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new LiteDeepLinkStrategy.Context(
                    LiteDeepLinkStrategy.Context.LinkType.LinkByNumber, user) { RouteTo = userRoute }));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendTextMessageRequest));
            SendTextMessageRequest tr = (SendTextMessageRequest)resp;

            Assert.IsNotNull(tr.Keyboard);
            Assert.AreEqual(fakeGreetingMessages.LiteLinkKnownUser + "\r\n"
                + "LiteLinkRouted "
                + userRoute.FullName, tr.Text);
        }

        [TestMethod]
        public void ConversationStartedStrategy_LiteLink_AcceptedWithoutRoute() {
            DAL.Model.User user = entities.GetUserQ(where: q => q.Where(u => u.FullName != null),
                transport: TransportKind.Test);

            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new LiteDeepLinkStrategy.Context(
                    LiteDeepLinkStrategy.Context.LinkType.LinkByNumber, user)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendTextMessageRequest));
            SendTextMessageRequest tr = (SendTextMessageRequest)resp;

            Assert.IsNotNull(tr.Keyboard);
            Assert.AreEqual(fakeGreetingMessages.LiteLinkKnownUser + "\r\n"
                + fakeGreetingMessages.LiteLinkUnrouted, tr.Text);
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_Rejected() {
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Rejected, new object()));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendTextMessageRequest));
            SendTextMessageRequest tr = (SendTextMessageRequest)resp;
            Assert.AreEqual(fakeGreetingMessages.LinkRejectedOrUnknown, tr.Text);
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_Unknown() {
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Unknown));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendTextMessageRequest));
            SendTextMessageRequest tr = (SendTextMessageRequest)resp;
            Assert.AreEqual(fakeGreetingMessages.LinkRejectedOrUnknown, tr.Text);
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_Picture() {
            WebChatDeepLink wcdl = MakeMessage(null, "image/jpeg", 100);
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new WebChatDeepLinkStrategy.Context(wcdl)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendPictureMessageRequest));
            SendPictureMessageRequest tr = (SendPictureMessageRequest)resp;
            Assert.IsTrue(String.IsNullOrEmpty(tr.Text));
            Assert.IsNotNull(tr.Keyboard);
            Assert.AreEqual(wcdl.MessageToUser.Message.FileInfo.Url, tr.Media);
            Assert.IsTrue(wcdl.MessageToUser.Message.FromTransport.User.FullName.StartsWith(tr.Sender.Name));
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_PictureText() {
            string text = "Этот текст гораздо длиннее, чем вайбер может позволить себе отправить с картинкой, "
                + "поэтому мы его обрежем. И добавим три точки в конце. "
                + "И ещё на всяекий случай одно длинное очень длинное предложение";
            WebChatDeepLink wcdl = MakeMessage(text, "image/jpeg", 100);
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new WebChatDeepLinkStrategy.Context(wcdl)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendPictureMessageRequest));
            SendPictureMessageRequest tr = (SendPictureMessageRequest)resp;
            Assert.IsTrue(text.StartsWith(tr.Text.Substring(0, tr.Text.Length - 3)));
            Assert.IsNotNull(tr.Keyboard);
            Assert.AreEqual(wcdl.MessageToUser.Message.FileInfo.Url, tr.Media);
            Assert.IsTrue(wcdl.MessageToUser.Message.FromTransport.User.FullName.StartsWith(tr.Sender.Name));
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_BigPicture() {
            WebChatDeepLink wcdl = MakeMessage(null, "image/jpeg", SendPictureMessageRequest.MaxImageSize + 1);
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new WebChatDeepLinkStrategy.Context(wcdl)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendFileMessageRequest));
            SendFileMessageRequest tr = (SendFileMessageRequest)resp;
            Assert.IsNotNull(tr.Keyboard);
            Assert.AreEqual(wcdl.MessageToUser.Message.FileInfo.Url, tr.Media);
            Assert.AreEqual(wcdl.MessageToUser.Message.FileInfo.FileName, tr.FileName);
            Assert.AreEqual(wcdl.MessageToUser.Message.FileInfo.FileLength, tr.Size);
            Assert.IsTrue(wcdl.MessageToUser.Message.FromTransport.User.FullName.StartsWith(tr.Sender.Name));
        }

        [TestMethod]
        public void ConversationStartedStrategy_DLResult_BigFile() {
            WebChatDeepLink wcdl = MakeMessage(null, "text/text", SendFileMessageRequest.MaxFileSize + 1);
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            SendMessageRequest resp = strategy.Process(entities, data, new Core.DeepLinkResult(
                Core.DeepLinkResultStatus.Accepted, new WebChatDeepLinkStrategy.Context(wcdl)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(SendUrlMessageRequest));
            SendUrlMessageRequest tr = (SendUrlMessageRequest)resp;
            Assert.IsNotNull(tr.Keyboard);
            Assert.AreEqual(wcdl.MessageToUser.Message.FileInfo.Url, tr.Media);
            Assert.IsTrue(wcdl.MessageToUser.Message.FromTransport.User.FullName.StartsWith(tr.Sender.Name));
        }

        private WebChatDeepLink MakeMessage(string text, string mediaType, int len) {
            DAL.Model.Message msg = entities.SendMessage(
                from: Global.SystemBotId,
                to: Global.SystemBotId,
                fromt: TransportKind.FLChat,
                tot: TransportKind.WebChat,
                text: text,
                fileMediaType: mediaType,
                fileLength: len);
            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == msg.Id).Single();
            return wcdl;
        }
    }
}
