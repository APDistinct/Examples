using System;
using System.Collections.Generic;
using FLChat.DAL.Model;
using FLChat.WebService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class ChainCompilerTests
    {
        private class FakeAddMessageCompiler : IMessageTextCompiler
        {
            private string _pattern;

            public FakeAddMessageCompiler(string pattern = "fake one ")
            {
                _pattern = pattern;
            }

            public string MakeText(MessageToUser mtu, string text)
            {
                return text + _pattern;
            }            
        }

        private class FakeNullMessageCompiler : IMessageTextCompiler
        {
            public string MakeText(MessageToUser mtu, string text)
            {
                return "";
            }
        }

        private ChainCompiler compiller;
        private MessageToUser mtu;
        private User user;

        [TestInitialize]
        public void Init()
        {
            compiller = new ChainCompiler();            
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
                },
            };
        }

        [TestMethod]
        public void ChainCompilerTest_Create()
        {
            List<IMessageTextCompiler> list = new List<IMessageTextCompiler>();
            list.Add(new FakeAddMessageCompiler());
            list.Add(new FakeAddMessageCompiler());
            list.Add(new FakeAddMessageCompiler());
            list.Add(new FakeNullMessageCompiler());
            list.Add(new FakeAddMessageCompiler());
            ChainCompiler compiller = new ChainCompiler(list);
            string text = "";
            var ret = compiller.MakeText(mtu, text);
            Assert.AreEqual(ret.WordsCount("fake one "), 1);
        }

        [TestMethod]
        public void ChainCompilerTest_Add()
        {
            ChainCompiler compiller = new ChainCompiler();
            string text = null;
            var ret = compiller.MakeText(mtu, text);
            Assert.AreEqual(text, ret);
            text = "123";
            ret = compiller.MakeText(mtu, text);
            Assert.AreEqual(text, ret);
            
            compiller.Add(new FakeAddMessageCompiler());
            compiller.Add(new FakeAddMessageCompiler());
            compiller.Add(new FakeAddMessageCompiler());
            compiller.Add(new FakeNullMessageCompiler());
            compiller.Add(new FakeAddMessageCompiler());            
            
            ret = compiller.MakeText(mtu, text);
            Assert.AreEqual(ret.WordsCount("fake one "), 3);
        }

        [TestMethod]
        public void ChainCompilerTest_Priority()
        {
            ChainCompiler compiller = new ChainCompiler();
            
            string st1 = "1234";
            string st2 = "567";

            compiller.Add(new FakeAddMessageCompiler(st1));
            compiller.Add(new FakeAddMessageCompiler(st2));
            compiller.Add(new FakeNullMessageCompiler());
            string ret = compiller.MakeText(mtu, "");
            Assert.AreEqual(st2+st1, ret);
        }
    }
}
