using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public static class MessageErrorExtentions
    {
        public static MessageError ToMessageError(this Exception e) {
            return new MessageError() {
                Type = e.GetType().Name,
                Descr = e.Message,
                Trace = e.ToString()
            };
        }
    }
}
