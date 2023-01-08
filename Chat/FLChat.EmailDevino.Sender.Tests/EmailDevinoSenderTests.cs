using System;
using System.Threading;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.EmailDevino.Sender.Tests
{
    [TestClass]
    public class EmailDevinoSenderTests
    {
        //private CancellationTokenSource _cts;

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    _cts = new CancellationTokenSource();
        //    //string getFileCommand = "%id%"; // GetFileCommand();
        //    //GetHtmlParas(out string filen, out string avatarn, out string templ);
        //    GetHtmlParams(out string filen, out string avatarn, out string avatard, out string templ);
        //    ISendingConveyor conv = new SendingConveyorBulk(
        //                CreateClient(),
        //                DAL.TransportKind.Email,
        //                idSaver: new TransportIdSaver(),
        //                msgCompiler: new DevinoMsgHtmlCompiler(filen, avatarn, avatard, templ));

        //    //ISendingConveyor conv = new SendingConveyorBulk(
        //    //    CreateClient(),
        //    //    DAL.TransportKind.Email,
        //    //    idSaver: new TransportIdSaver(),
        //    //    msgCompiler: new SimpleMsgTextFileCompiler(getFileCommand));
        //    conv.Send(_cts.Token);
        //}


        private FLChat.Transport.MailSenderBulk CreateClient()
        {
            FLChat.Transport.MailSenderBulk client = new FLChat.Transport.MailSenderBulk();
            return client;
        }

        private void GetHtmlParas(out string filen, out string avatarn, out string templ)
        {
            filen = "";
            avatarn = "";
            templ = "";
            //using (ChatEntities entities = new ChatEntities())
            {
                //  Шаблон имени файла
                filen = Settings.Values.GetValue("MAINSERVER_NAME", "http://5.188.115.71:33892/FLChat/") +
                Settings.Values.GetValue("COMMAND_GETFILE", "file/%id%");
                //  Шаблон имени аватара
                avatarn = Settings.Values.GetValue("MAINSERVER_NAME", "http://5.188.115.71:33892/FLChat/") +
                Settings.Values.GetValue("COMMAND_AVATAR", "users/%id%/avatar");
                //  Шаблон текста письма
                templ = Settings.Values.GetValue("EMAIL_DEVINO_HTML_TEMPLATE_1", " %text% ");
            }
        }
        private void GetHtmlParams(out string filen, out string avatarn, out string avatard, out string templ)
        {
            filen = "";
            avatarn = "";
            avatard = "";
            templ = "";
            //using (ChatEntities entities = new ChatEntities())
            {
                //  Шаблон имени файла
                filen = Settings.Values.GetValue("MAINSERVER_NAME", "http://5.188.115.71:33892/FLChat/") +
                Settings.Values.GetValue("COMMAND_GETFILE", "file/%id%");
                //  Шаблон имени аватара
                avatarn = Settings.Values.GetValue("MAINSERVER_NAME", "http://5.188.115.71:33892/FLChat/") +
                Settings.Values.GetValue("COMMAND_AVATAR", "users/%id%/avatar");
                //  Ссылка на картинку для дефолтного аватара
                avatard = Settings.Values.GetValue("AVATAR_DEFAULT", "");
                //  Шаблон текста письма
                templ = Settings.Values.GetValue("EMAIL_DEVINO_HTML_TEMPLATE_1", " %text% ");
            }
        }
    }
}
