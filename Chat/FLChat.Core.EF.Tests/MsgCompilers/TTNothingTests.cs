using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class TTNothingTests
    {
        [TestMethod]
        public void TTNothing_Test() {
            string s = "123_*";
            TTNothing tt = new TTNothing();
            Assert.AreSame(s, tt.Transform(s));
        }
    }
}
