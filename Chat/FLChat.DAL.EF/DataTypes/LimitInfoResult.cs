using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.DataTypes
{
    public class LimitInfoResult
    {
        public int SelectionCount { get; set; }
        public bool ExceedDay { get; set; }
        public int? DayLimit { get; set; }
        public int? SentOverToday { get; set; }
        public bool ExceedOnce { get; set; }
        public int? OnceLimit { get; set; }
    }
}
