using Devino.Viber;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Reports
{
    public class ReportStat1
    {
        List<CommonInfo> commonInfo = new List<CommonInfo>();

        //public Guid UserId { get; set; }        // Отправитель
        //public string FullName { get; set; }    //
        //public string Phone { get; set; }       //
        //public Guid MsgId { get; set; }         // Сообщение
        //public int MessageTypeId { get; set; }  // 
        //public string Text { get; set; }        //
        //public DateTime PostTm { get; set; }    //
        public int AllSentNum { get; set; } // Количество получателей

        public int ViberSentNum { get; set; } // Из них отправлено в Вайбер

        //  Статусы СМС
        private Dictionary<SmsStatus, int> SmsStats = new Dictionary<SmsStatus, int>();

        //public int SmsEnqueued { get; set; }    // enqueued – сообщение находится в очереди на отправку.
        //public int SmsSent { get; set; }        // sent – сообщение отправлено абоненту 
        //public int SmsDelivered { get; set; }   // delivered – сообщение доставлено абоненту.
        //public int SmsUndelivered { get; set; } // undelivered – сообщение отправлено, но не доставлено абоненту.
        //  Статусы вайбер        
        private Dictionary<ViberStatus, int> ViberStats = new Dictionary<ViberStatus, int>();
        //public int ViberEnqueued { get; set; }    // enqueued – сообщение находится в очереди на отправку.
        //public int ViberSent { get; set; }        // sent – сообщение отправлено абоненту 
        //public int ViberDelivered { get; set; }   // delivered – сообщение доставлено абоненту.
        //public int ViberRead { get; set; }        // read – сообщение просмотрено абонентом. 
        //public int ViberVisited { get; set; }     // visited абонент перешел по ссылке в сообщении. 
        //public int ViberUndelivered { get; set; } // undelivered – сообщение отправлено, но не доставлено абоненту.
        //public int ViberFailed { get; set; }      // failed – сообщение не было отправлено в результат сбоя. 
        //public int ViberCancelled { get; set; }   // cancelled – отправка сообщения отменена.
        //public int ViberVp_expired { get; set; }  // vp_expired – сообщение просрочено, финальный статус не получен в рамках заданного validity period


        public ReportStat1()
        {
            InitCounters();
        }

        private void InitCounters()
        {
            commonInfo.Clear();
            AllSentNum = 0;
            ViberSentNum = 0;
            foreach (SmsStatus st in (SmsStatus[]) Enum.GetValues(typeof(SmsStatus)))
            {
                SmsStats[st] = 0;
            }

            foreach (ViberStatus st in (ViberStatus[]) Enum.GetValues(typeof(ViberStatus)))
            {
                ViberStats[st] = 0;
            }

            //SmsEnqueued = 0;
            //SmsSent = 0;
            //SmsDelivered = 0;
            //SmsUndelivered = 0;
            //ViberEnqueued = 0;
            //ViberSent = 0;
            //ViberDelivered = 0;
            //ViberRead = 0;
            //ViberVisited = 0;
            //ViberUndelivered = 0;
            //ViberFailed = 0;
            //ViberCancelled = 0;
            //ViberVp_expired = 0;
        }

        public bool WriteReport(string writePath, bool append = false)
        {
            bool ret = true;
            using (StreamWriter sw = new StreamWriter(writePath, append, System.Text.Encoding.Default))
            {
                sw.WriteLine($" {DateTime.Now} ");
                foreach (var info in commonInfo)
                {
                    //string str = $"{stat.FullName};{stat.Phone};{stat.IsFailed};";
                    sw.WriteLine($"Отправитель  {info.FullName}");
                    sw.WriteLine($"Телефон  {info.Phone}");
                    sw.WriteLine($"Сообщение:  {info.Text}");
                    sw.WriteLine($"Время отправки  {info.PostTm.ToString()}");
                    sw.WriteLine();
                }

                sw.WriteLine($"Всего отправлено  {AllSentNum} ");
                sw.WriteLine($"Из них в Viber  {ViberSentNum} ");

                sw.WriteLine("Viber");
                foreach (ViberStatus st in (ViberStatus[]) Enum.GetValues(typeof(ViberStatus)))
                {
                    sw.WriteLine($"{st.ToString()};{ViberStats[st]};");
                }

                sw.WriteLine("SMS");
                foreach (SmsStatus st in (SmsStatus[]) Enum.GetValues(typeof(SmsStatus)))
                {
                    string str = $"{st.ToString()};{SmsStats[st]};";
                    sw.WriteLine(str);
                }
            }

            return ret;
        }

        public bool MakeReport(MessageSentStats[] list, Dictionary<string, GetStatusResponse> dic, CommonInfo info,
            bool append = false)
        {
            bool ret = true;
            if (!append)
            {
                InitCounters();
            }

            commonInfo.Add(info);
            var stats = list.Where(z => z.ToTransportTypeId == 100 && z.TransportId != null).ToArray();
            AllSentNum += list.Count();
            ViberSentNum += stats.Count();

            //  По идее, можно переделать в запрос с разделением по значениям, но это попозже. В данный момент так нагляднее
            foreach (var stat in dic)
            {
                var val = stat.Value;
                foreach (SmsStatus st in (SmsStatus[]) Enum.GetValues(typeof(SmsStatus)))
                {
                    if (val.Messages?.ToArray()[0]?.SmsStates != null &&
                        val.Messages.ToArray()[0].SmsStates.Count() > 0)
                    {
                        SmsStats[st] += (val.Messages.ToArray())[0].SmsStates.ToArray()[0].Status == st ? 1 : 0;
                    }
                }

                foreach (ViberStatus st in (ViberStatus[]) Enum.GetValues(typeof(ViberStatus)))
                {
                    if (val.Messages?.ToArray()[0] != null)
                    {
                        ViberStats[st] += (val.Messages.ToArray())[0].Status == st ? 1 : 0;
                    }
                }

            }

            return ret;
        }

        public bool MakeReport(MessageSentStats[] list, Dictionary<string, StatusResponse> dic, CommonInfo info,
            bool append = false)
        {
            bool ret = true;
            if (!append)
            {
                InitCounters();
            }

            commonInfo.Add(info);
            var stats = list.Where(z => z.ToTransportTypeId == 100 && z.TransportId != null).ToArray();
            AllSentNum += list.Count();
            ViberSentNum += stats.Count();

            //  По идее, можно переделать в запрос с разделением по значениям, но это попозже. В данный момент так нагляднее
            foreach (var stat in dic)
            {
                var val = stat.Value;
                foreach (SmsStatus st in (SmsStatus[]) Enum.GetValues(typeof(SmsStatus)))
                {
                    if (val.SmsStates != null && val.SmsStates.Any())
                    {
                        SmsStats[st] += val.SmsStates.FirstOrDefault()?.Status == st ? 1 : 0;
                    }
                }

                foreach (ViberStatus st in (ViberStatus[]) Enum.GetValues(typeof(ViberStatus)))
                {
                    ViberStats[st] += val.Status == st ? 1 : 0;
                }

            }

            return ret;
        }
    }
}
