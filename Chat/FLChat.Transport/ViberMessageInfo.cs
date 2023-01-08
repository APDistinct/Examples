using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Transport
{
    public class ViberMessageInfo
    {
        public string Text { get; set; }
        public string Caption { get; set; }
        public string Action { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string SmsText { get; set; }
    }
}
