using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL.Model;

namespace FLChat.WebService
{
    public class FakeMessageTextCompiler : IMessageTextCompilerWithCheck
    {
        private Func<string, bool> _isChangable;
        private Func<MessageToUser, string, string> _makeText;

        public FakeMessageTextCompiler(Func<string, bool> isChangable, Func<MessageToUser, string, string> makeText) {
            _isChangable = isChangable;
            _makeText = makeText;
        }

        public bool IsChangable(string text) {
            return _isChangable(text);
        }

        public string MakeText(MessageToUser mtu, string text) {
            return _makeText(mtu, text);
        }
    }
}
