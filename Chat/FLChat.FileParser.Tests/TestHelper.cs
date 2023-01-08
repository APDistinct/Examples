using System;
using System.IO;

namespace FLChat.FileParser.Tests
{
    public static class TestHelper
    {
        public static string GetCsvFile(string fileName)
        {
            var filePath = $"{Directory.GetCurrentDirectory()}/Csv/{fileName}";
            return File.ReadAllText(filePath);
        }

        public static string GetExcelFile(string fileName)
        {
            var filePath = $"{Directory.GetCurrentDirectory()}/Excel/{fileName}";
            return Convert.ToBase64String(File.ReadAllBytes(filePath));
        }
    }
}