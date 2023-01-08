using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class FileMsgTextCompiler : IMessageTextCompiler
    {
        private TagReplaceTextCompiler compiller;
        private string _pattern;
        private readonly string _id = "%id%";

        public FileMsgTextCompiler(string patternLink, string patternFile)
        {
            _pattern = patternLink;
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
               = new Dictionary<string, Func<MessageToUser, string>>()
               {
                    { "FileLink", mtu => patternFile.Replace(_id, mtu.Message.FileInfo.Id.ToString() ?? "") },
                    { "FileText", mtu => mtu.Message.FileInfo.FileName ?? "Файл для скачивания" },
               };
            
            compiller = new TagReplaceTextCompiler(ReplaceDict, true);
        }

        public string MakeText(MessageToUser mtu, string text)
        {            
            if (mtu.Message.FileInfo == null)
                return text;
            else
                return String.Concat(text, "   ", compiller.MakeText(mtu, _pattern));
        }
    }
}
