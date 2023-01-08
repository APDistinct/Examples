using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.MsgCompilers
{
    public class TTNothing : ITextTransform
    {
        public string Transform(string sourse) => sourse;
    }
}
