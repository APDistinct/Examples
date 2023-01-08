using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Models
{
    public class TimeReportModelNew
    {
        #region MapperConfiguration
        //public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
        //{
        //    cfg.CreateMap<TimeReportDBModel, TimeReportModelNew>();

        //    cfg.CreateMap<DAL.EF.Model.User, UserBaseInfo>()
        //        .ForMember(dst => dst.Position, opt => opt.MapFrom(src => src.Position.Name ?? ""));
        //    cfg.CreateMap<DAL.EF.Model.User, UserBaseInfo>()
        //        .ForMember(dst => dst.Department, opt => opt.MapFrom(src => src.UserDepartments.Select(x => x.Department.Name).ToList()));
        //    cfg.CreateMap<ProjectDBModel, ProjectModelNew>()
        //    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Project.Id));
        //    cfg.CreateMap<ProjectDBModel, ProjectModelNew>()
        //    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Project.Name))
        //    //cfg.CreateMap<ProjectDBModel, ProjectModelNew>()
        //    .ForMember(dst => dst.OutId, opt => opt.MapFrom(src => src.Project.OutId));
        //    cfg.CreateMap<ProjectDBModel, ProjectModelNew>()
        //    .ForMember(dst => dst.IsDeleted, opt => opt.MapFrom(src => src.Project.IsDeleted));
        //    cfg.CreateMap<ProjectDBModel, ProjectModelNew>()
        //    .ForMember(dst => dst.Pmsystem, opt => opt.MapFrom(src => src.Project.Pmsystem.Name));
        //    //cfg.CreateMap<DAL.EF.Model.WorkItem, WorkItemModelNew.DayInfo>()
        //    //.ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));
        //    //cfg.CreateMap<DAL.EF.Model.TimeReport, WorkItemModelNew.DayInfo>()
        //    //.ForMember(dst => dst.ReportDate, opt => opt.MapFrom(src => src.ReportDate));
        //    //cfg.CreateMap<DAL.EF.Model.WorkItem, WorkItemModelNew.DayInfo>()
        //    //.ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));
        //});
        //Chrono.DAL.EF.Model  Department
        
        #endregion MapperConfiguration

        public UserBaseInfo? User { get; set; }
        public DateTime DateFrom { get; set; }
        public int DayCount { get; set; }
        public /*IEnumerable*/ICollection <IProjectModelNew> Projects { get; set; } = new List<IProjectModelNew>();
    }
}
