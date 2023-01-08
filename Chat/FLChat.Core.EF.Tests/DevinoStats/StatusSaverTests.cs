using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.DevinoStats.Tests
{
    [TestClass]
    public class StatusSaverTests
    {
        [TestMethod]
        public void ToStatsSaveTableTest()
        {
            MessageStats msV = new MessageStats()
            {
                TransportId = "qwert",
                SentTo  = 3,
                ViberStatus = 1,
                SmsStatus = null,
                UpdatedTime = DateTime.UtcNow,
                IsFinished = false,
            };
            MessageStats msS = new MessageStats()
            {
                TransportId = "qwert12",
                SentTo = null,
                ViberStatus = null,
                SmsStatus = 2,
                UpdatedTime = DateTime.UtcNow,
                IsFinished = false,
            };
            DataTable dt = new MessageStats[] { msV, msS }.ToStatsSaveTable();

            Assert.AreEqual(2, dt.Rows.Count);
            AreEqual(msV, dt.Rows[0]);
            AreEqual(msS, dt.Rows[1]);
        }

        private void AreEqual(MessageStats ms, DataRow row)
        {
            AreEqual(ms.TransportId, row["TransportId"]);
            AreEqual(ms.SentTo, row["SentTo"]);
            AreEqual(ms.ViberStatus, row["ViberStatus"]);
            AreEqual(ms.SmsStatus, row["SmsStatus"]);
            AreEqual(ms.UpdatedTime, row["UpdatedTime"]);
            AreEqual((bool?)ms.IsFinished, row["IsFinished"]);
        }

        private void AreEqual<T>(T expected, object obj) where T : class
        {
            if (expected != null)
                Assert.AreEqual(expected, (T)obj);
            else
                Assert.AreEqual(DBNull.Value, obj);
        }

        private void AreEqual<T>(T? expected, object obj) where T : struct
        {
            if (expected.HasValue)
                Assert.AreEqual(expected.Value, (T?)obj);
            else
                Assert.AreEqual(DBNull.Value, obj);
        }
    }
}
