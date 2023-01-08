using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Excel
{
    public interface IColumnIndexes
    {
        int FLUserNumber { get; }
        int? ParentFLUserNumber { get; }
        int? Surname { get; }
        int? Name { get; }
        int? Partronymic { get; }
        int? Birthday { get; }
        int? Phone { get; }
        int? Email { get; }
        int? Title { get; }
        int? ZipCode { get; }
        int? Country { get; }
        int? Region { get; }
        int? City { get; }
        int? RegDate { get; }
        int? SmsPermission { get; }
        int? EmailPermission { get; }
        int? IsDirector { get; }
        int? LastOrder { get; }
        int? LoScores { get; }
        int? PeriodWoLo { get; }
        int? OlgScores { get; }
        int? GoScores { get; }
        int? CashbackBalance { get; }
        int? FLClubPoints { get; }
        int? FLClubPointsBurn { get; }
    }
}
