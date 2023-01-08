using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

namespace FLChat.DAL.Import.Excel.ColumnIndexes.Tests
{
    [TestClass]
    public class FixedColumnIndexesTests
    {
        [TestMethod]
        public void FixedColumnIndexes () {
            ExcelPackage excel = new ExcelPackage(new System.IO.FileInfo(".\\Res\\ColumnIndexes.xlsx"));
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[1];
            FixedColumnIndexes ind = new FixedColumnIndexes();
            Assert.AreEqual("CONSULTANTNUMBER", worksheet.Cells[1, ind.FLUserNumber].Value.ToString().Trim());
            Assert.AreEqual("MENTORNUMBER", worksheet.Cells[1, ind.ParentFLUserNumber.Value].Value.ToString().Trim());
            Assert.AreEqual("SURNAME", worksheet.Cells[1, ind.Surname.Value].Value.ToString().Trim());
            Assert.AreEqual("NAME", worksheet.Cells[1, ind.Name.Value].Value.ToString().Trim());
            Assert.AreEqual("PATRONYMIC", worksheet.Cells[1, ind.Partronymic.Value].Value.ToString().Trim());
            Assert.AreEqual("BIRTHDAY", worksheet.Cells[1, ind.Birthday.Value].Value.ToString().Trim());
            Assert.AreEqual("MOBILE", worksheet.Cells[1, ind.Phone.Value].Value.ToString().Trim());
            Assert.AreEqual("EMAIL", worksheet.Cells[1, ind.Email.Value].Value.ToString().Trim());
            Assert.AreEqual("TITLE", worksheet.Cells[1, ind.Title.Value].Value.ToString().Trim());
            Assert.AreEqual("ZIP", worksheet.Cells[1, ind.ZipCode.Value].Value.ToString().Trim());
            Assert.AreEqual("COUNTRY", worksheet.Cells[1, ind.Country.Value].Value.ToString().Trim());
            Assert.AreEqual("REGION", worksheet.Cells[1, ind.Region.Value].Value.ToString().Trim());
            Assert.AreEqual("CITY", worksheet.Cells[1, ind.City.Value].Value.ToString().Trim());
            Assert.AreEqual("REGISTRATIONDATE", worksheet.Cells[1, ind.RegDate.Value].Value.ToString().Trim());
            Assert.AreEqual("EMAILPERMISSION", worksheet.Cells[1, ind.EmailPermission.Value].Value.ToString().Trim());
            Assert.AreEqual("SMSPERMISSION", worksheet.Cells[1, ind.SmsPermission.Value].Value.ToString().Trim());
            Assert.AreEqual("ISDIRECTOR", worksheet.Cells[1, ind.IsDirector.Value].Value.ToString().Trim());
            Assert.AreEqual("LASTORDERDATE", worksheet.Cells[1, ind.LastOrder.Value].Value.ToString().Trim());
            Assert.AreEqual("LO", worksheet.Cells[1, ind.LoScores.Value].Value.ToString().Trim());
            Assert.AreEqual("PERIODSWOLO", worksheet.Cells[1, ind.PeriodWoLo.Value].Value.ToString().Trim());
            Assert.AreEqual("OLG", worksheet.Cells[1, ind.OlgScores.Value].Value.ToString().Trim());
            Assert.AreEqual("GO", worksheet.Cells[1, ind.GoScores.Value].Value.ToString().Trim());
            Assert.AreEqual("CASHBACKBALANCE", worksheet.Cells[1, ind.CashbackBalance.Value].Value.ToString().Trim());
            Assert.AreEqual("FLCLUBPOINTS", worksheet.Cells[1, ind.FLClubPoints.Value].Value.ToString().Trim());
            Assert.AreEqual("FLCLUBPOINTSBURN", worksheet.Cells[1, ind.FLClubPointsBurn.Value].Value.ToString().Trim());            
        }
    }
}
