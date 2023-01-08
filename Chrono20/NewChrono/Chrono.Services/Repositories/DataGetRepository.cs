using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chrono.DAL.EF.Model;
using Chrono.Services.Models;

using Microsoft.EntityFrameworkCore;

namespace Chrono.Services.Repositories;

public interface IDataGetRepository
{
    Task<List<User>?> GetUserUsersAsync(Guid userId);
    //Task<List<Project>?> GetUserProjectsAsync(Guid userId);
    Task<List<Project>> GetUserProjectsAsync(Guid userId, bool addDeleted = false);
    Task<List<ProjectModel>?> ReadProjectInfoAsync(Guid userId, DateTime dateFrom, int dayCount);
    Task<User?> ReadUserAsync(Guid userId);
    //Task<User?> ReadUserBySidAsync(string sid);
    //Task<User?> ReadUserByUserNameAsync(string name);
    Task<Project?> ReadProjectByNameAsync(string name, Guid systemId);
    Task<Project?> ReadProjectByOutIdAsync(string outId, Guid systemId);
    Task<WorkItem?> ReadWorkItemByIdAsync(string outId, Guid projectId);
    Task<User> WriteUserAsync(UserInfoIncomeInner userInfo);
    Task<Project> WriteProjectAsync(ProjectInfoIncomeInner projectInfo);
    Task<WorkItem?> WriteWorkItemAsync(WorkItemInfoIncomeInner workitemInfo);
    Task<Position> ReadOrCreatePositionByNameAsync(string data);
    Task<Country> ReadOrCreateCountryByNameAsync(string data);
    Task<Location> ReadOrCreateLocationByNameAsync(string data);
    ///
    Task<UserBaseInfo?> ReadUserBySid(string sid);
    Task<IProjectInfoSimple?> ReadProjectById(Guid projectId);
    Task<UserBaseInfo?> ReadUserById(Guid uid);
    Task<IEnumerable<UserBaseInfo>> ReadUserByUserName(string name);
    Task<IEnumerable<UserBaseInfo>> ReadUserByEmail(string email);
    Task<IEnumerable<UserInfoSimple>?> GetUserUsers(Guid userId, bool addDeleted = false);
    Task<IEnumerable<ProjectInfoSimple>> GetUserProjects(Guid userId, bool addDeleted = false);
    Task<IEnumerable<UserInfoSimple>?> GetUsersByProjects(IEnumerable<Guid> projectIds, bool addDeleted = false);
}

public class DataGetRepository : BaseRepository, IDataGetRepository
{
    private readonly ChronoContext _context;
    private string[] manageRoles = new[] { "Project Manager", "Account manager", "Resource manager", "Project Owner" };
    private string[] workRoles = new[] { "User" };
    public DataGetRepository(ChronoContext context, IMapper mapper) : base(mapper)
    {
        _context = context;
    }

    public async Task<User?> ReadUserAsync(Guid num)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == num);
