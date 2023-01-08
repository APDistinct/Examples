using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;

namespace FLChat.FileParser
{
    public interface IFileReader
    {
        List<string> Parse(string file);
    }

    public class CsvReader : IFileReader
    {
        private readonly char _delimiter;
        private readonly bool _hasHeaders;

        public CsvReader(char delimiter = ',', bool hasHeaders = false)
        {
            _delimiter = delimiter;
            _hasHeaders = hasHeaders;
        }

        public List<string> Parse(string fileStrings)
        {
            List<string> phones = fileStrings
                .Split(new[] {"\r\n", "\r", "\n", ",", ";", "|"},  StringSplitOptions.None)
                .Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

            return phones;
        }

        //public List<string> Parse(byte[] file)
        //{
        //    var phones = new List<string>();

        //    using (var csv = new CsvReader(new StreamReader(new MemoryStream(file)), _hasHeaders, _delimiter))
        //    {
        //        int fieldCount = csv.FieldCount;

        //        while (csv.ReadNextRecord())
        //        {
        //            for (int i = 0; i < fieldCount; i++)
        //                phones.Add(csv[i]);
        //        }
        //    }

        //    return phones.ToList();
        //}
    }
}
