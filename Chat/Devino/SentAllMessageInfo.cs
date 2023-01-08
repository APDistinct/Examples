using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devino
{
    public class SentAllMessageInfo
    {
        public SentAllMessageInfo(List<string> providerId, DateTime sentTime)
        {
            ProviderId = providerId.ToArray();
            SentTime = sentTime;
        }

        public string[] ProviderId { get; }
        public DateTime SentTime { get; }
    }
}
