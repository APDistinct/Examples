using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.FileParser.Tests
{
    [TestClass]
    public class CsvReaderTests
    {
        [TestMethod]
        public void ReadCsvFile()
        {
            var reader = new CsvReader();
            string file = TestHelper.GetCsvFile("phonesWithTheSame.csv");
            
            List<string> phones = reader.Parse(file);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(18, phones.Count);
        }    
        
        [TestMethod]
        public void ReadCsvFile3()
        {
            var reader = new CsvReader();
            string file = TestHelper.GetCsvFile("Untitled 1.csv");

            List<string> phones = file.Split(',', ';', '|').ToList();

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(12, phones.Count);
        }  
        
        [TestMethod]
        public void ReadCsvFile2()
        {
            var reader = new CsvReader();
            string file = TestHelper.GetCsvFile("phonesDifferent.csv");
            
            List<string> phones = reader.Parse(file);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(5, phones.Count);
        } 
        
        [TestMethod]
        public void ReadCsvFile1()
        {
            var reader = new CsvReader();
            string file = TestHelper.GetCsvFile("Untitled 1.csv");
            
            List<string> phones = reader.Parse(file);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(12, phones.Count);
        }

        [TestMethod]
        public void ReadTxtFile3()
        {
            var reader = new CsvReader(delimiter:'\n');
            string file = TestHelper.GetCsvFile("phones.txt");
            
            List<string> phones = reader.Parse(file);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }

            Assert.AreEqual(8, phones.Count);
        }
    }
}
