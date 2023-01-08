using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Chrono.DAL.EF.Model
{
    public partial class ChronoContext : DbContext
    {
        public ChronoContext()
        {
        }

        public ChronoContext(DbContextOptions<ChronoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<BusinessLine> BusinessLines { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<DefaultTimetable> DefaultTimetables { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<LocationHistory> LocationHistories { get; set; } = null!;
        public virtual DbSet<OutProjectInfoPattern> OutProjectInfoPatterns { get; set; } = null!;
        public virtual DbSet<OutTimeReportDatum> OutTimeReportData { get; set; } = null!;
        public virtual DbSet<OutTimeReportInfo> OutTimeReportInfos { get; set; } = null!;
        public virtual DbSet<OutUser> OutUsers { get; set; } = null!;
        public virtual DbSet<OutWorkItem> OutWorkItems { get; set; } = null!;
        public virtual DbSet<Pmsystem> Pmsystems { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectCollection> ProjectCollections { get; set; } = null!;
        public virtual DbSet<ProjectRole> ProjectRoles { get; set; } = null!;
        public virtual DbSet<ReportStatus> ReportStatuses { get; set; } = null!;
        public virtual DbSet<RoleMember> RoleMembers { get; set; } = null!;
        public virtual DbSet<SentForApproveLog> SentForApproveLogs { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<TimeReport> TimeReports { get; set; } = null!;
        public virtual DbSet<TimeReportHistory> TimeReportHistories { get; set; } = null!;
        public virtual DbSet<Timetable> Timetables { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBusinessLine> UserBusinessLines { get; set; } = null!;
        public virtual DbSet<UserDepartment> UserDepartments { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;
        public virtual DbSet<WorkItem> WorkItems { get; set; } = null!;
        public virtual DbSet<WorkItemState> WorkItemStates { get; set; } = null!;
        public virtual DbSet<WorkItemStateHistory> WorkItemStateHistories { get; set; } = null!;
        public virtual DbSet<WorkItemStatus> WorkItemStatuses { get; set; } = null!;
        public virtual DbSet<WorkItemType> WorkItemTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=APD\\APDSERVER;User Id=txchrono;Password=Tx12cHrono34;Database=TimeTrack2_0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.DateBill).HasColumnType("datetime");
            });

            modelBuilder.Entity<BusinessLine>(entity =>
            {
                entity.ToTable("BusinessLine", "Cfg");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "Cfg");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<DefaultTimetable>(entity =>
            {
                entity.ToTable("DefaultTimetable", "dbo");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department", "Cfg");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<LocationHistory>(entity =>
            {
                entity.ToTable("LocationHistory", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTill).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationHistories)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationHistory_Location");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LocationHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationHistory_User");
            });

            modelBuilder.Entity<OutProjectInfoPattern>(entity =>
            {
                entity.ToTable("OutProjectInfoPattern", "Import");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.DateName).HasMaxLength(255);

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.TimeName).HasMaxLength(255);

                entity.Property(e => e.UserName).HasMaxLength(255);

                entity.Property(e => e.WorkItemName).HasMaxLength(255);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.OutProjectInfoPatterns)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OutProjectInfoPattern_ProjectId__Project_Id");
            });

            modelBuilder.Entity<OutTimeReportDatum>(entity =>
            {
                entity.ToTable("OutTimeReportData", "Import");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.DateValue)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserValue).HasMaxLength(255);

                entity.Property(e => e.WorkItemValue).HasMaxLength(255);

                entity.HasOne(d => d.OutTimeReportInfo)
                    .WithMany(p => p.OutTimeReportData)
                    .HasForeignKey(d => d.OutTimeReportInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OutTimeReportData_OutTimeReportInfoId__OutTimeReportInfo_Id");

                entity.HasOne(d => d.TimeReport)
                    .WithMany(p => p.OutTimeReportData)
                    .HasForeignKey(d => d.TimeReportId)
                    .HasConstraintName("FK__OutTimeReportsData_TimeReportId__TimeReport_Id");
            });

            modelBuilder.Entity<OutTimeReportInfo>(entity =>
            {
                entity.ToTable("OutTimeReportInfo", "Import");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.LoadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OutTimeReportInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OutTimeReportInfo_UserId__User_UserId");
            });

            modelBuilder.Entity<OutUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__OutUser__Id");

                entity.ToTable("OutUser", "Import");

                entity.HasIndex(e => e.OutName, "UNQ__OutUser__OutName")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.OutName).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.OutUser)
                    .HasForeignKey<OutUser>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OutUser_UserId__User_UserId");
            });

            modelBuilder.Entity<OutWorkItem>(entity =>
            {
                entity.ToTable("OutWorkItem", "Import");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.OutName).HasMaxLength(255);

                entity.HasOne(d => d.WorkItem)
                    .WithMany(p => p.OutWorkItems)
                    .HasForeignKey(d => d.WorkItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OutWorkItem_WorkItemId__WorkItem_Id");
            });

            modelBuilder.Entity<Pmsystem>(entity =>
            {
                entity.ToTable("PMSystem", "Cfg");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position", "Cfg");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.OutId).HasMaxLength(256);

                entity.Property(e => e.PmsystemId).HasColumnName("PMSystemId");

                entity.HasOne(d => d.Pmsystem)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.PmsystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_PMSystem");

                entity.HasOne(d => d.ProjectCollection)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectCollectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Project_ProjectCollection");
            });

            modelBuilder.Entity<ProjectCollection>(entity =>
            {
                entity.ToTable("ProjectCollection", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<ProjectRole>(entity =>
            {
                entity.ToTable("ProjectRole", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__ProjectRole_ProjectRole");
            });

            modelBuilder.Entity<ReportStatus>(entity =>
            {
                entity.ToTable("ReportStatus", "Cfg");
            });

            modelBuilder.Entity<RoleMember>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ProjectId, e.RoleId })
                    .HasName("PK__RoleMember");

                entity.ToTable("RoleMember", "dbo");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.RoleMembers)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMember_Project");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMembers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMember_ProjectRole");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoleMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMember_User");
            });

            modelBuilder.Entity<SentForApproveLog>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SentDate })
                    .HasName("PK__SentForApproveLog");

                entity.ToTable("SentForApproveLog", "dbo");

                entity.Property(e => e.SentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Settings", "Cfg");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Key).HasMaxLength(255);

                entity.Property(e => e.Value).HasMaxLength(1000);
            });

            modelBuilder.Entity<TimeReport>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__TimeReport__Id")
                    .IsClustered(false);

                entity.ToTable("TimeReport", "dbo");

                entity.HasIndex(e => new { e.UserId, e.WorkItemId, e.ReportDate, e.Type }, "UNQ__TimeReport_Main")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ReportDate).HasColumnType("date");

                entity.HasOne(d => d.ExternalBill)
                    .WithMany(p => p.TimeReportExternalBills)
                    .HasForeignKey(d => d.ExternalBillId)
                    .HasConstraintName("FK_TimeReport_Bill_External");

                entity.HasOne(d => d.InternalBill)
                    .WithMany(p => p.TimeReportInternalBills)
                    .HasForeignKey(d => d.InternalBillId)
                    .HasConstraintName("FK_TimeReport_Bill_Internal");

                entity.HasOne(d => d.ReportStatus)
                    .WithMany(p => p.TimeReports)
                    .HasForeignKey(d => d.ReportStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeReport_ReportStatus");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TimeReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_TimeReport_User");

                entity.HasOne(d => d.WorkItem)
                    .WithMany(p => p.TimeReports)
                    .HasForeignKey(d => d.WorkItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeReport_WorkItem");
            });

            modelBuilder.Entity<TimeReportHistory>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__TimeReportHistory__Id")
                    .IsClustered(false);

                entity.ToTable("TimeReportHistory", "dbo");

                entity.HasIndex(e => new { e.UserId, e.WorkItemId, e.ReportDate, e.Type }, "UNQ__TimeReportHistory_Main")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ReportDate).HasColumnType("date");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.UpdateUser)
                    .WithMany(p => p.TimeReportHistoryUpdateUsers)
                    .HasForeignKey(d => d.UpdateUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeReportHistory_UpdateUser");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TimeReportHistoryUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeReportHistory_User");

                entity.HasOne(d => d.WorkItem)
                    .WithMany(p => p.TimeReportHistories)
                    .HasForeignKey(d => d.WorkItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeReportHistory_WorkItem");
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Timetable");

                entity.ToTable("Timetable", "dbo");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Timetable)
                    .HasForeignKey<Timetable>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timetable_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "dbo");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.LocationFrom).HasColumnType("datetime");

                entity.Property(e => e.Sid).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(255);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_User_Country");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Location");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Position");
            });

            modelBuilder.Entity<UserBusinessLine>(entity =>
            {
                entity.ToTable("UserBusinessLine", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.BusinessLine)
                    .WithMany(p => p.UserBusinessLines)
                    .HasForeignKey(d => d.BusinessLineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBusinessLine_BusinessLine");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBusinessLines)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBusinessLine_User");
            });

            modelBuilder.Entity<UserDepartment>(entity =>
            {
                entity.ToTable("UserDepartment", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.UserDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDepartment_Department");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDepartments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDepartment_User");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo");

                entity.ToTable("UserInfo", "dbo");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.SynchronizeName).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserInfo)
                    .HasForeignKey<UserInfo>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserInfo_User");
            });

            modelBuilder.Entity<WorkItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__WorkItem__Id")
                    .IsClustered(false);

                entity.ToTable("WorkItem", "dbo");

                entity.HasIndex(e => new { e.ProjectId, e.OutId }, "UNQ__WorkItem_Main")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.OutId).HasMaxLength(256);

                entity.HasOne(d => d.WorkItemType)
                    .WithMany(p => p.WorkItems)
                    .HasForeignKey(d => d.WorkItemTypeId)
                    .HasConstraintName("FK_WorkItem_WorkItemType");
            });

            modelBuilder.Entity<WorkItemState>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__WorkItemState__Id")
                    .IsClustered(false);

                entity.ToTable("WorkItemState", "dbo");

                entity.HasIndex(e => new { e.WorkItemId, e.UserId, e.StartDate }, "UNQ__WorkItemState_Main")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.WorkItem)
                    .WithMany(p => p.WorkItemStates)
                    .HasForeignKey(d => d.WorkItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkItemState_WorkItem");

                entity.HasOne(d => d.WorkItemStatus)
                    .WithMany(p => p.WorkItemStates)
                    .HasForeignKey(d => d.WorkItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkItemState_WorkItemStatus");
            });

            modelBuilder.Entity<WorkItemStateHistory>(entity =>
            {
                entity.ToTable("WorkItemStateHistory", "dbo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ChangeDate).HasColumnType("datetime");

                entity.HasOne(d => d.ChangeUser)
                    .WithMany(p => p.WorkItemStateHistories)
                    .HasForeignKey(d => d.ChangeUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkItemStateHistory_User");

                entity.HasOne(d => d.WorkItemState)
                    .WithMany(p => p.WorkItemStateHistories)
                    .HasForeignKey(d => d.WorkItemStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkItemStateHistory_WorkItemState");

                entity.HasOne(d => d.WorkItemStatus)
                    .WithMany(p => p.WorkItemStateHistories)
                    .HasForeignKey(d => d.WorkItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkItemStateHistory_WorkItemStatus");
            });

            modelBuilder.Entity<WorkItemStatus>(entity =>
            {
                entity.ToTable("WorkItemStatus", "Cfg");
            });

            modelBuilder.Entity<WorkItemType>(entity =>
            {
                entity.ToTable("WorkItemType", "Cfg");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
