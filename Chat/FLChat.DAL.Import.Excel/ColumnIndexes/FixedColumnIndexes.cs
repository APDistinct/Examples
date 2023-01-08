using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Excel.ColumnIndexes
{
    public class FixedColumnIndexes : IColumnIndexes
    {
        public int FLUserNumber => 34;
        public int? ParentFLUserNumber => 35;
        public int? Surname => 3;
        public int? Name => 4;
        public int? Partronymic => 5;
        public int? Birthday => 6;
        public int? Phone => 7;
        public int? Email => 8;
        public int? Title => 9;
        public int? ZipCode => 10;
        public int? Country => 11;
        public int? Region => 12;
        public int? City => 13;
        public int? RegDate => 14;
        public int? EmailPermission => 15;
        public int? SmsPermission => 16;
        public int? IsDirector => 17;
        public int? LastOrder => 18;
        public int? LoScores => 25;
        public int? PeriodWoLo => 26;
        public int? OlgScores => 27;
        public int? GoScores => 28;
        public int? CashbackBalance => 29;
        public int? FLClubPoints => 30;
        public int? FLClubPointsBurn => 31;
    }
}
