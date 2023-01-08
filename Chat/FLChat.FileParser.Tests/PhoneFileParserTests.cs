using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.FileParser.Tests
{
    [TestClass]
    public class PhoneFileParserTests
    {
        [TestMethod]
        public void ReadCsv_IgnoreDuplicateNumbersAndWrongStrings()
        {
            var fileName = "phonesForDifferentCountries.csv";
            var parser = new PhoneFileParser();
            string file = TestHelper.GetCsvFile(fileName);

            List<string> phones = parser.Parse(file, fileName);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(6, phones.Count);
        }  
        
        [TestMethod]
        public void ReadExcel_IgnoreDuplicateNumbersAndWrongStrings()
        {
            var fileName = "excel.xlsx";
            var parser = new PhoneFileParser();
            string file = TestHelper.GetExcelFile(fileName);

            List<string> phones = parser.Parse(file, fileName);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(5, phones.Count);
        }   
        
        [TestMethod]
        public void ReadExcel_100000Phones()
        {
            var fileName = "100000.xlsx";
            var parser = new PhoneFileParser();
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            string file = TestHelper.GetExcelFile(fileName);
            Console.WriteLine($"GetExcelFile:   {stopwatch.Elapsed.TotalSeconds}");
            
            stopwatch.Restart();
            List<string> phones = parser.Parse(file, fileName);
            Console.WriteLine($"Parse:          {stopwatch.Elapsed.TotalSeconds}");

            //foreach (var phone in phones)
            //{
            //    Console.WriteLine(phone);
            //}

            Console.WriteLine($"Total phones:   {phones.Count}");
            //Assert.AreEqual(5, phones.Count);
        }


        [TestMethod]
        public void ReadExcel_ItalyPhones()
        {
            var fileName = "Италия.xlsx";
            var parser = new PhoneFileParser();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            string file = TestHelper.GetExcelFile(fileName);
            Console.WriteLine($"GetExcelFile:   {stopwatch.Elapsed.TotalSeconds}");

            stopwatch.Restart();
            List<string> phones = parser.Parse(file, fileName);
            Console.WriteLine($"Parse:          {stopwatch.Elapsed.TotalSeconds}");

            Console.WriteLine($"Total phones:   {phones.Count}");
        }
    }
}
