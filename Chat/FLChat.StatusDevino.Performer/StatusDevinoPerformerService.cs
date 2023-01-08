using FLChat.Core;
using FLChat.StatusDevino.Client;
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

namespace FLChat.StatusDevino.Performer
{
    public partial class StatusDevinoPerformerService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;
        //private int delay;

        public StatusDevinoPerformerService()
        {
            InitializeComponent();
        }               

        protected override void OnStart(string[] args)
        {
            log.Info("Start service");
            _cts = new CancellationTokenSource();
            try
            {
                //var str = ConfigurationManager.AppSettings["delay_min"];
                //if (!int.TryParse(ConfigurationManager.AppSettings["delay_min"] ?? "1000", out delay))
                //    throw new ConfigurationErrorsException("Configuration value delay_ms is invalid " + str);
                TimeSpan delay = CfgTimeSpan("delay_time", TimeSpan.FromHours(1));
                //log.Info("delay_ms");                
                Task.Run(() =>
                {
                    StatGetConveyor conv = new StatGetConveyor(
                        CreateClient(),
                         msgLoader : new MessageIdsLoader(),
                         statusSaver: new StatusSaver());
                    //log.Info("GetConveyor");
                    
                    conv.OnStartLog += (s, e) => { log.Info($"Begin perform : {e.Count.ToString()} rows started"); };
                    conv.OnEndLog += (s, e) => { log.Info($"End perform : {e.Count.ToString()} rows performed"); };
                    conv.OnStartRowLog += (s, e) => { if (log.IsDebugEnabled) log.Debug($"Part {e.Index.ToString()} : {e.Count.ToString()} rows started"); };
                    conv.OnEndRowLog += (s, e) => { if (log.IsDebugEnabled) log.Debug($"Part {e.Index.ToString()} : {e.Count.ToString()} rows performed"); };
                    conv.OnErrorLog += (s, e) => { log.Error("Exception in performer: ", e); };

                    conv.SendEndlessly(delay, _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
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
         
        private IMessageStatusPerformer CreateClient()
        {
            IMessageStatusPerformer client = new StatusDevinoClient();
            return client;
        }

        private TimeSpan CfgTimeSpan(string name, TimeSpan? defaultValue = null) {
            try {
                string value = ConfigurationManager.AppSettings[name];
                if (value == null && defaultValue.HasValue)
                    return defaultValue.Value;
                switch (value.Last()) {
                    case 'd': return TimeSpan.FromDays(int.Parse(value.Substring(0, value.Length - 1)));
                    case 'h': return TimeSpan.FromHours(int.Parse(value.Substring(0, value.Length - 1)));
                    case 'm': return TimeSpan.FromMinutes(int.Parse(value.Substring(0, value.Length - 1)));
                    case 's': return TimeSpan.FromDays(int.Parse(value.Substring(0, value.Length - 1)));
                    default: return TimeSpan.FromSeconds(int.Parse(value));
                }
            } catch (Exception e) {
                throw new FieldAccessException($"Config value {name}: {e.Message}", e);
            }
        }
    }
}

