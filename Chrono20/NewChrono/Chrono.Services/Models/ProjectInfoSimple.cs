using AutoMapper;
using System;

namespace Chrono.Services.Models;

public interface IProjectInfoSimple
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string OutId { get; set; } 
    public string? Pmsystem { get; set; }
    public bool IsDeleted { get; set; }
}
public class ProjectInfoSimple : IProjectInfoSimple
{
    #region MapperConfiguration
    public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<DAL.EF.Model.Project, ProjectInfoSimple>()
            .ForMember(dst => dst.Pmsystem, opt => opt.MapFrom(src => src.Pmsystem.Name ?? ""));
    });
    //Chrono.DAL.EF.Model  Department

    #endregion MapperConfiguration
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string OutId { get; set; } = "";
    public string? Pmsystem { get; set; }
    public bool IsDeleted { get; set; }
}