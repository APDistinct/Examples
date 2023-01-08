using System;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class FileMsgTextCompilerTests
    {
        private FileMsgTextCompiler compiler;
        private readonly string _id = "%id%";
        private string command = "command/%id%";
        private static readonly string FileLink = "FileLink";
        private static readonly string FileText = "FileText";
        private string pattern = $"https://vk.com/away.php?to=#{FileLink}&cc_key=#{FileText}";

        [TestInitialize]
        public void Init()
        {
            
            compiler = new FileMsgTextCompiler(pattern, command);
        }

        [TestMethod]
        public void FileMsgTextCompilerTestNoFile()
        {        
            string text = "Test mess";
            MessageToUser mtu = new MessageToUser()
            {
                Message = new Message()
                {
                Text = "some text",
                },
            };
            Assert.AreEqual(text, compiler.MakeText(mtu,text));
        }

        [TestMethod]
        public void FileMsgTextCompilerTestWithFile()
        {
            string text = "Test mess";
            MessageToUser mtu = new MessageToUser()
            {
                Message = new Message()
                {
                    Text = "some text",
                    FileInfo = new FileInfo() { Id = Guid.NewGuid(), FileName = "name" },
                },
            };
            string fname = command.Replace(_id, mtu.Message.FileInfo.Id.ToString());
            string ret = pattern.Replace("#"+FileLink, fname);
            ret = ret.Replace("#" + FileText, mtu.Message.FileInfo.FileName);
            Assert.AreEqual(text + "   " + ret, compiler.MakeText(mtu,text));
        }
    }
}
