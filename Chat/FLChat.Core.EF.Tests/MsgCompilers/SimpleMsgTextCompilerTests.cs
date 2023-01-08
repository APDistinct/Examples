using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class SimpleMsgTextCompilerTests
    {
        [TestMethod]
        public void SimpleMsgTextCompilerTest() {
            SimpleMsgTextCompiler compiler = new SimpleMsgTextCompiler();
            MessageToUser mtu = new MessageToUser() {
                Message = new Message() {
                    Text = "some text"
                }
            };
            Assert.AreEqual(mtu.Message.Text, compiler.MakeText(mtu));
        }
    }
}
