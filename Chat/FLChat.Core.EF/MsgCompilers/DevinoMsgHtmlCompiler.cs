using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class DevinoMsgHtmlCompiler : IMessageTextCompiler, IMessageBulkTextCompiler
    {
        //private readonly string _command;
        //private readonly string _fullname = "%fullname%";
        private readonly string _text = "%text%";
        private readonly string _senderavatar = "%senderavatar%";
        private readonly string _sendername = "%sendername%";
        private readonly string _senderrank = "%senderrank%";
        private readonly string _senderfile = "%senderfile%";
        //private readonly string _filename = "%filename%";
        private readonly string _senderid = "%id%";
        private readonly string _fileid = "%id%";
        private readonly string filename;
        private readonly string avatarname;
        private readonly string avatardefname;
        private readonly string template;

        public DevinoMsgHtmlCompiler(string filen, string avatarn, string avatard, string templ)
        {
            filename = filen;
            template = templ;
            avatarname = avatarn;
            avatardefname = avatard;
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
        public string MakeText(MessageToUser mtu, string text)
        {
            return MakeText(mtu.Message, text);
        }

        public string MakeText(Message mtu, string text_in)
        {
            //string template = "";  //  Шаблон текста письма

            //string fullname = "";  // Имя получателя
            string sendername = "";  // Имя отправителя
            string senderrank = "";  // Ранг отправителя
            string senderavatar = "";  // Аватар отправителя
            string senderfile = "";  //  Файл сообщения
            string text; //  Текст письма
            string textOut; //  Текст письма переработанный

            string senderid = "";  // Id отправителя в БД
            string fileid = "";
                                   //string filename = "";  // Имя файла
                                   //string avatarname = "";  // Имя аватара


            //var userTo = mtu.ToTransport.User;
            ////entities.User.Where(x => x.Id == mtu.ToUserId).FirstOrDefault();
            //if (userTo != null)
            //{
            //    fullname = userTo.FullName;
            //}

            var userFrom = mtu.FromTransport.User;
            if (userFrom != null)
            {
                sendername = userFrom.FullName ?? "";
                senderrank = userFrom.Rank?.Name ?? "";
                senderid = userFrom.Id.ToString();
                //  Ссылка на имя аватара
                // Проверка на существование
                if (userFrom.UserAvatar != null)
                {
                    senderavatar = avatarname.Replace(_senderid, senderid);
                }
                else
                {
                    senderavatar = avatardefname;
                }
            }

            text = text_in;
            if(mtu.FileInfo != null)
            {
                fileid = mtu.FileInfo.Id.ToString();
                senderfile = "<a href=\"" + filename.Replace(_fileid, fileid) + "\"> Файл </a>";
            }
            //}
            textOut = template;
            //template = template.Replace(_fullname, fullname);
            textOut = textOut.Replace(_sendername, sendername);
            textOut = textOut.Replace(_senderrank, senderrank);
            textOut = textOut.Replace(_senderavatar, senderavatar);
            textOut = textOut.Replace(_text, MakeHtmlText(text));
            textOut = textOut.Replace(_senderfile, senderfile);

            return textOut;
        }

        private string MakeHtmlText(string text)
        {
            StringBuilder newText = new StringBuilder(text.Length * 2);
            var arr = text.Split(new char[] { '\n' }, StringSplitOptions.None);
            foreach(var str in arr)
            {
                newText.Append( $"<p>{str}</p>");
            }
            return newText.ToString();
        }
    }
}


