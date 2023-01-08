using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Tests
{
    public class FakeDeepLinkGenerator : IDeepLinkGenerator
    {
        private string refStr;
        public FakeDeepLinkGenerator(string refstr = null)
        {
            refStr = refstr ?? "";
        }
        public string Generate(User u)
        {
            return u != null ? refStr : "";       
        }
    }
}
