using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class SqlExtentionsTests
    {
        [TestMethod]
        public void SqlExtentions_CreateDataTable() {
            IEnumerable<Guid> list = null;
            DataTable dt = list.CreateDataTable("Guid");
            Assert.AreEqual(0, dt.Rows.Count);
        }

        [TestMethod]
        public void SqlExtentions_CreateUserIdDeepDT() {
            IEnumerable<Tuple<Guid, int?>> list = null;
            DataTable dt = list.CreateUserIdDeepDT();
            Assert.AreEqual(0, dt.Rows.Count);
        }
    }
}
