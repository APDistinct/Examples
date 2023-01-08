using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Logger.Tests
{
    [TestClass]
    public class FileLoggerTests
    {
        [TestMethod]
        public void LoggerCreateNewFile()
        {
            var logger = new FileLogger();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
            var exist = File.Exists(path);

            Assert.IsTrue(exist);
        } 
        
        [TestMethod]
        public void Logger_CreateNewFileWithWrongPath_ThrowException()
        {
            Assert.ThrowsException<FileLogExceptions>(() =>
            {
                var path = "asd:\\asd\\log.txt";
                var logger = new FileLogger(path);
            });
        }

        [TestMethod]
        public void LoggerLogToFile()
        {
            var logger = new FileLogger();

            logger.Log("Hello");
        }
    }
}
