using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chrono.Services.Models;
using Chrono.Services.Repositories;

namespace Chrono.Services.Services;

public class TimeReportService : ITimeReportService
{
    private ITimeReportRepository timeReportGetRepository;
    private readonly IDataGetRepository dataGetRepository;
    private string[] statusEdit = new[] { "Edited", "Declined" };
    private string[] statusApprove = new[] { "WaitingForApprove" };
    private string[] statusView = new[] { "WaitingForApprove" };
    private string[] statusBill = new[] { "Approved" };
//    (0, 'Edited'),
//(1, 'WaitingForApprove'),
//(2, 'Declined'),
//(3, 'Approved'),
//(4, 'Billed');


    public TimeReportService(ITimeReportRepository timeReportGetRepository, IDataGetRepository dataGetRepository)
    {
        this.timeReportGetRepository = timeReportGetRepository;
        this.dataGetRepository = dataGetRepository;
    }


    public async Task<List<TimeReportModelNew>> GetTimeReportsNew(IEnumerable<Guid> userIdList, IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount)
    {
        List<Guid> ulist = new List<Guid>();
        List<Guid> plist = new List<Guid>();
        var listret = new List<TimeReportModelNew>();
        //var q = _context.TimeReports.Where(r => r.ReportDate >= dateFrom && r.ReportDate <= dateFrom.AddDays(dayCount)).ToListAsync(); 
        if (userIdList != null && userIdList.Count() > 0)
        {
            ulist.AddRange(userIdList);
            //plist = 
        }
        else
        {
            //ulist = 
            plist.AddRange(projectIdList);
        }
        foreach (var userId in ulist)
        {
            var uModel = await dataGetRepository.ReadUserById(userId);
            if(uModel != null)
            {
                var trModel = new TimeReportModelNew { DateFrom = dateFrom, DayCount = dayCount, User = uModel};
                //  Взять список собственных проектов для конкретного пользователя и пересечь его со списком присланных
                foreach(var prId in plist)
                {
                    IProjectInfoSimple? projectS = await dataGetRepository.ReadProjectById(prId);
                    IProjectModelNew? project = projectS != null ? new ProjectModelNew(projectS) : null;
                    if (project != null)
                    {
                        //  Перечень WI надо объеденить из списка активных из общей таблицы плюс из ReportTable по зафиксированной отчётности
                        var wiList = await timeReportGetRepository.GetUserWIList(userId, dateFrom, dayCount);  //  Потом добавить из ReportTable 

                        foreach (var id in wiList)
                        {
                            var tr = await timeReportGetRepository.GetUserTimeReport(userId, id, dateFrom, dayCount);
                            if(tr != null) project.WorkItems.Add(tr);
                        }
                    }
                }
            }
        }

     
        return listret;
    }
    public async Task<TimeReportModelNew> GetTimeReportN(Guid userId, DateTime dateFrom, int dayCount)
    {
        return await GetTimeReportAll(userId, dateFrom, dayCount, MakeForEdit);
    }
    private void MakeForEdit(IEnumerable<DayInfoNew> dayInfos)
    {
        foreach (DayInfoNew dayInfo in dayInfos)
        {
            dayInfo.IsEditable = statusEdit.Contains(dayInfo.ReportStatus);
        }
    }
    private void MakeForApprove(IEnumerable<DayInfoNew> dayInfos)
    {
        foreach (DayInfoNew dayInfo in dayInfos)
        {
            dayInfo.IsEditable = statusApprove.Contains(dayInfo.ReportStatus);
        }
    }
    private async Task<TimeReportModelNew> GetTimeReportAll(Guid userId, DateTime dateFrom, int dayCount, Action<IEnumerable<DayInfoNew>> action)
    {
        var user = await dataGetRepository.ReadUserById(userId);
        TimeReportModelNew modelNew = new TimeReportModelNew
        {
            DateFrom = dateFrom,
            DayCount = dayCount, 
            User = user,
        };
        if (user == null)
        {
            return modelNew;
        }
        var wiList = await timeReportGetRepository.GetWIList(userId, dateFrom, dayCount);
        var prList = wiList.Select(w => w.ProjectId).Distinct().ToList();

        
        foreach(var prId in prList)
        {
            //ProjectModelNew? //project = new ProjectModelNew();
            //        project = (ProjectModelNew?)await dataGetRepository.ReadProjectById(prId);
            IProjectInfoSimple? projectS = await dataGetRepository.ReadProjectById(prId);
            IProjectModelNew? project = projectS != null ? new ProjectModelNew(projectS) : null;
            if (project == null)
                continue;

            var wIdList = wiList.Where(w => w.ProjectId == prId).Select(x => x.WorkItemId).Distinct().ToList();
            foreach (var wi in wIdList)
                //foreach (var wi in wiList.Where(w => w.ProjectId == prId))
            {
                
                //modelNew.Projects.(ProjectModelNew?)await dataGetRepository.ReadProjectById(prId);
                var wiInfo = await timeReportGetRepository.GetUserTimeReport(userId, wi, dateFrom, dayCount);
                if (wiInfo != null)
                {
                    //  ++Продумать, как сюда поместить не только для Edit, но и для Approve. Вся выборка данных у них одинакова
                    if (wiInfo.ItemTimes?.Count > 0) action(wiInfo.ItemTimes);
                    //if (wiInfo.ItemTimes?.Count > 0) MakeForEdit(wiInfo.ItemTimes);
                    // получить даты для wiInfo и заполнить ItemTimes в соответствии с ... Если не хватает - добавить, если что-то лишнее - не трогать
                    
                    project.WorkItems.Add(wiInfo);
                }
                
            }
            if(project != null /*&& project.WorkItems.Count > 0*/) // Пустой список имеет право на существование. Его могут только начать заполнять
                modelNew.Projects.Add(project);
        }
        return modelNew;
    }
    /// <summary>
    ///     3.3.1.1 Запрос и получение информации для отображения
    /// </summary>
    /// <param name="userId">User's ID</param>
    /// <param name="dateFrom">Dtginning dste</param>
    /// <param name="dayCount">Count of days</param>
    /// <returns></returns>
    public async Task<TimeReportModel> GetTimeReport(Guid userId, DateTime dateFrom, int dayCount)
    {
        // Тестово для демонстрации структуры ответа

        //var items = new List<WorkItemTime>();
        var items = new List<DayInfo>();

        for (var i = 0; i < dayCount; i++)
            items.Add(new DayInfo
            {
                ComplitedOver = new WorkInfo { BillHours = 1, Hours = 2, IsEditable = true, ReportStatus = "", },
                ComplitedWork = new WorkInfo { BillHours = 8, Hours = 8, IsEditable = true, ReportStatus = "+", },
            });

        var model = new WorkItemModel { Name = "Test", Estimate = 1, ItemTimes = items, };
        return await Task.FromResult(new TimeReportModel
            {
                User = /*await dataGetRepository.ReadUserInfoAsync(userId), */ new UserBaseInfo { UserId = userId, },
                DateFrom = dateFrom,
                DayCount = dayCount,
                Projects = new List<ProjectModel>
                {
                    new() { Name = "Test", WorkItems = new List<WorkItemModel> { model, }, },
                },
            }
        );
    }

