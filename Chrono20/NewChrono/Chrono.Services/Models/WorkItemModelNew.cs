using AutoMapper;
using Chrono.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chrono.Services.Models
{
    public class DayInfoNew
    {
        #region MapperConfiguration
        public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DAL.EF.Model.TimeReport, DayInfoNew>()
            .ForMember(dst => dst.ReportStatus, opt => opt.MapFrom(src => src.ReportStatus.Status))
            .ForMember(dst => dst.IsOvertime, opt => opt.MapFrom(src => src.Type == (int)ETimeReportType.Overtime));

        });
        //Chrono.DAL.EF.Model  Department

        #endregion MapperConfiguration
        public DateTime ReportDate { get; set; }
        public bool IsOvertime { get; set; }
        public Guid Id { get; set; }
        public double? Hours { get; set; }
        public double? BillHours { get; set; }
        public string? ReportStatus { get; set; }
        public bool IsEditable { get; set; } = false;
    }
    public class WorkItemModelNew : WorkItemInfoSimple
    {
        #region MapperConfiguration
        public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DAL.EF.Model.WorkItem, WorkItemModelNew>()
            .ForMember(dst => dst.WorkItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.WorkItemType, opt => opt.MapFrom(src => src.WorkItemType.Name))
            //.ForMember(dst => dst.StartDate, opt => opt.MapFrom(src => src.WorkItemStates.Where(w => w.UserId == src.).FirstOrDefault(x => x.StartDate)))
            //.ForMember(dst => dst.ItemTimes, opt => opt.MapFrom(src => src.TimeReports))
            ;

        //cfg.CreateMap<DAL.EF.Model.TimeReport, WorkItemModelNew.DayInfo>()
        //.ForMember(dst => dst.ReportStatus, opt => opt.MapFrom(src => src.ReportStatus.Status))
        //.ForMember(dst => dst.IsOvertime, opt => opt.MapFrom(src => src.Type == 1));

        });
        //Chrono.DAL.EF.Model  Department

        #endregion MapperConfiguration
        [JsonIgnore]
        public DateTime StartDate { get; set; }
        [JsonIgnore]
        public DateTime EndDate { get; set; }
        public /*IEnumerable*/ICollection<DayInfoNew>? ItemTimes { get; set; }

    }
    
}