using Devino;
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

namespace FLChat.SmsDevino.Sender
{
    public partial class SmsDevinoSenderService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;
        private int delay;

        public SmsDevinoSenderService()
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
                DevinoSettingsReader dsr = new DevinoSettingsReader();
                dsr.BaseInit();
                //log.Info("delay_ms");                
                Task.Run(() =>
                {
                    //ISendingConveyor conv = new SendingConveyorBulk(
                    //    CreateBulkClient(),
                    //    DAL.TransportKind.Sms,
                    //    idSaver: new TransportIdSaver());
                    ISendingConveyor conv = new SendingConveyor(
                        CreateClient(dsr),
                        DAL.TransportKind.Sms,
                        idSaver: new TransportIdSaver(),
                        waiting: false);
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

        private FLChat.Transport.SmsSenderBulk CreateBulkClient(DevinoSettingsReader dsr)
        {
            FLChat.Transport.SmsSenderBulk client = new FLChat.Transport.SmsSenderBulk();
            return client;
        }

        private IMessageSender CreateClient(DevinoSettingsReader dsr)
        {
            FLChat.Transport.DevinoSmsSender client = new FLChat.Transport.DevinoSmsSender(settings: dsr);
            return client;
        }
    }
}
