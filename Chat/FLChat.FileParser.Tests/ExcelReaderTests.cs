using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.FileParser.Tests
{
    [TestClass]
    public class ExcelReaderTests
    {
        [TestMethod]
        public void ReadExcel()
        {
            var reader = new ExcelFileReader();
            string file = TestHelper.GetExcelFile("excel.xlsx");

            List<string> phones = reader.Parse(file);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(15, phones.Count);
        }
    }
}
