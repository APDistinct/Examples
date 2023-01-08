using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Logger
{
    public class FileLogExceptions : Exception
    {
        public FileLogExceptions()
        {
        }

        public FileLogExceptions(string message) : base(message)
        {
        }

        public FileLogExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
