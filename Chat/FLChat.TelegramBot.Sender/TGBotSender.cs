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
using log4net;

namespace FLChat.TelegramBot.Sender
{
    public partial class TGBotSender : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;

        public TGBotSender() {
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
                        DAL.TransportKind.Telegram,
                        idSaver: new TransportIdSaver(),
                        msgCompiler: new AuthorMsgTextCompiler().UniteWithHashCompiler());
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("Sending messages was stopped");
                    _cts.Dispose();
                });

                if (!int.TryParse(ConfigurationManager.AppSettings["get_updates"] ?? "0", out int enable))
                    throw new ConfigurationErrorsException("Configuration value <get_updates> is invalid");
                if (!int.TryParse(ConfigurationManager.AppSettings["get_updates_delay"] ?? "0", out int get_updates_delay))
                    throw new ConfigurationErrorsException("Configuration value <get_updates_delay> is invalid");
                if (enable != 0) {
                    Task.Run(async () => {
                        TelegramUpdateRequester updater = new TelegramUpdateRequester(CreateClient(), new TelegramUpdateHandler()) {
                            DelayTimeMs = get_updates_delay
                        };
                        int offset = await updater.ReceiveUpdates(0, _cts.Token, (s, e) => log.Error("Exception in update receiver: ", e));
                        log.Info("Update receiver was stopped");
                    });
                }
            } catch (AggregateException e) {
                log.Fatal("Service exception:", e);
            }
        }

        protected override void OnStop() {
            log.Info("Stop service");
            if (_cts != null)
                _cts.Cancel();
        }

        private TelegramClient CreateClient() {
            string token = ConfigurationManager.AppSettings["token"] ?? throw new ConfigurationErrorsException("Configuration value for telegram token must be present");
            string proxy = ConfigurationManager.AppSettings["proxy_addr"];
            if (!int.TryParse(ConfigurationManager.AppSettings["proxy_port"] ?? "0", out int port))
                throw new ConfigurationErrorsException("Configuration value for proxy port is invalid");
            string usr = ConfigurationManager.AppSettings["proxy_user"];
            string psw = ConfigurationManager.AppSettings["proxy_password"];
            TelegramClient client = new TelegramClient(token, proxy, port, usr, psw);
            client.Log.OnLogError += (s, e) => {
                log.Error("Write transport log error: ", e);
            };
            return client;
        }
    }
}
