using FLChat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.StatusDevino.Client
{
    public class StatusDevinoClient : IMessageStatusPerformer
    {
        private IGetStatusResponses _getStatus;  //  Получение внешнего статуса для списка
        private IMakeStatus _makeStatus;  //  Вычисление внутреннего состояния по внешнему статусу        

        public StatusDevinoClient(IGetStatusResponses getStatus = null, IMakeStatus makeStatus = null)
        {
            _getStatus = getStatus ?? new GetStatusResponses();
            _makeStatus = makeStatus ?? new MakeStatus();
         }

        public async Task<IEnumerable<MessageStats>> Perform(string[] ids, CancellationToken ct)
        {
            //  Получить статус по каждому
            var statsInfo = await _getStatus.Get(ids, ct);
            //  Рассчитать состояние
            MessageStats[] dms = _makeStatus.Make(statsInfo);
            return dms;
        }
    }
}
