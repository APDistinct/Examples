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

public interface ITimeReportRepository
{
    //Task<TimeReportModel> GetTimeReport(Guid userId, DateTime dateFrom, int dayCount);
    Task<List<TimeReport>> GetTimeReports(IEnumerable<Guid> userIdList, IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount);
    Task<List<TimeReportDBModel>> GetTimeDBReports(IEnumerable<Guid> userIdList, IEnumerable<Guid> wiIdList, DateTime dateFrom, int dayCount);
    //Task<List<User>?> GetUsersByProjectsAsync(IEnumerable<Guid> projectIdList);
    Task<WorkItemModelNew?> GetUserTimeReport(Guid uId, Guid wId, DateTime dateFrom, int dayCount);
    Task<List<Guid>> GetUserWIList(Guid uId, DateTime dateFrom, int dayCount);
    Task<List<WorkItemInfoBase>> GetWIList(Guid uId, DateTime dateFrom, int dayCount);
    Task<int> SetTimeReports(ITimeReportSetRequest setRequest, bool Transaction = true);
}

public class TimeReportRepository : ITimeReportRepository
{
    private readonly ChronoContext _context;

    public TimeReportRepository(ChronoContext context)
    {
        _context = context;
    }

    //public async Task<List<User>?> GetUsersByProjectsAsync(IEnumerable<Guid> projectIdList)
    //{
    //    var list = new List<User>();
    //    //var items = new List<WorkItemModel>();
    //    list.Add(new User { UserId = Guid.NewGuid(), });
    //    //  For real 
    //    //var userIdList = _context.RoleMembers.Where(p => projectIdList.Contains(p.ProjectId)).Select(r => r.UserId).ToList();
    //    //list = _context.Users.Where(u => userIdList.Contains(u.UserId)).ToList();
    //    return await Task.FromResult(list);
    //}
    
