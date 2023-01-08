using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

using FLChat.DAL.Import.Excel.ColumnIndexes;

namespace FLChat.DAL.Import.Excel.Tests
{
    [TestClass]
    public class SourceUserTests
    {
        [TestMethod]
        public void SourceUser_Test() {
            ExcelPackage excel = new ExcelPackage(new System.IO.FileInfo(".\\Res\\ColumnIndexes.xlsx"));
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[1];
            FixedColumnIndexes ind = new FixedColumnIndexes();

            SourceUser user = new SourceUser(worksheet, ind, 2);
            Assert.AreEqual(2726553, user.FLUserNumber);
            Assert.AreEqual(512985, user.ParentFLUserNumber);
            Assert.AreEqual("Ермольчик", user.Surname);
            Assert.AreEqual("Марина", user.Name);
            Assert.AreEqual("Васильевна", user.Partronymic);
            Assert.AreEqual(new DateTime(1975, 09, 30), user.Birthday);
            Assert.AreEqual("111", user.Phone);
            Assert.AreEqual("222", user.Email);
            Assert.IsNull(user.Title);
            Assert.AreEqual("220019", user.ZipCode);
            Assert.AreEqual("Беларусь", user.Country);
            Assert.AreEqual("Минская", user.Region);
            Assert.AreEqual("Минск", user.City);
            Assert.AreEqual(new DateTime(2009, 01, 03), user.RegDate);
            Assert.IsFalse(user.EmailPermission.Value);
            Assert.IsTrue(user.SmsPermission.Value);
            Assert.IsFalse(user.IsDirector.Value);
            Assert.AreEqual(new DateTime(2019, 07, 01), user.LastOrder);
            Assert.AreEqual((decimal)27.74, user.LoScores);
            Assert.AreEqual(0, user.PeriodWoLo);
            Assert.AreEqual((decimal)13.5, user.OlgScores);
            Assert.AreEqual((decimal)15.3, user.GoScores);
            Assert.AreEqual((decimal)12, user.CashbackBalance);
            Assert.AreEqual((decimal)23.77, user.FLClubPoints);
            Assert.AreEqual((decimal)18, user.FLClubPointsBurn);
        }
    }
}
