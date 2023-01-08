using System;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class DevinoViberSmsCompilerTests
    {
        private MessageToUser mtu;
        private User user;
        //private readonly string refStr = "TeStT !";

        [TestInitialize]
        public void Init()
        {
            
            user = new User()
            {
                FullName = "someNsomeN",
                City = new City() { Id = 1, Name = "CityOfTulun", }
            };

            mtu = new MessageToUser()
            {
                ToTransport = new Transport()
                {
                    User = user,
                },
                Message = new Message()
                {
                    Text = "some text",
                    ForwardMsg = new Message()
                    {
                        Text = "another text",
                    },                     
                },
            };
        }

        [TestMethod]
        public void MakeTextTest_Text()
        {
            string pattern = "Some words befour ";
            string text = " and after";
            string textf = " forward!";
            mtu.Message.ForwardMsg.Text = textf;

            DevinoViberSmsCompiler dvsc = new DevinoViberSmsCompiler( pattern);
            var ret = dvsc.MakeText(mtu, text);
            Assert.AreEqual(pattern + "\n\n" + textf, ret);
        }

        [TestMethod]
        public void MakeTextTest_Null()
        {
            string pattern = "Some words befour ";
            string text = " and after";
            //string textf = " forward!";
            mtu.Message.ForwardMsg.Text = null;

            DevinoViberSmsCompiler dvsc = new DevinoViberSmsCompiler(pattern);
            var ret = dvsc.MakeText(mtu, text);
            Assert.AreEqual(pattern + "\n\n", ret);
        }
    }
}
