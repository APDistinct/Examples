using Devino;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
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

namespace FLChat.ViberDevino.Sender
{
    public partial class ViberDevinoSenderService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;
        private int delay;
        private int _patternNum;

        public ViberDevinoSenderService()
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
                DevinoSettingsReader dsr = new DevinoSettingsReader();
                dsr.BaseInit();
                //log.Info("delay_ms");                
                Task.Run(() =>
                {
                    ISendingConveyor conv = new SendingConveyor(
                        CreateClient(dsr),
                        DAL.TransportKind.WebChat,
                        idSaver: new TransportIdSaver(),
                        msgCompiler: MakeTextCompiller(),
                        waiting: false);
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

        private IMessageTextCompiler MakeTextCompiller()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
               = new Dictionary<string, Func<MessageToUser, string>>()
               {
                    { "sendername", mtu => mtu.Message.FromTransport.User.FullName ?? "" },
               };
            string _pattern = Settings.Values.GetValue("WEB_CHAT_VIBER",
                    "#sendername, ваш личный консультант Faberlic, отправил вам сообщение:");
            IMessageTextCompiler compiler = 
               new DevinoViberCompiler(_pattern)
               .Add(new TagReplaceTextCompiler(ReplaceDict, true).AddStandartHashCompiler());

            return compiler;
        }

        private IMessageSender CreateClient(DevinoSettingsReader dsr)
        {
            //var clientCreater = new ClientCreater();
            return new DevinoViberSenderNew(_patternNum, settings: dsr);
                //clientCreater.CreateViberClient(_patternNum);
        }
    }
}
