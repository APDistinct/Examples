using Devino.Viber;
using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public interface IMakeStatus
    {
         DevinoMessageStatus[] Make(List<StatusResponse> statsInfo);
    }

    public class MakeStatus : IMakeStatus
    {
        private readonly string[] errors = 
            {
            "error-address-not-specified",
            "error-instant-message-provider-id-unknown"
        };

        public DevinoMessageStatus[] Make(List<StatusResponse> statsInfo)
        {
            List<DevinoMessageStatus> list = new List<DevinoMessageStatus>();
            foreach (var status in statsInfo)
            {
                //if( status.Code)
                DevinoMessageStatus dms = new DevinoMessageStatus()
                {
                    TransportId = status.ProviderId,
                    //WebFormRequested = status.Status == ViberStatus.Visited,
                };
                var ret = !errors.Contains(status.Code);
                dms.Update = ret;
                dms.IsFinished = !ret;
                dms.UpdatedTime = DateTime.UtcNow;
                dms.SentTo = (status.SmsStates?.Count ?? 0) > 0 ? (int)TransportKind.Sms : (int)TransportKind.Viber;
                if (ret)
                {
                    dms.ViberStatus = (int?)status.Status;
                    dms.SmsStatus = (int?)status.SmsStates?.Max(x => (int?)x.Status);
                    dms.IsFinished = dms.IsFinished || status.Status == ViberStatus.Visited || status.Status == ViberStatus.Unknown
                        || dms.SmsStatus == (int)SmsStatus.Undelivered || dms.SmsStatus == (int)SmsStatus.Delivered;
                }
                list.Add(dms);
            }
            return list.ToArray();
        }
    }
}
