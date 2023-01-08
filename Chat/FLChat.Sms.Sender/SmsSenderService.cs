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

namespace FLChat.Sms.Sender
{
    public partial class SmsSenderService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;
        private int delay;

        public SmsSenderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Start service");
            _cts = new CancellationTokenSource();
            try {
                var str = ConfigurationManager.AppSettings["delay_ms"];
                if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out delay))
                    throw new ConfigurationErrorsException("Configuration value delay_ms is invalid "+str);
                //log.Info("delay_ms");
                if (!int.TryParse(ConfigurationManager.AppSettings["sms_sender_kind"] ?? "0", out int kind))
                    throw new ConfigurationErrorsException("Configuration value sms_sender_kind is invalid");
                //log.Info("sms_sender_kind");
                Task.Run(() =>
                {
                    ISendingConveyor conv = GetConveyor(kind);
                    //log.Info("GetConveyor");
                    //new SendingConveyor(
                    //    CreateClient(),
                    //    DAL.TransportKind.Sms,
                    //    idSaver: null);
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("Sending messages was stopped");
                    _cts.Dispose();
                });
            }
            catch (/*Aggregate*/Exception e)
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

        private FLChat.Transport.SmsSender CreateClient()
        {
            FLChat.Transport.SmsSender client = new FLChat.Transport.SmsSender();
            return client;
        }

        private FLChat.Transport.SmsSenderBulk CreateBulkClient()
        {
            FLChat.Transport.SmsSenderBulk client = new FLChat.Transport.SmsSenderBulk();
            return client;
        }

        private ISendingConveyor GetConveyor(int kind)
        {
            ISendingConveyor conv = null;
            switch(kind)
            {
                case 1:
                    conv = new SendingConveyor(
                        CreateClient(),
                        DAL.TransportKind.Sms,
                        idSaver: null);
                    //log.Info("case 1:");
                    break;
                case 0:
                default:
                    conv = new SendingConveyorBulk(
                        CreateBulkClient(),
                        DAL.TransportKind.Sms,
                        idSaver: null);
                    //log.Info("case 0:");
                    break;
            }
            return conv;
        }
    }
}
