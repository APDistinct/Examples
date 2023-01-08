using Devino.Viber;
using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public class DataPerformer
    {
        private IIdsReader _idsReader;  //  Получение списка номеров для запроса состояния
        private IGetStatusResponses _getStatus;  //  Получение внешнего статуса для списка
        private IMakeStatus _makeStatus;  //  Вычисление внутреннего состояния по внешнему статусу
        private IDevinoStatsSaver _statsSaver;  //  Сохранение состояния всего списка

        public DataPerformer(IIdsReader idsReader = null, IGetStatusResponses getStatus = null, IDevinoStatsSaver statsSaver = null, IMakeStatus makeStatus = null)
        {
            _idsReader = idsReader ?? new IdsReader();
            _getStatus = getStatus ?? new GetStatusResponses();
            _makeStatus = makeStatus ?? new MakeStatus();
            _statsSaver = statsSaver ?? new DevinoStatsSaver();
        }

        public void Perform(Guid messageId)
        {
            using (ChatEntities entities = new ChatEntities())
            {
                //var list = MessageStatsReader.GetMessageSentStats(entities, messageId);
                //  Получить список идентификаторов
                var stats = _idsReader.GetIds(messageId, entities).ToArray();
                //  Получить статус по каждому
                var statsInfo = _getStatus.Get(stats);
                //  Рассчитать состояние
                DevinoMessageStatus[] dms = _makeStatus.Make(statsInfo);
                // Сохранить полученное
                _statsSaver.Save(dms, entities);
            }
        }

        //DevinoMessageStatus[]  MakeStatus(List<StatusResponse> statsInfo)
        //{
        //    List<DevinoMessageStatus> list = new List<DevinoMessageStatus>();
        //    foreach(var status in statsInfo)
        //    {
        //        DevinoMessageStatus dms = new DevinoMessageStatus()
        //        {
        //            TransportId = status.ProviderId,
        //            SentTo = (status.SmsStates?.Count ?? 0) > 0 ? (int)TransportKind.Sms : (int)TransportKind.Viber,
        //            ViberStatus = (int?)status.Status,
        //            SmsStatus = (int?)status.SmsStates?[0].Status,
        //            UpdatedTime = DateTime.UtcNow,
        //            //WebFormRequested = status.Status == ViberStatus.Visited,
        //        };
        //        list.Add(dms);
        //    }
        //    return list.ToArray();
        //}
    }
}
