using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Import.Tests
{
    [TestClass]
    public class ImportUsersFLBatchTests
    {
        [TestMethod]
        public void AdjustPhoneNumbers() {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("MOBILE", typeof(string)));
            AddMobile(dt, "123456789");
            AddMobile(dt, "1(234)56-789");
            dt.AdjustPhoneNumbers();
            for (int i = 0; i < dt.Rows.Count - 1; ++i)
                Assert.AreEqual("123456789", (string)dt.Rows[i][0]);
        }

        [TestMethod]
        public void AdjustPhoneNumbers_null() {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("MOBILE", typeof(string)));
            AddMobile(dt, null);
            dt.AdjustPhoneNumbers();
            for (int i = 0; i < dt.Rows.Count - 1; ++i)
                Assert.IsInstanceOfType(dt.Rows[i][0], typeof(DBNull));
        }

        private void AddMobile(DataTable dt, string phone) {
            var row = dt.NewRow();
            row[0] = phone;
            dt.Rows.Add(row);
        }
    }
}
