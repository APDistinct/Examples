using System;
using System.Threading.Tasks;

using Chrono.DAL.EF.Model;
using Chrono.Services.Enums;
using Chrono.Services.Models;
using Chrono.Services.Repositories;

namespace Chrono.Services.Test;

public class TimeReportRepositoryTests
{
    private static readonly string alias = Guid.NewGuid().ToString();
    private readonly ChronoContext context;
    private readonly ITimeReportRepository service;
    private readonly IDataGetRepository dataGetRepository;

    public TimeReportRepositoryTests()
    {
        context = InMemoryData.GetTestContext(alias);
        service = new TimeReportRepository(context); 
        dataGetRepository = new DataGetRepository(context, null);
    }

    [Fact]
    public async Task GetTimeReportsTest_AllOK()
    {
        var list = new List<WorkItem>();
        var project = await InMemoryData.GetDefaultProjectAsync(context);
        var user = await InMemoryData.WriteUserAsync(context);
        for (int i = 0; i < 3; i++)
        {
            WorkItemInfoIncomeInner info1 = new()
            {
                Estimate = 0,
                OutId = "1" + Guid.NewGuid(),
                ProjectId = project.Id,
                Name = "Test1 " + Guid.NewGuid(),
                //WorkItemTypeId
            };
            
            var ret1 = await dataGetRepository.WriteWorkItemAsync(info1);
            if(ret1 != null) list.Add(ret1);
        }
        var repList = new List<TimeReport>();
        //DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
        DateTime date = DateTime.UtcNow;
        for (int i = 0; i < 3; i++)
        {
            foreach(var item in list)
            {
                var rep = new TimeReport
                {
                    UserId = user.UserId,
                    WorkItemId = item.Id,
                    ReportDate = date.AddDays(-i),
                    Hours = 8,
                    BillHours = i,
                    Type = 0,
                };
                repList.Add(rep);
            }
        }
        await context.TimeReports.AddRangeAsync(repList);
        await context.SaveChangesAsync();
        var uids = new List<Guid> { user.UserId };
        var wids = list.Select(w => w.Id).ToList();  //new List<Guid> { project.Id };
        var ret = await service.GetTimeReports(uids, wids, date, 3);
        Assert.NotNull(ret);
        Assert.Equal(list.Count, ret.Count);
        ret = await service.GetTimeReports(uids, wids, date.AddDays(-1), 3);
        Assert.NotNull(ret);
        Assert.Equal(2*list.Count, ret.Count);
    }
    [Fact]
    public async Task GetUserTimeReportTest_AllOK()
    {
        var list = new List<WorkItem>();
        var project = await InMemoryData.GetDefaultProjectAsync(context);
        var user = await InMemoryData.WriteUserAsync(context);
        int daycount = 3;
        for (int i = 0; i < 3; i++)
        {
            WorkItemInfoIncomeInner info1 = new()
            {
                Estimate = 0,
                OutId = "1" + Guid.NewGuid(),
                ProjectId = project.Id,
                Name = "Test1 " + Guid.NewGuid(),
                //WorkItemTypeId
            };

            var ret1 = await dataGetRepository.WriteWorkItemAsync(info1);
            if (ret1 != null) list.Add(ret1);
        }
        var repList = new List<TimeReport>();
        //DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
        DateTime date = DateTime.UtcNow;
        for (int i = 0; i < daycount; i++)
        {
            foreach (var item in list)
            {
                var rep = new TimeReport
                {
                    UserId = user.UserId,
                    WorkItemId = item.Id,
                    ReportDate = date.AddDays(-i),
                    Hours = 8,
                    BillHours = i,
                    Type = 0,
                };
                repList.Add(rep);
            }
        }
        await context.TimeReports.AddRangeAsync(repList);
        await context.SaveChangesAsync();

        foreach (var item in list)
        {
            var rett = await service.GetUserTimeReport(user.UserId, item.Id, date, 3);
            Assert.NotNull(rett);
        }
        var wi = list.First();
        var rettt = await service.GetUserTimeReport(user.UserId, wi.Id, date.AddDays(-daycount), 3);
        Assert.NotNull(rettt);
        Assert.NotNull(rettt.ItemTimes);
        Assert.Equal(rettt.ItemTimes.Count, daycount);
    }
    [Fact]
    public async Task SetTimeReportsTest_AllOK()
    {
        var list = new List<WorkItem>();
        var project = await InMemoryData.GetDefaultProjectAsync(context);
        var user = await InMemoryData.WriteUserAsync(context);
        var user2 = await InMemoryData.WriteUserAsync(context);
        for (int i = 0; i < 3; i++)
        {
            WorkItemInfoIncomeInner info1 = new()
            {
                Estimate = 0,
                OutId = "1" + Guid.NewGuid(),
                ProjectId = project.Id,
                Name = "Test1 " + Guid.NewGuid(),
                //WorkItemTypeId
            };

            var ret1 = await dataGetRepository.WriteWorkItemAsync(info1);
            if (ret1 != null) list.Add(ret1);
        }
        //var wiarr = list.Select(l => l.Id).ToArray();

        var repList = new List<TimeReport>();
        DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
        DateTime repdate = date.ToDateTime(TimeOnly.Parse("0:0:0")); // DateTime.UtcNow;
        var workInfos = new List<DayInfoSet>();
        
        for (int i = 0; i < 3; i++)
        {
            foreach (var item in list)
            {
                var rep = new DayInfoSet
                {
                    UserId = user.UserId,
                    WorkItemId = item.Id,
                    ReportDate = repdate.AddDays(-i),
                    Hours = 8,
                    BillHours = i,
                    IsOvertime = false,
                };
                workInfos.Add(rep);
            }
        }
        TimeReportSetRequest setRequest = new()
        {
            ReportUserId = user2.UserId,
            WorkInfos = workInfos,
        };

        var ret = await service.SetTimeReports(setRequest, false);
        foreach (var tr in workInfos)
        {
            var newtr = context.TimeReports
                .FirstOrDefault(t => t.UserId == tr.UserId && t.ReportDate == tr.ReportDate && t.WorkItemId == tr.WorkItemId && t.Type == (tr.IsOvertime ? (int)ETimeReportType.Overtime : (int)ETimeReportType.WorkTime));
            Assert.NotNull(newtr);
            Assert.Equal(newtr.Hours, tr.Hours);
            Assert.Equal(newtr.BillHours, tr.BillHours);
            var newtrh = context.TimeReportHistories
                .FirstOrDefault(t => t.UserId == tr.UserId && t.ReportDate == tr.ReportDate && t.WorkItemId == tr.WorkItemId && t.Type == (tr.IsOvertime ? (int)ETimeReportType.Overtime : (int)ETimeReportType.WorkTime));
            Assert.NotNull(newtrh);
            Assert.Equal(newtrh.Hours, tr.Hours);
            Assert.Equal(newtrh.BillHours, tr.BillHours);
        }
        ret = await service.SetTimeReports(setRequest, false);
        foreach (var tr in workInfos)
        {
            var newtrh = context.TimeReportHistories
                .Where(t => t.UserId == tr.UserId && t.ReportDate == tr.ReportDate && t.WorkItemId == tr.WorkItemId && t.Type == (tr.IsOvertime ? (int)ETimeReportType.Overtime : (int)ETimeReportType.WorkTime))
                .ToList();
            Assert.NotNull(newtrh);
            Assert.True(newtrh.Count() > 1);
        }
    }
    [Fact]
    public async Task GetTimeDBReportsTest_AllOK()
    {
        var list = new List<WorkItem>();
        var project = await InMemoryData.GetDefaultProjectAsync(context);
        var user = await InMemoryData.WriteUserAsync(context);
        for (int i = 0; i < 3; i++)
        {
            WorkItemInfoIncomeInner info1 = new()
            {
                Estimate = 0,
                OutId = "1" + Guid.NewGuid(),
                ProjectId = project.Id,
                Name = "Test1 " + Guid.NewGuid(),
                //WorkItemTypeId
            };

            var ret1 = await dataGetRepository.WriteWorkItemAsync(info1);
            if (ret1 != null) list.Add(ret1);
        }
        var repList = new List<TimeReport>();
        //DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
        DateTime date = DateTime.UtcNow;
        for (int i = 0; i < 3; i++)
        {
            foreach (var item in list)
            {
                var rep = new TimeReport
                {
                    UserId = user.UserId,
                    WorkItemId = item.Id,
                    ReportDate = date.AddDays(-i),
                    Hours = 8,
                    BillHours = i,
                    Type = 0,
                };
                repList.Add(rep);
            }
        }
        await context.TimeReports.AddRangeAsync(repList);
        await context.SaveChangesAsync();
        var uids = new List<Guid> { user.UserId };
        var wids = list.Select(w => w.Id).ToList();  //new List<Guid> { project.Id };
        var ret = await service.GetTimeDBReports(uids, wids, date, 3);
        Assert.NotNull(ret);
        //Assert.Equal(list.Count, ret.Count);
        ret = await service.GetTimeDBReports(uids, wids, date.AddDays(-1), 3);
        Assert.NotNull(ret);
        //Assert.Equal(2 * list.Count, ret.Count);
    }
}