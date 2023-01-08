using System;
using System.Collections.Generic;
using System.Linq;
using FLChat.Core.Media;
using FLChat.FileParser.Exceptions;

namespace FLChat.FileParser
{
    public enum FileType
    {
        Txt,
        Csv,
        Excel
    }

    public class PhoneFileParser : IPhoneFileParser
    {
        private readonly IPhoneParser _phoneParser;
        public PhoneFileParser(IPhoneParser phoneParser = null)
        {
            _phoneParser = phoneParser ?? new PhoneParser();
        }

        public List<string> Parse(string file, string name)
        {
            IFileReader reader = GetParser(name);
            List<string> phones = reader.Parse(file);
            List<string> phonesOk = ValidateNumbers(phones);

            return phonesOk;
        }

        private List<string> ValidateNumbers(List<string> phones)
        {
            var validList = new List<string>();
            foreach (var phone in phones)
            {
                if(_phoneParser.TryParse(phone, out string value))
                {
                    validList.Add(value);
                }
            }

            return validList.Distinct().ToList();
        }

        private IFileReader GetParser(string name)
        {
            var type = GetFileType(name);

            switch (type)
            {
                case FileType.Txt:
                    return new CsvReader();
                case FileType.Csv:
                    return new CsvReader(); 
                case FileType.Excel:
                    return new ExcelFileReader();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private FileType GetFileType(string name)
        {
            if (name.EndsWith(".txt"))
                return FileType.Txt;
            if (name.EndsWith(".csv"))
                return FileType.Csv;
            if (name.EndsWith(".xlsx"))
                return FileType.Excel;

            throw new WrongFileTypeException(name);
        }
    }
}
