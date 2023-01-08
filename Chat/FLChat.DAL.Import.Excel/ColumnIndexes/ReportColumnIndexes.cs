using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Excel.ColumnIndexes
{
    public class ReportColumnIndexes : IColumnIndexes
    {
        public int FLUserNumber => 3;
        public int? ParentFLUserNumber => 5;

        public int? Surname => 4;
        public int? Name => null;
        public int? Partronymic => null;

        public int? Birthday => null;

        public int? Phone => 10;
        public int? Email => 11;

        public int? Title => null;

        public int? ZipCode => null;

        public int? Country => null;

        public int? Region => null;

        public int? City => null;

        public int? RegDate => 7;

        public int? SmsPermission => null;

        public int? EmailPermission => null;

        public int? IsDirector => null;

        public int? LastOrder => null;

        public int? LoScores => 12;

        public int? PeriodWoLo => 15;

        public int? OlgScores => 13;

        public int? GoScores => 14;

        public int? CashbackBalance => null;

        public int? FLClubPoints => null;

        public int? FLClubPointsBurn => null;
    }
}