    public async Task<int> SetTimeReports(ITimeReportSetRequest setRequest)
    {
        var user = await dataGetRepository.ReadUserById(setRequest.ReportUserId);
        if (user == null) throw new Exception($"User with Id {setRequest.ReportUserId} not found");
        return await timeReportGetRepository.SetTimeReports(setRequest);
            //Task.FromResult(1);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectIdList">List of Ids of projects for wich gets a time reports</param>
    /// <param name="dateFrom">Dtginning dste</param>
    /// <param name="dayCount">Count of days</param>
    /// <param name="addDeleted"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByProjectsForApprove(IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount, bool addDeleted = false)
    {
        return await GetTimeReportsByProjectsForAll(projectIdList, dateFrom, dayCount, MakeForApprove, addDeleted);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectIdList">List of Ids of projects for wich gets a time reports</param>
    /// <param name="dateFrom">Dtginning dste</param>
    /// <param name="dayCount">Count of days</param>
    /// <param name="addDeleted"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByProjectsForView(IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount, bool addDeleted = false)
    {
        return await GetTimeReportsByProjectsForAll(projectIdList, dateFrom, dayCount, MakeForEdit, addDeleted);
    }
    private async Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByProjectsForAll(IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount, Action<IEnumerable<DayInfoNew>> action, bool addDeleted = false)
    {
        var userIdList = (await dataGetRepository.GetUsersByProjects(projectIdList, addDeleted ))?.Select(u => u.UserId).Distinct().ToList();
        var list = new List<TimeReportModelNew>();
        if(userIdList != null && userIdList.Any())
        {
            foreach(var userId in userIdList)
            {
                var trm = await GetTimeReportN(userId, dateFrom, dayCount);
                if (trm != null) list.Add(trm);
            }
        }
        return list;
        //return await GetTimeReports(userIdList.Select(u => u.UserId).ToList(), dateFrom, dayCount);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIdList">List of Ids of users for wich gets a time reports</param>
    /// <param name="dateFrom">Dtginning dste</param>
    /// <param name="dayCount">Count of days</param>
    /// <param name="addDeleted"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByUsertsForApprove(IEnumerable<Guid> userIdList, DateTime dateFrom, int dayCount, bool addDeleted = false)
    {
        return await GetTimeReportsByUsertsForAll(userIdList, dateFrom, dayCount, MakeForApprove, addDeleted);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIdList">List of Ids of users for wich gets a time reports</param>
    /// <param name="dateFrom">Dtginning dste</param>
    /// <param name="dayCount">Count of days</param>
    /// <param name="addDeleted"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByUsertsForView(IEnumerable<Guid> userIdList, DateTime dateFrom, int dayCount, bool addDeleted = false)
    {
        return await GetTimeReportsByUsertsForAll(userIdList, dateFrom, dayCount, MakeForEdit, addDeleted);
    }
    private async Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByUsertsForAll(IEnumerable<Guid> userIdList, DateTime dateFrom, int dayCount, Action<IEnumerable<DayInfoNew>> action, bool addDeleted = false)
    {
        //var userIdList = (await dataGetRepository.GetUsersByProjects(userIdList, addDeleted))?.Select(u => u.UserId).Distinct().ToList();
        var list = new List<TimeReportModelNew>();
        if (userIdList != null && userIdList.Any())
        {
            foreach (var userId in userIdList)
            {
                var trm = await GetTimeReportAll(userId, dateFrom, dayCount, action);
                if (trm != null) list.Add(trm);
            }
        }
        return list;
        //return await GetTimeReports(userIdList.Select(u => u.UserId).ToList(), dateFrom, dayCount);
    }
    public async Task<IEnumerable<TimeReportModel>> GetTimeReports(IEnumerable<Guid> userIdList, DateTime dateFrom, int dayCount)
    {
        // Для фактического результата надо будет вызвать функцию из репозитория и потом переработать данные из БД.

        // Тестово для демонстрации структуры ответа
        var list = new List<TimeReportModel>();
        var items = new List<DayInfo>();
        for (var i = 0; i < dayCount; i++)
            items.Add(new DayInfo
            {
                ComplitedOver = new WorkInfo { BillHours = 1, Hours = 2, IsEditable = true, ReportStatus = "", },
                ComplitedWork = new WorkInfo { BillHours = 8, Hours = 8, IsEditable = true, ReportStatus = "+", },
            });
        foreach (var userId in userIdList)
        {
            var model = new WorkItemModel { Name = "Test", Estimate = 1, ItemTimes = items, };
            list.Add(new TimeReportModel
            {
                User = new UserBaseInfo { UserId = userId, },
                DateFrom = dateFrom,
                DayCount = dayCount,
                Projects = new List<ProjectModel>
                {
                    new() { Name = "Test", WorkItems = new List<WorkItemModel> { model, }, },
                },
            });
        }

        ;
        return await Task.FromResult(list);
    }

    public async Task<IEnumerable<TimeReportModel>> GetTimeReportsByProjects(IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount)
    {
        // Для фактического результата надо будет вызвать функцию из репозитория и потом переработать данные из БД.
        //var userIdList = await timeReportGetRepository.GetUsersByProjectsAsync(projectIdList) ?? new List<DAL.EF.Model.User>();
        //return await GetTimeReports(userIdList.Select(u => u.UserId).ToList(), dateFrom, dayCount);
        // Тестово для демонстрации структуры ответа
        var list = new List<TimeReportModel>();
        var items = new List<DayInfo>();
        for (var i = 0; i < dayCount; i++)
            items.Add(new DayInfo
            {
                ComplitedOver = new WorkInfo { BillHours = 1, Hours = 2, IsEditable = true, ReportStatus = "", },
                ComplitedWork = new WorkInfo { BillHours = 8, Hours = 8, IsEditable = true, ReportStatus = "+", },
            });
        foreach (var projectId in projectIdList)
        {
            var model = new WorkItemModel { Name = "Test", Estimate = 1, ItemTimes = items, };
            list.Add(new TimeReportModel
            {
                User = new UserBaseInfo { UserId = Guid.NewGuid(), },
                DateFrom = dateFrom,
                DayCount = dayCount,
                Projects = new List<ProjectModel>
                {
                    new() { Name = "Test", Id = projectId, WorkItems = new List<WorkItemModel> { model, }, },
                },
            });
        }

        ;
        return await Task.FromResult(list);
    }
}