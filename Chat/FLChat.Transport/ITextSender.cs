using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Transport
{
    public interface ITextSender
    {
        void Send(string addressee, string text);
    }
}
