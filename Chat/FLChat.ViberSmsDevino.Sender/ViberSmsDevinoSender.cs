using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Transport;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.ViberSmsDevino.Sender
{
    //public static class ViberSmsChecker
    //{
    //    public static bool Check(MessageToUser mtu)
    //    {
    //        string specific = "WEBCHAT";
    //        //  ForwardMsgId не пустое и в Specific содержится строка WEBCHAT
    //        if (mtu.Message.Specific == null)
    //            return false;
    //        return (mtu.Message.Specific.Contains(specific) && mtu.Message.ForwardMsgId != null);
    //        //return ((mtu.Message.Specific?.Contains(specific) ?? false) && mtu.Message.ForwardMsgId != null);
    //    }
    //}

    public partial class ViberSmsDevinoSender : ServiceBase
    {
       
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;
        private int delay;
        private int _patternNum;

        public ViberSmsDevinoSender()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Start service");
            _cts = new CancellationTokenSource();
            try
            {
                var str = ConfigurationManager.AppSettings["delay_ms"];
                if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out delay))
                    throw new ConfigurationErrorsException("Configuration value delay_ms is invalid " + str);
                str = ConfigurationManager.AppSettings["url_num"];
                if (!int.TryParse(str ?? "0", out _patternNum))
                    throw new ConfigurationErrorsException("Configuration value pattern_num is invalid " + str);
                
                //log.Info("delay_ms");                
                Task.Run(() =>
                {
                    ISendingConveyor conv = new SendingConveyor(
                        CreateClient(),
                        DAL.TransportKind.Sms,
                        idSaver: new TransportIdSaver(),
                        msgCompiler: MakeTextCompiller());
                    //log.Info("GetConveyor");
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("Sending messages was stopped");
                    _cts.Dispose();
                });
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

        private IMessageSender CreateClient()
        {
            var clientCreater = new ClientCreater();
            return clientCreater.CreateClient(_patternNum);
        }
        //    // Формирование ссылки по СМС-сообщению - на страницу выбора транспорта
        //    //string _pattern = Settings.Values.GetValue("WEB_CHAT_DEEP_URL", "https://chat.faberlic.com/external/%code%");
        //    //int pos = _pattern.IndexOf('%');
        //    //if (pos < 0)
        //    //    pos = _pattern.Length - 1;
        //    //_pattern = _pattern.Substring(0, pos);
        //    //pos = _pattern.Length - 1;
        //    //if(_pattern[pos] == '/')
        //    //    _pattern = _pattern.Substring(0, pos);

        //    // Формирование ссылки по WebChatDeepLink с переадрессацией в Viber-bot
        //    string _pattern = GetPatternLink(TransportKind.Viber);

        //    CombiSender client = new CombiSender(
        //        new DevinoViberSender(_pattern, ChangeParamTextCompiler.GetForwardMsg), 
        //        new DevinoSmsSender(),
        //        ViberSmsChecker.Check);
        //    return client;
        //}

        private IMessageTextCompiler MakeTextCompiller()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
               = new Dictionary<string, Func<MessageToUser, string>>()
               {
                    { "sendername", mtu => mtu.Message.FromTransport.User.FullName ?? "" },
               };
            //new TagReplaceTextCompiler(ReplaceDict)
            //    userFrom = mtu.FromTransport.User
            string _pattern = Settings.Values.GetValue("WEB_CHAT_VIBER",
                    "#sendername, ваш личный консультант Faberlic, отправил вам сообщение:");            
            CombiCompiler compiler = new CombiCompiler(
               new DevinoViberSmsCompiler(_pattern)
               .Add(new ChangeParamTextCompiler(
                   new TagReplaceTextCompiler(ReplaceDict, true).AddStandartHashCompiler(),
                   ChangeParamTextCompiler.GetForwardMsg //(u => u.Message.ForwardMsg.ToUser)
                   )),
               new SimpleMsgTextCompiler(),
               ViberSmsChecker.Check
               );

            return compiler;
        }

        private string GetPatternLink(TransportKind kind)
        {
            string ret = null;
            using (ChatEntities entities = new ChatEntities())
            {
                ret = entities.TransportType.Where(t => t.Id == (int)kind).Select(x => x.DeepLink).FirstOrDefault();
            }
            return ret;
        }
    }
}
