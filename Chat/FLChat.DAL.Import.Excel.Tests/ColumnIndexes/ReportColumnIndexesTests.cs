using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OfficeOpenXml;

namespace FLChat.DAL.Import.Excel.ColumnIndexes.Tests
{
    [TestClass]
    public class ReportColumnIndexesTests
    {
        [TestMethod]
        public void ReportColumnIndexes() {
            ExcelPackage excel = new ExcelPackage(new System.IO.FileInfo(".\\Res\\ReportColumnIndexes.xlsx"));
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[1];
            ReportColumnIndexes ind = new ReportColumnIndexes();
            Assert.AreEqual("Номер консультанта", worksheet.Cells[1, ind.FLUserNumber].Value.ToString().Trim());
            Assert.AreEqual("Рег. № наставника", worksheet.Cells[1, ind.ParentFLUserNumber.Value].Value.ToString().Trim());
            Assert.AreEqual("ФИО", worksheet.Cells[1, ind.Surname.Value].Value.ToString().Trim());
            Assert.IsNull(ind.Name);
            Assert.IsNull(ind.Partronymic);
            Assert.IsNull(ind.Birthday);
            Assert.AreEqual("Телефон", worksheet.Cells[1, ind.Phone.Value].Value.ToString().Trim());
            Assert.AreEqual("Email", worksheet.Cells[1, ind.Email.Value].Value.ToString().Trim());
            Assert.IsNull(ind.Title);
            Assert.IsNull(ind.ZipCode);
            Assert.IsNull(ind.Country);
            Assert.IsNull(ind.Region);
            Assert.IsNull(ind.City);
            Assert.AreEqual("Дата регистрации", worksheet.Cells[1, ind.RegDate.Value].Value.ToString().Trim());
            Assert.IsNull(ind.EmailPermission);
            Assert.IsNull(ind.SmsPermission);
            Assert.IsNull(ind.IsDirector);
            Assert.IsNull(ind.LastOrder);
            Assert.AreEqual("ЛО, баллы", worksheet.Cells[1, ind.LoScores.Value].Value.ToString().Trim());
            Assert.AreEqual("Kоличество периодов без ЛО", worksheet.Cells[1, ind.PeriodWoLo.Value].Value.ToString().Trim());
            Assert.AreEqual("ОЛГ, баллы", worksheet.Cells[1, ind.OlgScores.Value].Value.ToString().Trim());
            Assert.AreEqual("ГО, баллы", worksheet.Cells[1, ind.GoScores.Value].Value.ToString().Trim());
            Assert.IsNull(ind.CashbackBalance);
            Assert.IsNull(ind.FLClubPoints);
            Assert.IsNull(ind.FLClubPointsBurn);

            //DateTime dt = new DateTime(2011, 12, 29) - TimeSpan.FromDays(37254);
        }
    }
}
