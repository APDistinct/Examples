using System;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class SimpleMsgTextFileCompilerTests
    {
        private readonly string _id = "%id%";

        [TestMethod]
        public void SimpleMsgTextFileCompilerTestNoFile()
        {
            string command = "command/%id%";
            SimpleMsgTextFileCompiler compiler = new SimpleMsgTextFileCompiler(command);
            Message mtu = new Message()
            {
                    Text = "some text"
            };
            Assert.AreEqual(mtu.Text, compiler.MakeText(mtu));
        }

        [TestMethod]
        public void SimpleMsgTextFileCompilerTestWithFile()
        {
            string command = "command/%id%";
            SimpleMsgTextFileCompiler compiler = new SimpleMsgTextFileCompiler(command);
            Message mtu = new Message()
            {
                Text = "some text",
                FileInfo = new FileInfo() { Id = Guid.NewGuid()},
            };
            Assert.AreEqual(mtu.Text+ "\n" + command.Replace(_id, mtu.FileInfo.Id.ToString()), compiler.MakeText(mtu));
        }
    }
}
