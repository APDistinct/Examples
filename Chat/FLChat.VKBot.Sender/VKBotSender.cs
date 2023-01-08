using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
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

namespace FLChat.VKBot.Sender
{
    public partial class VKBotSender : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private CancellationTokenSource _cts;

        public VKBotSender()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("VK - Start service");
            _cts = new CancellationTokenSource();
            try
            {
                if (!int.TryParse(ConfigurationManager.AppSettings["delay_ms"] ?? "1000", out int delay))
                    throw new ConfigurationErrorsException("Configuration value delay_ms is invalid");
                Task.Run(() => {
                    ISendingConveyor conv = new SendingConveyor(
                        CreateClient(),
                        DAL.TransportKind.VK,
                        idSaver: new TransportIdSaver(),
                        msgCompiler: /*new AuthorMsgSimpleTextCompiler()//.UniteWithHashCompiler()*/ MakeTextCompiller()
                        );
                    conv.SendEndlessly(TimeSpan.FromMilliseconds(delay), _cts.Token, (s, e) => log.Error("Exception in conveyor: ", e));
                    log.Info("VK - Sending messages was stopped");
                    _cts.Dispose();
                });

                if (!int.TryParse(ConfigurationManager.AppSettings["get_updates"] ?? "0", out int enable))
                    throw new ConfigurationErrorsException("Configuration value <get_updates> is invalid");
                if (!int.TryParse(ConfigurationManager.AppSettings["get_updates_delay"] ?? "0", out int get_updates_delay))
                    throw new ConfigurationErrorsException("Configuration value <get_updates_delay> is invalid");
                //  Чтение номера группы int groupId = 179649792; и передача в updater
                if (enable != 0)
                {
                    Task.Run(async () =>
                    {
                        VKUpdateRequester updater = new VKUpdateRequester(CreateClient(), new VKUpdateHandler())
                        {
                            DelayTimeMs = get_updates_delay
                        };
                        int offset = await updater.ReceiveUpdates(0, _cts.Token, (s, e) => log.Error("Exception in update receiver: ", e));
                        log.Info("Update receiver was stopped");
                    });
                }
            }
            catch (AggregateException e)
            {
                log.Fatal("Service exception:", e);
            }
        }

        protected override void OnStop()
        {
            log.Info("VK - Stop service");
            if (_cts != null)
                _cts.Cancel();
        }

        private VKClient CreateClient()
        {
            //  Нужен
            string token = ConfigurationManager.AppSettings["token"] ?? throw new ConfigurationErrorsException("Configuration value for VK token must be present");
                        
            VKClient client = new VKClient(token);
            client.Log.OnLogError += (s, e) => {
                log.Error("Write transport log error: ", e);
            };
            return client;
        }

        private IMessageTextCompiler MakeTextCompiller()
        {
            ////  Шаблон имени файла
            //string filen = Settings.Values.GetValue("MAINSERVER_NAME", "https://rvprj.ru:8443/FLChat/") +
            //Settings.Values.GetValue("COMMAND_GETFILE", "file/%id%");
            ////  Шаблон ссылки файла
            //string templ = Settings.Values.GetValue("VK_FILELINK_TEMPLATE", "https://vk.com/away.php?to=#FileLink&cc_key=#FileText");
            
            IMessageTextCompiler compiller = new AuthorMsgSimpleTextCompiler().UniteWithHashCompiler()/*.Add(new FileMsgTextCompiler(templ, filen))*/;
            return compiller;
        }
    }
}
