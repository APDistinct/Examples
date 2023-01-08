using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class User
    {
        public User()
        {
            LocationHistories = new HashSet<LocationHistory>();
            OutTimeReportInfos = new HashSet<OutTimeReportInfo>();
            RoleMembers = new HashSet<RoleMember>();
            TimeReportHistoryUpdateUsers = new HashSet<TimeReportHistory>();
            TimeReportHistoryUsers = new HashSet<TimeReportHistory>();
            TimeReports = new HashSet<TimeReport>();
            UserBusinessLines = new HashSet<UserBusinessLine>();
            UserDepartments = new HashSet<UserDepartment>();
            WorkItemStateHistories = new HashSet<WorkItemStateHistory>();
        }

        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public Guid PositionId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid LocationId { get; set; }
        public DateTime LocationFrom { get; set; }
        public string Sid { get; set; } = null!;
        public bool Deleted { get; set; }

        public virtual Country? Country { get; set; }
        public virtual Location Location { get; set; } = null!;
        public virtual Position Position { get; set; } = null!;
        public virtual OutUser OutUser { get; set; } = null!;
        public virtual Timetable Timetable { get; set; } = null!;
        public virtual UserInfo UserInfo { get; set; } = null!;
        public virtual ICollection<LocationHistory> LocationHistories { get; set; }
        public virtual ICollection<OutTimeReportInfo> OutTimeReportInfos { get; set; }
        public virtual ICollection<RoleMember> RoleMembers { get; set; }
        public virtual ICollection<TimeReportHistory> TimeReportHistoryUpdateUsers { get; set; }
        public virtual ICollection<TimeReportHistory> TimeReportHistoryUsers { get; set; }
        public virtual ICollection<TimeReport> TimeReports { get; set; }
        public virtual ICollection<UserBusinessLine> UserBusinessLines { get; set; }
        public virtual ICollection<UserDepartment> UserDepartments { get; set; }
        public virtual ICollection<WorkItemStateHistory> WorkItemStateHistories { get; set; }
    }
}
