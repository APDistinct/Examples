using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.TelegramBot.Exceptions
{
    public class HandleUpdateException : Exception
    {
        public HandleUpdateException() {
        }

        public HandleUpdateException(string message) : base(message) {
        }

        public HandleUpdateException(string message, Exception innerException) : base(message, innerException) {
        }

        protected HandleUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
