using FLChat.Core;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FLChat.Logger;

namespace FLChat.VKBot.Routers
{
    public class VKInviteRouter : IMessageRouter
    {
        private readonly VKClient _vkClient;
        private readonly IMessageRouter _router;
        private FileLogger _logger;

        public VKInviteRouter(IMessageRouter router = null, VKClient vkClient = null)
        {
            _router = router;
            _vkClient = vkClient;
            string fname = "C:\\FLChat\\Log.txt";
            _logger = new FileLogger(fname);
            //_logger = new FileLogger();
        }

        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg)
        {
            _logger.Log("Start RouteMessage");
            var scp = entities.ScenarioProcess.Where(x => /*x.ScenarioId == scNum && */x.UserId == dbmsg.FromUserId).FirstOrDefault();
            _logger.LogObject("scp", scp);
            if (scp != null)
            {
                _logger.LogObject("_vkClient", _vkClient);
                _logger.LogObject("_router", _router);
                if (_vkClient != null && _router != null)
                {
                    try
                    {
                        _logger.LogObject("message", message);

                        VKMessageAdapter newmess = message as VKMessageAdapter;

                        _logger.LogObject("newmess", newmess);
                        if (newmess != null)
                        {
                            _logger.Log("Start sending GetUserInfoAsync");
                            // Добыть информацию по отправителю
                            var task = _vkClient.Client.GetUserInfoAsync(message.FromId, CancellationToken.None);
                            task.Wait();
                            var user = task.Result.User.FirstOrDefault();
                            _logger.LogObject("User", user);
                            // Записать полученные данные в message
                            newmess.PhoneNumber = user?.MobilePhone;
                            newmess.FromName = user?.FirstName + " " + user?.LastName;
                            _logger.LogObject("newmess", newmess);
                            // Далее выполнить тот, для которого это всё добывалось
                            return _router?.RouteMessage(entities, newmess, dbmsg);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Log($"Exception: {e.Message}");
                    }
                }
            }
            _logger.Log("End RouteMessage");
            return _router?.RouteMessage(entities, message, dbmsg);
        }
    }
}