    /// <summary>
    /// Get time report's data filtering by lists of users and WIs
    /// </summary>
    /// <param name="userIdList">user's Ids</param>
    /// <param name="wiIdList">WI Ids</param>
    /// <param name="dateFrom">start date</param>
    /// <param name="dayCount">count od days</param>
    /// <returns></returns>
    public async Task<List<TimeReport>> GetTimeReports(IEnumerable<Guid> userIdList, IEnumerable<Guid> wiIdList, DateTime dateFrom, int dayCount)
    {
        //var q = _context.TimeReports.Where(r => r.ReportDate >= dateFrom && r.ReportDate <= dateFrom.AddDays(dayCount)).ToListAsync(); 

        var query = _context.TimeReports.Where(r => r.ReportDate >= dateFrom && r.ReportDate <= dateFrom.AddDays(dayCount));
        if (userIdList?.Count() > 0) query = query.Where(r => userIdList.Contains(r.UserId));
        if (wiIdList?.Count() > 0) query = query.Where(r => wiIdList.Contains(r.WorkItemId));

        var list = await query.ToListAsync();

        return list;
    }
    public async Task<List<TimeReportDBModel>> GetTimeDBReports(IEnumerable<Guid> userIdList, IEnumerable<Guid> wiIdList, DateTime dateFrom, int dayCount)
    {        
        var list = await GetTimeReports(userIdList, wiIdList, dateFrom, dayCount);
        var users = list.Select(r => r.User).DistinctBy(r => r.UserId);
        var listret = new List<TimeReportDBModel>();
        foreach (var user in users)
        {
            var trModel = new TimeReportDBModel { DateFrom = dateFrom, DayCount = dayCount, User = user/*, Projects = new List<ProjectDBModel>() */};
            var projects = list.Where(u => u.UserId == user.UserId).Select(u => u.WorkItem.ProjectId).Distinct().ToList();

            foreach (var pId in projects)
            {
                var project = _context.Projects.FirstOrDefault(p => p.Id == pId);
                if (project != null)
                {
                    var wiList = list.Where(w => w.WorkItem.ProjectId == pId).ToList();
                    if (wiList.Count() > 0)
                    {
                        //var pmModels = new List<ProjectDBModel>() { new ProjectDBModel() { WorkItems = wiList, Project = project } };
                        trModel.Projects.Add(new ProjectDBModel() { WorkItems = wiList, Project = project }); // = //pmModels;
                    }
                }
                if(trModel.Projects.Count > 0)
                    listret.Add(trModel);
            }
        }
        return listret;
    }
    //public async Task<List<TimeReportModelNew>> GetTimeReports(List<TimeReportDBModel> list)
    //{
    //    var ret = list.Select(x => x).ProjectTo<TimeReportModelNew>(TimeReportModelNew.MapperConfig);
    //}
    public async Task<List<WorkItemInfoBase>> GetWIList(Guid uId, DateTime dateFrom, int dayCount)
    {

        var query = _context.WorkItemStates.Where(r => r.EndDate >= dateFrom && r.StartDate <= dateFrom.AddDays(dayCount));
        //  После тестов переделать на Automapper
        var list = await query.Select(w => new WorkItemInfoBase { WorkItemId =  w.WorkItemId, ProjectId = w.WorkItem.ProjectId }).ToListAsync();
        var list1 = await _context.TimeReports.Where(r => r.UserId == uId && r.ReportDate >= dateFrom && r.ReportDate <= dateFrom.AddDays(dayCount))
            .Select(w => new WorkItemInfoBase { WorkItemId = w.WorkItemId, ProjectId = w.WorkItem.ProjectId })
            .ToListAsync();
        list.AddRange(list1);

        return list.Distinct().ToList();
    }
    /// <summary>
    /// Retuns a list of WI, wich are actie in period
    /// </summary>
    /// <param name="wiIdList">WI Ids</param>
    /// <param name="dateFrom">start date</param>
    /// <param name="dayCount">count of days</param>
    /// <returns></returns>
    public async Task<List<Guid>> GetWIList(IEnumerable<Guid> wiIdList, DateTime dateFrom, int dayCount)
    {
        var query = _context.WorkItemStates.Where(r => r.EndDate >= dateFrom && r.StartDate <= dateFrom.AddDays(dayCount));
        if (wiIdList?.Count() > 0) query = query.Where(r => wiIdList.Contains(r.WorkItemId));

        var list = await query.Select(w => w.Id).ToListAsync();

        return list;
    }
    /// <summary>
    /// Retuns a list of WI, wich are actie in period for one user
    /// </summary>
    /// <param name="uId">user's Id</param>
    /// <param name="dateFrom">start date</param>
    /// <param name="dayCount">count of days</param>
    /// <returns>WI Ids</returns>
    public async Task<List<Guid>> GetUserWIList(Guid uId, DateTime dateFrom, int dayCount)
    {
        var query = _context.WorkItemStates.Where(r => r.UserId == uId && r.EndDate >= dateFrom && r.StartDate <= dateFrom.AddDays(dayCount));
        //if (wiIdList?.Count() > 0) query = query.Where(r => wiIdList.Contains(r.WorkItemId));

        var list = await query.Select(w => w.Id).ToListAsync();

        return list;
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
            //var test = _context.TimeReports.ToList();
            var query = _context.TimeReports.Where(r => r.WorkItemId == wId && r.UserId == uId && r.ReportDate >= dateFrom && r.ReportDate <= dateFrom.AddDays(dayCount))
                   .Include(u => u.ReportStatus)/*.Collection(b => b.Posts)*/;
            //  Переделать после выяснения и доработки структуры БД ReportStatus? - может ли быть?
            //var lissst = await query.ToListAsync();
            var list = await query./*Select(w => w.Id).*/ProjectTo<DayInfoNew>(DayInfoNew.MapperConfig).ToListAsync();
            //var list = await query.Select(w => new WorkItemModelNew.DayInfo()
            //var list = lissst.Select(w => new /*WorkItemModelNew.*/DayInfoNew()
            //{ Id = w.Id, BillHours = w.BillHours, Hours = w.Hours, IsOvertime = w.Type == 1, ReportDate = w.ReportDate, ReportStatus = w.ReportStatus?.Status ?? ""} ).ToList();
            //    //.ToListAsync();
            wimodel.ItemTimes = list;
        }
        return wimodel;
    }
    public async Task<int> SetTimeReports(ITimeReportSetRequest setRequest, bool Transaction = true)
    {
        if(Transaction) await StartTransaction();
        var mapper = new Mapper(DayInfoSet.MapperConfig);
        if(setRequest.WorkInfos == null) return await Task.FromResult(0);
        foreach (var info in setRequest.WorkInfos)
        {
            DAL.EF.Model.TimeReport tr = mapper.Map<DAL.EF.Model.TimeReport>(info);
            DAL.EF.Model.TimeReportHistory trh = mapper.Map<DAL.EF.Model.TimeReportHistory>(info);
            trh.UpdateUserId = setRequest.ReportUserId;
            trh.UpdateDate = DateTime.UtcNow;
            await ChangeTimeReport(tr);
            await AddTimeReportHistory(trh);
        }
        if (Transaction) await CommitTransaction();
        return await Task.FromResult(1);
    }
    private async Task StartTransaction()
    {
        await _context.Database.BeginTransactionAsync();
    }
    private async Task CommitTransaction()
    {
        await _context.Database.CommitTransactionAsync();
    }
    private async Task ChangeTimeReport(TimeReport tr)
    {
        var newtr = _context.TimeReports.FirstOrDefault(t => t.UserId == tr.UserId && t.ReportDate == tr.ReportDate && t.WorkItemId == tr.WorkItemId && t.Type == tr.Type);
        if(newtr == null)
        {
            newtr = new TimeReport
            {
                UserId = tr.UserId,
                ReportDate = tr.ReportDate,
                WorkItemId = tr.WorkItemId,
                Type = tr.Type,
            };
            _context.TimeReports.Add(newtr);
        }
        newtr.Hours = tr.Hours;
        newtr.BillHours = tr.BillHours;
        await _context.SaveChangesAsync();
    }
    private async Task AddTimeReportHistory(TimeReportHistory tr)
    {
        _context.TimeReportHistories.Add(tr);
        await _context.SaveChangesAsync();
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