using AutoMapper;
using System;

namespace Chrono.Services.Models;

public class UserInfoSimple
{
    #region MapperConfiguration
    public static MapperConfiguration MapperConfig => new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<DAL.EF.Model.User, UserInfoSimple>();
    });
    
    #endregion MapperConfiguration
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
}