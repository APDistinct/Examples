using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public interface IMessageDelayTimeChecker
    {
        bool DelayCheck(DateTime dt);
        bool CancellCheck(DateTime dt);
    }

    public class MessageDelayTimeChecker : IMessageDelayTimeChecker
    {
        private readonly int _delay = 1000;
        private readonly int _cancell = 1000;

        public bool DelayCheck(DateTime dt)
        {
            return dt.AddMilliseconds(_delay) < DateTime.UtcNow;
        }

        public bool CancellCheck(DateTime dt)
        {
            return dt.AddMilliseconds(_cancell) < DateTime.UtcNow;
        }
    }
}
