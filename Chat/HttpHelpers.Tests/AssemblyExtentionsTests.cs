using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Reflection.Tests
{
    [TestClass]
    public class AssemblyExtentionsTests
    {
        [TestMethod]
        public void AssemblyExtentions_LoadResourceByFullNameTest() {
            var assembly = Assembly.GetExecutingAssembly();
            string s = assembly.LoadResourceByFullName("HttpHelpers.Tests.Resources.Test.txt");
            Assert.AreEqual("test data", s);
        }

        [TestMethod]
        public void AssemblyExtentions_LoadResourceTest() {
            var assembly = Assembly.GetExecutingAssembly();
            string s = assembly.LoadResource(".Resources.Test.txt");
            Assert.AreEqual("test data", s);
        }

        [TestMethod]
        public void AssemblyExtentions_LoadResourceByTailTest() {
            var assembly = Assembly.GetExecutingAssembly();
            string s = assembly.LoadResourceByTail("Test.txt");
            Assert.AreEqual("test data", s);
        }
    }
}
