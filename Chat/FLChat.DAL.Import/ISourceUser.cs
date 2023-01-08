using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    public interface IUserInfo
    {
        int FLUserNumber { get; }
        int? ParentFLUserNumber { get; }
        string Surname { get; }
        string Name { get; }
        string Partronymic { get; }
        DateTime? Birthday { get; }
        string Phone { get; }
        string Email { get; }
        string Title { get; }
        string ZipCode { get; }
        string Country { get; }
        string Region { get; }
        string City { get; }
        DateTime? RegDate { get; } 
        bool? SmsPermission { get; }
        bool? EmailPermission { get; }
        bool? IsDirector { get; }
        DateTime? LastOrder { get; }
        decimal? LoScores { get; }
        int? PeriodWoLo { get; }
        decimal? OlgScores { get; }
        decimal? GoScores { get; }
        decimal? CashbackBalance { get; }
        decimal? FLClubPoints { get; }
        decimal? FLClubPointsBurn { get; }
        bool? IsDeleted { get; }
    }

    public interface IImportSource<T> : IEnumerable<T> where T : class
    {
        int TopUserNumber { get; }
    }
}
