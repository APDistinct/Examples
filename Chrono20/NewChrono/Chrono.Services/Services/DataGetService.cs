using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chrono.DAL.EF.Model;
using Chrono.Services.Models;
using Chrono.Services.Repositories;

namespace Chrono.Services.Services;

public class DataGetService : IDataGetService
{
    private readonly IDataGetRepository dataGetRepository;

    public DataGetService(IDataGetRepository dataGetRepository)
    {
        this.dataGetRepository = dataGetRepository;
    }

    public async Task<IEnumerable<ProjectModel>?> GetProjects(Guid userIdList, DateTime dateFrom, int dayCount)
    {
        return await dataGetRepository.ReadProjectInfoAsync(userIdList, dateFrom, dayCount);
        //var list = new List<ProjectModel>();
        //var items = new List<WorkItemModel>();
        //list.Add(new ProjectModel() { Id = Guid.NewGuid(), Name = "Test", /*WorkItems =*/ });
        //return await Task.FromResult(list);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="addDeleted"></param>
    /// <returns></returns>
    public async Task<IEnumerable<UserInfoSimple>?> GetUserUsers(Guid userId, bool addDeleted = false)
    {
        var list = await dataGetRepository.GetUserUsers(userId, addDeleted);        
        //  Добавить персональных подчинённых. Попозже - после проектирования в БД.
        return list?.DistinctBy(x => x.UserId);
    }
    //public async Task<IEnumerable<UserInfoSimple?>?> GetUserUsers(Guid userId)
    //{
    //    var list = await dataGetRepository.GetUserUsersAsync(userId);
    //    var info = list?.Select(user => GetUserSimple(user)
    //        //new UserBaseInfo()
    //        //{
    //        //    UserId = user.UserId,
    //        //    Email = user.Email,
    //        //    BusinessLine = user.UserBusinessLines?.Where(u => u.UserId == user.UserId)?.Select(l => l.BusinessLine.Name).ToList(),
    //        //    Department = user.UserDepartments.Where(u => u.UserId == user.UserId)?.Select(l => l.Department.Name).ToList(),
    //        //    Deleted = user.Deleted,
    //        //    Sid = user.Sid,
    //        //    DisplayName = user.DisplayName,
    //        //    Position = user.Position.Name,
    //        //    UserName = user.UserName,
    //        //    Roles = user.RoleMembers?.Select(r => r.Role?.Name).ToList(),
    //        //}
    //    ).ToList();
    //    return info;
    //    //var list = new List<ProjectModel>();
    //    //var items = new List<WorkItemModel>();
    //    //list.Add(new ProjectModel() { Id = Guid.NewGuid(), Name = "Test", /*WorkItems =*/ });
    //    //return await Task.FromResult(list);
    //}

    public async Task<IEnumerable<ProjectInfoSimple?>?> GetUserProjects(Guid userId, bool addDeleted = false)
    {
        var list = await dataGetRepository.GetUserProjects(userId, addDeleted);
        //var list = await dataGetRepository.GetUserProjectsAsync(userId, addDeleted);
        //var info = list?.Select(pr => GetProjectSimple(pr)).ToList();
        //return info;
        return list?.DistinctBy(x => x.Id)/*.ToList()*/;
    }

    public async Task<UserBaseInfo?> GetUserBySid(string sid)
    {
        var user = await dataGetRepository.ReadUserBySid(sid);
        return user;
    }

    public async Task<UserBaseInfo?> GetUserById(Guid userId)
    {
        var user = await dataGetRepository.ReadUserAsync(userId);
        //var userInfo = user == null ? null : new UserBaseInfo()
        //{ UserId = user.UserId,
        // Email = user.Email,
        // BusinessLine = user.UserBusinessLines?.Where(u => u.UserId == user.UserId)?.Select(l => l.BusinessLine.Name).ToList(),
        // Department = user.UserDepartments.Where(u => u.UserId == user.UserId)?.Select(l => l.Department.Name).ToList(),
        // Deleted = user.Deleted,
        // Sid = user.Sid,
        // DisplayName = user.DisplayName,
        // Position = user.Position.Name,
        // UserName = user.UserName,
        //};
        //return userInfo;
        return GetUser(user);
        //return await Task.FromResult(new UserBaseInfo() {UserId = userId });
    }
    public async Task<UserBaseInfo?> ReadUserById(Guid userId)
    {
        var user = await dataGetRepository.ReadUserById(userId);
       
        return user;
    }
    /// <summary>
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<UserBaseInfo>> GetUserByUserName(string userName)
    {
        var user = await dataGetRepository.ReadUserByUserName(userName);
        return user;
        //return await Task.FromResult(new UserBaseInfo() { UserName = userName });
    }

    public async Task<IEnumerable<UserBaseInfo>> GetUsersByEmail(string email)
    {
        var user = await dataGetRepository.ReadUserByEmail(email);
        return user;
    }

    /// <summary>
    ///     Write new user or update data if user with the same Sid already exists
    /// </summary>
    /// <param name="data">user's data</param>
    /// <returns>user's info in UserBaseInfo format</returns>
    public async Task<UserBaseInfo> WriteIncomeUser(UserInfoIncome data)
    {
        UserInfoIncomeInner userinfo = new();

        userinfo.LocationFrom = data.LocationFrom;
        userinfo.DisplayName = data.DisplayName;
        userinfo.Deleted = data.Deleted;
        userinfo.Sid = data.Sid;
        userinfo.UserName = data.UserName;
        userinfo.Email = data.Email;

        if (data.Position != null)
        {
            var position = await dataGetRepository.ReadOrCreatePositionByNameAsync(data.Position);
            userinfo.PositionId = position.Id;
        }

        if (data.Country != null)
        {
            var temp = await dataGetRepository.ReadOrCreateCountryByNameAsync(data.Country);
            userinfo.CountryId = temp.Id;
        }

        if (data.Location != null)
        {
            var temp = await dataGetRepository.ReadOrCreateLocationByNameAsync(data.Location);
            userinfo.LocationId = temp.Id;
        }

        var user = await dataGetRepository.WriteUserAsync(userinfo);
        return GetUser(user);
    }

    /// <summary>
    ///     Write new project or update data if project with the same OutId and PmsystemId already exists
    /// </summary>
    /// <param name="data">project's data</param>
    /// <returns>project's info in ProjectInfoSimple format</returns>
    public async Task<ProjectInfoSimple> WriteIncomeProject(ProjectInfoIncome data)
    {
        ProjectInfoIncomeInner info = new()
        {
            OutId = data.OutId,
            PmsystemId = data.PmsystemId,
            Name = data.Name,
            IsDeleted = data.IsDeleted,
            //ProjectCollectionId = data.ProjectCollectionName,
            Uri = data.Uri,
        };
        var ret = await dataGetRepository.WriteProjectAsync(info);

        return GetProjectSimple(ret);
    }

    /// <summary>
    ///     Write new WI or update data if the one with same OutId and ProjectId already exists
    /// </summary>
    /// <param name="data">WI's data</param>
    /// <returns>WI's info in WorkItemInfoSimple format</returns>
    public async Task<WorkItemInfoSimple> WriteIncomeWorkItem(WorkItemInfoIncome data)
    {
        WorkItemInfoIncomeInner info = new()
        {
            Name = data.Name,
            Estimate = data.Estimate,
            //ProjectId = dataGetRepository.ReadProjectByNameAsync( data.Project),
            OutId = data.OutId,
            //WorkItemTypeId = dataGetRepository. data.WorkItemType.i
            //ProjectCollectionId = data.ProjectCollectionName,
        };
        if (data.Project != null)
        {
            var temp = await dataGetRepository.ReadProjectByNameAsync(data.Project, data.PmsystemId);
            info.ProjectId = temp.Id;
        }

        //if (data.WorkItemType != null)
        //{
        //    var temp = await dataGetRepository.ReadProjectByNameAsync(data.Project);
        //    info.ProjectId = temp.Id;
        //}
        var ret = await dataGetRepository.WriteWorkItemAsync(info);

        return GetWorkItemSimple(ret);
    }

    private static UserBaseInfo? GetUser(User? user)
    {
        UserBaseInfo? userInfo = null;
        if (user != null)
            userInfo = new UserBaseInfo
            {
                UserId = user.UserId,
                Email = user.Email,
                BusinessLine = user.UserBusinessLines?.Where(u => u.UserId == user.UserId)?.Select(l => l.BusinessLine.Name).ToList(),
                Department = user.UserDepartments?.Where(u => u.UserId == user.UserId)?.Select(l => l.Department.Name).ToList(),
                Deleted = user.Deleted,
                Sid = user.Sid,
                DisplayName = user.DisplayName,
                Position = user.Position?.Name,
                UserName = user.UserName,
                Roles = user.RoleMembers?.Select(r => r.Role?.Name).ToList(),
            };
        ;
        return userInfo;
    }

    private static UserInfoSimple? GetUserSimple(User? user)
    {
        UserInfoSimple? userInfo = null;
        if (user != null)
            userInfo = new UserInfoSimple
            {
                UserId = user.UserId,
                UserName = user.UserName,
            };
        ;
        return userInfo;
    }

    private static ProjectInfoSimple? GetProjectSimple(Project? data)
    {
        ProjectInfoSimple? info = null;
        if (data != null)
            info = new ProjectInfoSimple
            {
                Id = data.Id,
                Name = data.Name,
                OutId = data.OutId,
                Pmsystem = data.Pmsystem?.Name ?? "",
                IsDeleted = data.IsDeleted,
            };
        ;
        return info;
    }

    private static WorkItemInfoSimple? GetWorkItemSimple(WorkItem? data)
    {
        WorkItemInfoSimple? info = null;
        if (data != null)
            info = new WorkItemInfoSimple
            {
                Estimate = data.Estimate,
                WorkItemType = data.WorkItemType?.Name,
                Name = data.Name,
                WorkItemId = data.Id,
            };
        ;
        return info;
    }
}