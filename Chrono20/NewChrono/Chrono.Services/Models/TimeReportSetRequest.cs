using AutoMapper;
using Chrono.Services.Enums;
using System.Collections.Generic;

namespace Chrono.Services.Models;

public interface ITimeReportSetRequest
{
    public Guid ReportUserId { get; set; }
    IEnumerable<DayInfoSet>? WorkInfos { get; set; }
}


public class DayInfoSet
{
    #region MapperConfiguration
    public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<DayInfoSet, DAL.EF.Model.TimeReport>()
        .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.IsOvertime ? (int)ETimeReportType.Overtime : (int)ETimeReportType.WorkTime));
        cfg.CreateMap<DayInfoSet, DAL.EF.Model.TimeReportHistory>()
        .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.IsOvertime ? (int)ETimeReportType.Overtime : (int)ETimeReportType.WorkTime));
    });

    #endregion MapperConfiguration
    public Guid UserId { get; set; }
    public Guid WorkItemId { get; set; }
    public DateTime ReportDate { get; set; }
    public bool IsOvertime { get; set; }
    public Guid? Id { get; set; }
    public double? Hours { get; set; }
    public double? BillHours { get; set; }    
}

public class TimeReportSetRequest : ITimeReportSetRequest
{
    public Guid ReportUserId { get; set; }
    public IEnumerable<DayInfoSet>? WorkInfos { get; set; }
}