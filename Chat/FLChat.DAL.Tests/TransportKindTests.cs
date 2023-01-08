using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class TransportKindTests
    {
        [TestMethod]
        public void TransportKind_AssignOutOfRange() {
            TransportKind kind = (TransportKind)999;
            foreach(int v in Enum.GetValues(typeof(TransportKind))) {
                Assert.AreNotEqual(kind, v);
            }

            Assert.AreEqual(999, (int)kind);
        }
    }
}
