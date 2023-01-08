using AutoMapper;
using System;
using System.Collections.Generic;

namespace Chrono.Services.Models;

public class UserBaseInfo
{
    #region MapperConfiguration
    public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<DAL.EF.Model.User, UserBaseInfo>()
            .ForMember(dst => dst.Position, opt => opt.MapFrom(src => src.Position.Name ?? ""))
        //cfg.CreateMap<DAL.EF.Model.User, UserBaseInfo>()
            .ForMember(dst => dst.Department, opt => opt.MapFrom(src => src.UserDepartments.Select(x => x.Department.Name).ToList()))
            .ForMember(dst => dst.BusinessLine, opt => opt.MapFrom(src => src.UserBusinessLines.Select(x => x.BusinessLine.Name).Distinct().ToList()))
            .ForMember(dst => dst.Roles, opt => opt.MapFrom(src => src.RoleMembers.Select(x => x.Role.Name).Distinct().ToList()))
            ;
    });
    //Chrono.DAL.EF.Model  Department

    #endregion MapperConfiguration
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string? Position { get; set; }
    public IEnumerable<string>? Department { get; set; }

    public IEnumerable<string>? BusinessLine { get; set; }

    //public int LocationId { get; set; }
    //public DateTime LocationFrom { get; set; }
    public string Sid { get; set; } = null!;
    public bool Deleted { get; set; }
    public IEnumerable<string>? Roles { get; set; }
}