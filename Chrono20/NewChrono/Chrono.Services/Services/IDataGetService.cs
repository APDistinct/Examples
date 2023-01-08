using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Chrono.Services.Models;

namespace Chrono.Services.Services;

public interface IDataGetService
{
    //Task<IEnumerable<UserInfoSimple?>?> GetUserUsers(Guid userId);
    Task<IEnumerable<ProjectInfoSimple?>?> GetUserProjects(Guid userId, bool addDeleted = false);
    Task<IEnumerable<ProjectModel>?> GetProjects(Guid userIdList, DateTime dateFrom, int dayCount);
    Task<UserBaseInfo?> GetUserById(Guid userId);
    Task<UserBaseInfo?> GetUserBySid(string userName);
    Task<IEnumerable<UserBaseInfo>> GetUserByUserName(string userName);
    Task<IEnumerable<UserBaseInfo>> GetUsersByEmail(string email);
    Task<UserBaseInfo> WriteIncomeUser(UserInfoIncome data);
    Task<ProjectInfoSimple> WriteIncomeProject(ProjectInfoIncome data);
    Task<WorkItemInfoSimple> WriteIncomeWorkItem(WorkItemInfoIncome data);
    Task<UserBaseInfo?> ReadUserById(Guid userId);
    Task<IEnumerable<UserInfoSimple>?> GetUserUsers(Guid userId, bool addDeleted = false);
}