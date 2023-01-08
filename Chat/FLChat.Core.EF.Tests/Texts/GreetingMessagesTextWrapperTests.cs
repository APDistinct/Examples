using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Texts.Tests
{
    [TestClass]
    public class GreetingMessagesTextWrapperTests
    {
        [TestMethod]
        public void GreetingMessagesTextWrapper_Test() {
            IGreetingMessagesText texts = new FakeGreetingMessages();
            GreetingMessagesTextWrapper wrapper = new GreetingMessagesTextWrapper(texts);
            Assert.AreSame(texts.LinkRejectedOrUnknown, wrapper.LinkRejectedOrUnknown);
            Assert.AreSame(texts.LiteLinkKnownUser, wrapper.LiteLinkKnownUser);
            Assert.AreSame(texts.LiteLinkRouted, wrapper.LiteLinkRouted);
            Assert.AreSame(texts.LiteLinkUnknownUser, wrapper.LiteLinkUnknownUser);
            Assert.AreSame(texts.LiteLinkUnrouted, wrapper.LiteLinkUnrouted);
        }
    }
}
