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

using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.Viber.Bot;

namespace FLChat.Viber.Sender
{
    public partial class ViberBotSender : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;

        public ViberBotSender() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            log.Info("Start service");
            _cts = new CancellationTokenSource();
            try {
                if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out int delay))
                    throw new ConfigurationErrorsException("Configuration value delay_ms is invalid");
                Task.Run(() => {
                    ISendingConveyor conv = new SendingConveyor(
                    //new SendingConveyor(
                        CreateClient(),
                        DAL.TransportKind.Viber,
                        idSaver: new TransportIdSaver(),
                        //msgCompiler: new SimpleMsgTextCompiler().UniteWithHashCompiler());
                        msgCompiler: ViberFactory.CreateCompiler());
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("Sending messages was stopped");
                    _cts.Dispose();
                });

            } catch (AggregateException e) {
                log.Fatal("Service exception:", e);
            }
}

        protected override void OnStop() {
            log.Info("Stop service");
            if (_cts != null)
                _cts.Cancel();
        }

        private ViberSender CreateClient() {
            string token = ConfigurationManager.AppSettings["token"] ?? throw new ConfigurationErrorsException("Configuration value for viber token must be present");
            ViberSender client = new ViberSender(token);
            client.Log.OnLogError += (s, e) => {
                log.Error("Write transport log error: ", e);
            };
            return client;
        }
    }
}