#pragma warning restore CS8603 // Possible null reference return.
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dayCount"></param>
    /// <returns></returns>
    public async Task<List<ProjectModel>?> ReadProjectInfoAsync(Guid userId, DateTime dateFrom, int dayCount)
    {
        var list = new List<ProjectModel>();
        //var items = new List<WorkItemModel>();
        list.Add(new ProjectModel { Id = Guid.NewGuid(), Name = "Test", /*WorkItems =*/ });
        return await Task.FromResult(list);
    }
    /// </summary>
    /// <param name="uId">user's Id</param>
    /// <param name="wId">WI Id</param>
    /// <param name="dateFrom">start date</param>
    /// <param name="dayCount">count of days</param>
    /// <returns></returns>
    public async Task<WorkItemModelNew?> GetUserTimeReport(Guid uId, Guid wId, DateTime dateFrom, int dayCount)
    {
        var wimodel = await _context.WorkItems.Where(r => r.Id == wId)
            .ProjectTo<WorkItemModelNew>(WorkItemModelNew.MapperConfig).FirstOrDefaultAsync();
        if (wimodel != null)
        {
            var query = _context.TimeReports.Where(r => r.WorkItemId == wId && r.UserId == uId && r.ReportDate >= dateFrom && r.ReportDate <= dateFrom.AddDays(dayCount));
            var list = await query.Select(w => w.Id).ProjectTo</*WorkItemModelNew.*/DayInfoNew>(/*WorkItemModelNew.*/DayInfoNew.MapperConfig).ToListAsync();
            wimodel.ItemTimes = list;
        }
        return wimodel;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<User>?> GetUserUsersAsync(Guid userId)
    {
        var list = new List<User>();
        //var items = new List<WorkItemModel>();
        list.Add(new User { UserId = Guid.NewGuid(), });
        return await Task.FromResult(list);
    }
    public async Task<IEnumerable<UserInfoSimple>?> GetUserUsers(Guid userId, bool addDeleted = false)
    {
        var list = await _context.RoleMembers.Where(p => manageRoles.Contains(p.Role.Name) && p.UserId == userId).Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted)
            /*.Include(p => p.Pmsystem)*/.Select(w => w.Id).ToListAsync();
        if(list == null || list.Count <= 0)
            return new List<UserInfoSimple>();
        //var u1 = await _context.RoleMembers.Where(p => workRoles.Contains(p.Role.Name) && list.Contains(p.ProjectId)).Select(x => x.User).ToListAsync();

        //var users = await _context.RoleMembers.Where(p => workRoles.Contains(p.Role.Name) && list.Contains(p.ProjectId)).Select(x => x.User).Where(z => !z.Deleted || addDeleted)
        //    .ProjectTo<UserInfoSimple>(UserInfoSimple.MapperConfig)
        //    .ToListAsync();
        //if (addDeleted)
        //{
        //    var list1 = await _context.TimeReports.Where(r => list.Contains(r.WorkItem.ProjectId) ).Select(u => u.User).Distinct()
        //        .ProjectTo<UserInfoSimple>(UserInfoSimple.MapperConfig).ToListAsync();
        //    users.AddRange(list1);
        //}
        //return users.DistinctBy(x => x.UserId).ToList();
        return await GetUsersByProjects(list, addDeleted);
    }
    public async Task<IEnumerable<UserInfoSimple>?> GetUsersByProjects(IEnumerable<Guid> projectIds, bool addDeleted = false)
    {
        //var u1 = await _context.RoleMembers.Where(p => workRoles.Contains(p.Role.Name) && list.Contains(p.ProjectId)).Select(x => x.User).ToListAsync();
        var users = await _context.RoleMembers.Where(p => workRoles.Contains(p.Role.Name) && projectIds.Contains(p.ProjectId))
            .Select(x => x.User).Where(z => !z.Deleted || addDeleted)
            .ProjectTo<UserInfoSimple>(UserInfoSimple.MapperConfig)
            .ToListAsync();
        if (addDeleted)
        {
            var list1 = await _context.TimeReports.Where(r => projectIds.Contains(r.WorkItem.ProjectId)).Select(u => u.User).Distinct()
                .ProjectTo<UserInfoSimple>(UserInfoSimple.MapperConfig).ToListAsync();
            users.AddRange(list1);
        }
        return users.DistinctBy(x => x.UserId).ToList();
    }
    public async Task<IEnumerable<ProjectInfoSimple>> GetUserProjects(Guid userId, bool addDeleted = false)
    {
        // Со временем сделать настройку из конфигурации
        var roles = manageRoles;
        //new[] { "Project Manager", "Account manager", "Resource manager", "Project Owner" };

        //var ll = await _context.RoleMembers.Where(p => roles.Contains(p.Role.Name) /*&& p.UserId == userId*/).ToListAsync();
        //var ll = await _context.RoleMembers.Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted).ToListAsync();
        //var ll1 = await _context.RoleMembers.Select(p => p.Role.Name).ToListAsync();
        //var ll2 = await _context.Projects.ToListAsync();
        var list = await _context.RoleMembers.Where(p => roles.Contains(p.Role.Name) && p.UserId == userId).Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted)
            .ProjectTo<ProjectInfoSimple>(ProjectInfoSimple.MapperConfig).ToListAsync();
        //new List<Project>();
        //var items = new List<WorkItemModel>();
        //list.Add(new Project { Id = Guid.NewGuid(), });
        return list;
        //return await Task.FromResult(list);
    }
    public async Task<List<Project>> GetUserProjectsAsync(Guid userId, bool addDeleted = false)
    {
        // Со временем сделать настройку из конфигурации
        var roles = manageRoles;
        //new[] { "Project Manager", "Account manager", "Resource manager", "Project Owner" };

        //var ll = await _context.RoleMembers.Where(p => roles.Contains(p.Role.Name) /*&& p.UserId == userId*/).ToListAsync();
        //var ll = await _context.RoleMembers.Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted).ToListAsync();
        //var ll1 = await _context.RoleMembers.Select(p => p.Role.Name).ToListAsync();
        //var ll2 = await _context.Projects.ToListAsync();
        var list = await _context.RoleMembers.Where(p => roles.Contains(p.Role.Name) && p.UserId == userId).Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted)
            /*.Include(p => p.Pmsystem)*/.ToListAsync();
        //new List<Project>();
        //var items = new List<WorkItemModel>();
        //list.Add(new Project { Id = Guid.NewGuid(), });
        return list;
        //return await Task.FromResult(list);
    }
    public async Task<List<Guid>> GetUserWorkProjects(Guid userId, bool addDeleted = false)
    {
        // Со временем сделать настройку из конфигурации
        var roles = workRoles; // new[] { "User" };
        //var ll = await _context.RoleMembers.Where(p => roles.Contains(p.Role.Name) /*&& p.UserId == userId*/).ToListAsync();
        //var ll = await _context.RoleMembers.Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted).ToListAsync();
        //var ll1 = await _context.RoleMembers.Select(p => p.Role.Name).ToListAsync();
        //var ll2 = await _context.Projects.ToListAsync();
        var list = await _context.RoleMembers.Where(p => roles.Contains(p.Role.Name) && p.UserId == userId).Select(x => x.Project).Where(z => !z.IsDeleted || addDeleted)
            /*.Include(p => p.Pmsystem)*/.Select(p => p.Id).ToListAsync();
        //new List<Project>();
        //var items = new List<WorkItemModel>();
        //list.Add(new Project { Id = Guid.NewGuid(), });
        return list;
    }
    /// <summary>
    ///     Write new user or change user's info for existing. Search key - Sid
    /// </summary>
    /// <param name="userInfo">Info about user. Sid must exist</param>
    /// <returns></returns>
    public async Task<User> WriteUserAsync(UserInfoIncomeInner userInfo)
    {
        var user = await ReadUserBySidAsync(userInfo.Sid);
        bool pri;
        if (pri = user == null)
            user = new User
            {
                Sid = userInfo.Sid,
            };
        user.CountryId = userInfo.CountryId;
        user.UserName = userInfo.UserName;
        user.DisplayName = userInfo.DisplayName;
        user.Deleted = userInfo.Deleted;
        user.Email = userInfo.Email;
        user.LocationId = userInfo.LocationId;
        user.PositionId = userInfo.PositionId;
        user.LocationFrom = userInfo.LocationFrom;

        if (pri) await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    ///     Write new project or change project's info for existing. Search key - Name
    ///     Временно - ключ поле Name. Необходимо будет добавить внешний Id и, вероятно, признак, откуда приходят данные. Id в
    ///     справочнике внешних источников.
    /// </summary>
    /// <param name="projectInfo">Info about project. Name(temporary) must exist</param>
    /// <returns></returns>
    public async Task<Project> WriteProjectAsync(ProjectInfoIncomeInner projectInfo)
    {
        var project = await ReadProjectByOutIdAsync(projectInfo.OutId, projectInfo.PmsystemId);
        bool pri;
        if (pri = project == null)
            project = new Project
            {
                OutId = projectInfo.OutId,
                PmsystemId = projectInfo.PmsystemId,
            };
        project.Name = projectInfo.Name;
        project.Uri = projectInfo.Uri;
        project.ProjectCollectionId = projectInfo.ProjectCollectionId;
        project.IsDeleted = projectInfo.IsDeleted;

        if (pri) await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    /// <summary>
    ///     Write new WorkItem or change WorkItem's info for existing. Search key - ItemId
    /// </summary>
    /// <param name="workitemInfo">Info about WorkItem. ItemId must exist</param>
    /// <returns>WorkItem</returns>
    public async Task<WorkItem?> WriteWorkItemAsync(WorkItemInfoIncomeInner workitemInfo)
    {
        var workItem = await ReadWorkItemByIdAsync(workitemInfo.OutId, workitemInfo.ProjectId);
        bool pri;
        if (pri = workItem == null)
            workItem = new WorkItem
            {
                OutId = workitemInfo.OutId,
                ProjectId = workitemInfo.ProjectId,
            };
        workItem.Estimate = workitemInfo.Estimate;
        workItem.Name = workitemInfo.Name;
        //workItem.ProjectId = workitemInfo.ProjectId;
        workItem.WorkItemTypeId = workitemInfo.WorkItemTypeId;

        if (pri) await _context.WorkItems.AddAsync(workItem);
        await _context.SaveChangesAsync();
        return workItem;
    }

    /// <summary>
    /// Find all users, who has Name in DB equal param
    /// </summary>
    /// <param name="name">User's name</param>
    /// <returns>All users with the same name</returns>
    public async Task<IEnumerable<UserBaseInfo>> ReadUserByUserName(string name)
    {
        return await _context.Users.Where(u => u.UserName == name)
            .ProjectTo<UserBaseInfo>(UserBaseInfo.MapperConfig).ToListAsync(); ;
    }

    private async Task<User?> ReadUserBySidAsync(string sid)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Sid == sid);
    }
    /// <summary>
    /// Get user by Sid parameter. Convert to UserBaseInfo.
    /// </summary>
    /// <param name="sid"></param>
    /// <returns>User with sid equal to the parameter. May be null</returns>
    public async Task<UserBaseInfo?> ReadUserBySid(string sid)
    {
        return await _context.Users.Where(u => u.Sid == sid)
            .ProjectTo<UserBaseInfo>(UserBaseInfo.MapperConfig).FirstOrDefaultAsync();
    }
    /// <summary>
    /// Get user by Id parameter. Convert to UserBaseInfo.
    /// </summary>
    /// <param name="uid"></param>
    /// <returns>User with id equal to the parameter. May be null</returns>
    public async Task<UserBaseInfo?> ReadUserById(Guid uid)
    {
        return await _context.Users.Where(u => u.UserId == uid)
            .ProjectTo<UserBaseInfo>(UserBaseInfo.MapperConfig).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<UserBaseInfo>> ReadUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email)
            .ProjectTo<UserBaseInfo>(UserBaseInfo.MapperConfig).ToListAsync();
    }

    public async Task<IProjectInfoSimple?> ReadProjectById(Guid projectId)
    {
        return await _context.Projects.Where(u => u.Id == projectId)
            .ProjectTo<ProjectInfoSimple>(ProjectInfoSimple.MapperConfig).FirstOrDefaultAsync(); 
    }
    public async Task<Project?> ReadProjectByNameAsync(string name, Guid systemId)
    {
        return await _context.Projects.FirstOrDefaultAsync(u => u.Name == name && u.PmsystemId == systemId);
    }

    /// <summary>
    /// </summary>
    /// <param name="outId">Outer Id from PM system</param>
    /// <param name="systemId">Id of system's kind in DB</param>
    /// <returns></returns>
    public async Task<Project?> ReadProjectByOutIdAsync(string outId, Guid systemId)
    {
        return await _context.Projects.FirstOrDefaultAsync(u => u.OutId == outId && u.PmsystemId == systemId);
    }

    /// <summary>
    /// </summary>
    /// <param name="outId">Outer Id from PM system</param>
    /// <param name="projectId">project's Id in DB</param>
    /// <returns></returns>
    public async Task<WorkItem?> ReadWorkItemByIdAsync(string outId, Guid projectId)
    {
        return await _context.WorkItems.FirstOrDefaultAsync(u => u.OutId == outId && u.ProjectId == projectId);
    }

    public async Task<Position> ReadOrCreatePositionByNameAsync(string data)
    {
        var temp = await _context.Positions.FirstOrDefaultAsync(u => u.Name == data);
        if (temp == null)
        {
            temp = new Position
            {
                Name = data,
            };
            await _context.Positions.AddAsync(temp);
            await _context.SaveChangesAsync();
        }

        return (Position)temp;
    }

    public async Task<Country> ReadOrCreateCountryByNameAsync(string data)
    {
        var temp = await _context.Countries.FirstOrDefaultAsync(u => u.Name == data);
        if (temp == null)
        {
            temp = new Country
            {
                Name = data,
            };
            await _context.Countries.AddAsync(temp);
            await _context.SaveChangesAsync();
        }

        return (Country)temp;
    }

    public async Task<Location> ReadOrCreateLocationByNameAsync(string data)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(u => u.Name == data);
        if (location == null)
        {
            location = new Location
            {
                Name = data,
                Type = 1,
            };
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
        }

        return location;
    }

    public User ReadUser(Guid num)
    {
        return _context.Users.FirstOrDefault(u => u.UserId == num);
    }

    //public async Task<UserBaseInfo?> ReadUserInfoAsync(Guid userId)
    //{
    //    UserBaseInfo? userInfo = null;
    //    // Тестово для демонстрации структуры ответа
    //    return await Task.FromResult(userInfo = new UserBaseInfo { UserId = userId, });
    //    //var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    //    //if (user != null)
    //    //{
    //    //    userInfo = new UserBaseInfo()
    //    //    { 
    //    //        UserId = userId, 
    //    //        Department = await _context.Departments.Where(d => d.Id == user.DepartmentId).Select(x => x.Name).FirstOrDefaultAsync(),
    //    //        DisplayName = user.DisplayName,
    //    //        Position = await _context.Positions.Where(d => d.Id == user.PositionId).Select(x => x.Name).FirstOrDefaultAsync(),
    //    //        UserName = user.UserName,
    //    //        Sid = user.Sid,
    //    //        Email = user.Email,
    //    //        Deleted = user.Deleted,
    //    //    };
    //    //}
    //    //return userInfo;
    //}
}