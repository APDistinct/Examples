using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class CombiCompiler : IMessageTextCompiler
    {
        private readonly IMessageTextCompiler LeftHandCompiler;
        private readonly IMessageTextCompiler RightHandCompiler;
        readonly Func<MessageToUser, bool> Func;

        public CombiCompiler(IMessageTextCompiler leftHandCompiler, IMessageTextCompiler rightHandCompiler, Func<MessageToUser, bool> func)
        {
            LeftHandCompiler = leftHandCompiler;
            RightHandCompiler = rightHandCompiler;
            Func = func;
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            if (Func(mtu))
                return LeftHandCompiler.MakeText(mtu, text);
            else
                return RightHandCompiler.MakeText(mtu, text);            
        }
    }
}
