using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Bot.Exceptions
{
    public class ViberAdapterException : Exception
    {
        public ViberAdapterException(string message) : base(message) {
        }
    }
}
