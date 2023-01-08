using System;
using FLChat.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class TextCodeSenderTests
    {
        public class ActionTextSender : ITextSender
        {
            private Action<string, string> _action;

            public ActionTextSender(Action<string, string> action) {
                _action = action;
            }

            public void Send(string addressee, string text) {
                _action(addressee, text);
            }
        }
    }
}
