using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.FileParser.Exceptions
{
    public class WrongFileTypeException : Exception
    {
        public string FileName { get; set; }
        public WrongFileTypeException(string fileName) : base($"Wrong file format: {fileName}")
        {
            FileName = fileName;
        }
    }
}
