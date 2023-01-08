using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.MsgCompilers
{
    public class DevinoMsgUserHtmlCompiler : IMessageTextCompiler//, IMessageBulkTextCompiler
    {
        //private readonly string _command;
        private readonly string _fullname = "%fullname%";
        private readonly string _text = "%text%";
        private readonly string _senderavatar = "%senderavatar%";
        private readonly string _sendername = "%sendername%";
        private readonly string _senderrank = "%senderrank%";
        //private readonly string _filename = "%filename%";
        private readonly string _senderid = "%id%";
        //private readonly string _fileid = "%id%";
        private readonly string filename;        
        private readonly string avatarname;
        private string template;

        public DevinoMsgUserHtmlCompiler(string filen, string avatarn, string templ)
        {
            filename = filen;
            template = templ;
            avatarname = avatarn;
        }

        //public string MakeText(MessageToUser mtu)
        //{
        //    string fullname = 
        //        mtu..ToUser;
        //    string text = "%text%";
        //    string senderavatar = "%senderavatar%";
        //    string sendername = "%sendername%";
        //    string senderrank = "%senderrank%";
        //    string filename;

        //    return MakeText(mtu.Message);
        //}

        public string MakeText(MessageToUser mtu, string text_in)
        {
            //string template = "";  //  Шаблон текста письма

            string fullname = "";  // Имя получателя
            string sendername = "";  // Имя отправителя
            string senderrank = "";  // Ранг отправителя
            string senderavatar = "";  // Аватар отправителя
            string text; //  Текст письма
            string textOut; //  Текст письма переработанный

            string senderid = "";  // Id отправителя в БД
            //string filename = "";  // Имя файла
            //string avatarname = "";  // Имя аватара
                        
                var userTo = mtu.ToTransport.User;                    
                if (userTo != null)
                {
                    fullname = userTo.FullName;
                }
                var userFrom = mtu.Message.FromTransport.User;
                if (userFrom != null)
                {
                    sendername = userFrom.FullName;
                    senderrank = userFrom.Rank.Name;
                    senderid = userFrom.Id.ToString();
                    //  Ссылка на имя аватара
                    senderavatar = avatarname.Replace(_senderid, senderid);
                }

                text = text_in;
            //}
            textOut = template;
            textOut = textOut.Replace(_fullname, fullname);
            textOut = textOut.Replace(_sendername, sendername);
            textOut = textOut.Replace(_senderrank, senderrank);
            textOut = textOut.Replace(_senderavatar, senderavatar);
            textOut = textOut.Replace(_text, text);

            return textOut;            
        }

        //public string MakeText(Message mtu)
        //{
        //    string append = "";           

        //    if (mtu.FileInfo != null)
        //    {
        //        Guid fileId = mtu.FileInfo.Id;
        //        append = "\n" + _command.Replace(_id, fileId.ToString());
        //    }
        //    return mtu.Text + append;
        //}
    }
}
