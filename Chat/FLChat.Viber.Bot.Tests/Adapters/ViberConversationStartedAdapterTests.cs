using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberConversationStartedAdapterTests
    {
        [TestMethod]
        public void ViberConversationStartedAdapter_Test() {
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            ViberConversationStartedAdapter adapter = new ViberConversationStartedAdapter(data);
            Assert.AreEqual(data.User.Id, adapter.FromId);
            Assert.AreEqual(data.User.Name, adapter.FromName);
            Assert.AreEqual("DeepLink " + data.Context, adapter.Text);
            Assert.IsNull(adapter.PhoneNumber);
            Assert.IsNotNull(adapter.DeepLink);
            Assert.AreEqual(data.Context, adapter.DeepLink);
            Assert.IsNull(adapter.ReplyToMessageId);
            Assert.AreEqual(data.MessageToken.ToString(), adapter.MessageId);
        }

        [TestMethod]
        public void ViberConversationStartedAdapter_WithoutContext() {
            CallbackData data = Helper.ReadJson("callback_conversation_started");
            data.Context = null;
            Assert.ThrowsException<ViberAdapterException>( () => new ViberConversationStartedAdapter(data));
        }

        [TestMethod]
        public void ViberConversationStartedAdapter_OtherEvents() {
            foreach (CallbackData data in Helper.CreateCallbackDataForAllEvents(new CallbackEvent[] { CallbackEvent.ConversationStarted }))
                Assert.ThrowsException<ViberAdapterException>(() => new ViberConversationStartedAdapter(data));
        }
    }
}
