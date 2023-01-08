using FLChat.Core;
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

namespace FLChat.Email.Sender
{
    public partial class EmailSenderService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;

        public EmailSenderService()
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
                Task.Run(() => {
                    ISendingConveyor conv = new SendingConveyor(
                        CreateClient(),
                        DAL.TransportKind.Email,
                        idSaver: null);
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("Sending messages was stopped");
                    _cts.Dispose();
                });
            }
            catch (AggregateException e)
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

        private FLChat.Transport.MailSender CreateClient()
        {
            FLChat.Transport.MailSender client = new FLChat.Transport.MailSender();
            return client;
        }
        
    }
}
