using FLChat.DAL.Model;
using System;

namespace FLChat.Core.MsgCompilers
{
    public class SimpleMsgTextFileCompiler : IMessageTextCompiler, IMessageBulkTextCompiler
    {
        private readonly string _command;
        private readonly string _id = "%id%";

        public SimpleMsgTextFileCompiler(string command)
        {
            _command = command;
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            return MakeText(mtu.Message, text);
        }

        public string MakeText(Message mtu, string text)
        {
            string append = "";

            if (mtu.FileInfo != null)
            {
                Guid fileId = mtu.FileInfo.Id;
                append = "\n" + _command.Replace(_id, fileId.ToString());
            }
            return text + append;
        }
    }
}