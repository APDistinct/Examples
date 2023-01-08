using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Routers
{
    public static class RouterFactory
    {
        public static ChainRouter CreateDefaultRouters() {
            return CreateDefaultRouters(Enumerable.Empty<IMessageRouter>());
        }

        public static ChainRouter CreateDefaultRouters(IMessageRouter router) {
            return CreateDefaultRouters(Enumerable.Repeat(router, 1));
        }

        public static ChainRouter CreateDefaultRouters(IEnumerable<IMessageRouter> routers) {
            //routers from first to last
            return new ChainRouter(routers.Concat(
                new IMessageRouter[] {
                    //new DeepLinkToSystemBotRouter(),
                    //new AnswerRouter(),
                    new ReplyRouter(),
                    new MsgAddresseeRouter(),
                    new OwnerRouter(),
                    new NearestParentRouter(),
                    new SentryRouter(),
                    new RejectMessageRouter(DAL.Model.Settings.Values.GetValue("TEXT_REJECT_MESSAGE", "Добрый день. Произошла ошибка. Ваше сообщение не будет отправлено и прочитано"))
            }));
        }
    }
}
