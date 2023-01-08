using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Reports
{
    public class CommonInfo
    {
        public Guid UserId { get; set; }        // Отправитель
        public string FullName { get; set; }    //
        public string Phone { get; set; }       //
        public Guid MsgId { get; set; }         // Сообщение
        public int MessageTypeId { get; set; }  // 
        public string Text { get; set; }        //
        public DateTime PostTm { get; set; }    //
    }
}
