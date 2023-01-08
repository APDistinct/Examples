using Devino;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FLChat.EmailDevino.Sender
{
    public partial class EmailDevinoSenderService : ServiceBase
    {
        //  
        private string filen, avatarn, avatard, templ, subject;

        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;

        public EmailDevinoSenderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Start service");
            _cts = new CancellationTokenSource();
            try
            {
                if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out int delay))
                    throw new ConfigurationErrorsException("Configuration value delay_ms is invalid");
                //string getFileCommand = GetFileCommand();
                GetHtmlParams(/*out string filen, out string avatarn, out string avatard, out string templ*/);
                DevinoSettingsReader dsr = new DevinoSettingsReader();
                dsr.EmailInit();
                Task.Run(() =>
                {
                    ISendingConveyor conv = new SendingConveyor(
                        CreateClient(dsr),
                        DAL.TransportKind.Email,
                        idSaver: new TransportIdSaver(),
                        msgCompiler: MakeTextCompiller(),
                        waiting: false
                        //new SimpleMsgTextCompiler()
                        //new DevinoMsgHtmlCompiler(filen, avatarn, avatard, templ)
                        );
                    //msgCompiler: new SimpleMsgTextFileCompiler(getFileCommand));
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("Sending messages was stopped");
                    _cts.Dispose();
                });
                //Task.Run(() =>
                //{
                //    ISendingConveyor conv = new SendingConveyorBulk(
                //        CreateBulkClient(),
                //        DAL.TransportKind.Email,
                //        idSaver: new TransportIdSaver(),
                //        msgCompiler: new DevinoMsgHtmlCompiler(filen, avatarn, avatard, templ));
                //    //msgCompiler: new SimpleMsgTextFileCompiler(getFileCommand));
                //    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                //    log.Info("Sending messages was stopped");
                //    _cts.Dispose();
                //});
            }
            catch (DbEntityValidationException e)
            {
                log.Fatal("Service exception:", e);
                foreach (var err in e.EntityValidationErrors)
                {
                    var ert = err.ValidationErrors;
                    foreach (var ve in ert)
                    {
                        log.Info(ve.PropertyName + "  " + ve.ErrorMessage);
                    }
                }
            }
            catch (AggregateException e)
            {
                log.Fatal("Service exception:", e);
            }
            catch (Exception e)
            {
                log.Fatal("Service exception:", e);
            }
        }

        protected override void OnStop()
        {
            log.Info("Stop service");
            if (_cts != null)
                _cts.Cancel();
        }

        private IMessageTextCompiler MakeTextCompiller()
        {
            //Dictionary<string, Func<MessageToUser, string>> ReplaceDict
            //    = new Dictionary<string, Func<MessageToUser, string>>()
            //    {
            //        { "ФИО", mtu => mtu.ToTransport.User.FullName ?? "" },
            //        { "город", mtu => mtu.ToTransport.User.City?.Name ?? "" },
            //    };
            //var hrtc = new TagReplaceTextCompiler(ReplaceDict);
            //GetHtmlParams(/*out string filen, out string avatarn, out string avatard, out string templ*/);
            //var dmhc = new DevinoMsgHtmlCompiler(filen, avatarn, avatard, templ);
            //return new ChainCompiler(
            //    new IMessageTextCompiler[]
            //    {
            //        IMessageTextCompilerExtentions.CreateTagTextCompiler(),
            //        //new TagReplaceTextCompiler(ReplaceDict),
            //        new DevinoMsgHtmlCompiler(filen, avatarn, avatard, templ),
            //    });
            return new DevinoMsgHtmlCompiler(filen, avatarn, avatard, templ).UniteWithHashCompiler(html: true);
        }

        private FLChat.Transport.MailSenderBulk CreateBulkClient()
        {
            FLChat.Transport.MailSenderBulk client = new FLChat.Transport.MailSenderBulk();
            return client;
        }

        private FLChat.Transport.DevinoMailSender/*MailSender*/ CreateClient(DevinoSettingsReader dsr)
        {            
            FLChat.Transport.DevinoMailSender client = new FLChat.Transport.DevinoMailSender
                (new DevinoMsgHtmlCompiler("", "", "", subject), subject, settings: dsr );
            return client;
        }       

        private void GetHtmlParams(/*out string filen, out string avatarn, out string avatard, out string templ*/)
        {
            filen = "";
            avatarn = "";
            avatard = "";
            templ = "";
            subject = "";
            //using (ChatEntities entities = new ChatEntities())
            {
                //  Шаблон имени файла
                filen = FileInfo.UrlTemplate;
                //Settings.Values.GetValue("MAINSERVER_NAME", "http://5.188.115.71:8082/FLChat/") +
                //Settings.Values.GetValue("COMMAND_GETFILE", "file/%id%");
                //  Шаблон имени аватара
                avatarn = UserAvatar.UrlTemplate;
                //    Settings.Values.GetValue("MAINSERVER_NAME", "https://rvprj.ru:8443/FLChat/") +
                //Settings.Values.GetValue("COMMAND_AVATAR", "users/%id%/avatar");
                //  Ссылка на картинку для дефолтного аватара
                avatard = Settings.Values.GetValue("AVATAR_DEFAULT", "");
                //  Шаблон текста письма
                templ = Settings.Values.GetValue("EMAIL_DEVINO_HTML_TEMPLATE_1", " %text% ");
                //  Шаблон темы письма
                subject = Settings.Values.GetValue("EMAIL_SUBJECT_TEMPLATE",
                    "Сообщение от личного консультанта Faberlic %sendername%");
            }
        }
    }
}


