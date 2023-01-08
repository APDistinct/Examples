using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class MessageKindTests
    {
        [TestMethod]
        public void MessageKindExtentions_DefaultTransportViewName() {
            foreach (MessageKind mk in Enum.GetValues(typeof(MessageKind)).Cast<MessageKind>()) {
                if (mk == MessageKind.Mailing)
                    Assert.AreEqual("[Usr].[UserMailingTransportView]", mk.DefaultTransportViewName());
                else
                    Assert.AreEqual("[Usr].[UserDefaultTransportView]", mk.DefaultTransportViewName());
            }
        }
    }
}
