using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public interface IDevinoStatsSaver
    {
        void Save(DevinoMessageStatus[] dms, ChatEntities entities);
    } 

    public class DevinoStatsSaver : IDevinoStatsSaver
{
        public void Save(DevinoMessageStatus[] dms, ChatEntities entities)
        {
            foreach(var dm in dms)
            {
                try
                {
                    var mess = entities.MessageTransportId.Where(x => x.TransportId == dm.TransportId && x.TransportTypeId == (int)TransportKind.WebChat)
                        .Include(m => m.MessageToUser.WebChatDeepLink).FirstOrDefault();
                    if (mess != null)
                    {
                        var wcdl = //entities.WebChatDeepLink.Where(x => x.MsgId == mess.MsgId && x.ToUserId == mess.ToUserId).FirstOrDefault();
                          mess.MessageToUser.WebChatDeepLink?.FirstOrDefault();
                        //mess?.TransportType?.WebChatDeepLink?.FirstOrDefault();
                        if (wcdl != null)
                        {
                            if (dm.Update)
                            {
                                wcdl.SentTo = dm.SentTo;
                                //if(wcdl.SmsStatus < dm.SmsStatus)
                                //    wcdl.SmsStatus = dm.SmsStatus;
                                wcdl.SmsStatus = Replace(wcdl.SmsStatus, dm.SmsStatus);
                                //if (wcdl.ViberStatus < dm.ViberStatus)
                                //    wcdl.ViberStatus = dm.ViberStatus;
                                wcdl.ViberStatus = Replace(wcdl.ViberStatus, dm.ViberStatus);
                                wcdl.UpdatedTime = dm.UpdatedTime;
                            }
                            //wcdl.WebFormRequested = dm.WebFormRequested;
                            wcdl.IsFinished = dm.IsFinished;
                            entities.SaveChanges();
                        }
                    }
                }
                catch
                { }
            }
        }

        public int? Replace(int? oldV, int? newV)
        {
            var ret = oldV;
            if (newV != null && (oldV == null || oldV.Value < newV.Value))
                ret = newV;
            return ret;
        }

    }
}
