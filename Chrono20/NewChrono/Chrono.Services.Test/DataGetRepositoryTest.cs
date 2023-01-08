using System;
using System.Linq;
using System.Threading.Tasks;

using Chrono.DAL.EF.Model;
using Chrono.Services.Models;
using Chrono.Services.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Chrono.Services.Test;

public class DataGetRepositoryTest
{
    private static readonly string alias = Guid.NewGuid().ToString();

    //private readonly string connectionString = "Server=APD\\APDSERVER;User Id=txchrono;Password=Tx12cHrono34;Database=TimeTrack2_0";
    private readonly ChronoContext context;
    private readonly IDataGetRepository service;

    public DataGetRepositoryTest()
    {
        //context = new ChronoContext(GetOptions(connectionString ));
        context = InMemoryData.GetTestContext(alias);

        service = new DataGetRepository(context, mapper: null);
    }

    private static DbContextOptions<ChronoContext> GetOptions(string connectionString)
    {
        return new DbContextOptionsBuilder<ChronoContext>().UseSqlServer(connectionString).Options;
    }

    [Fact /*(Skip = "Reason")*/]
    public void ReadUserTest_OK()
    {
        var userdef = context.Users.FirstOrDefault();
        if (userdef != null)
        {
            //DataGetRepository test = new DataGetRepository(new ChronoContext(GetOptions(connectionString)));
            var test = new DataGetRepository(InMemoryData.GetTestContext(alias), null);
            var num = userdef.UserId;
            var user = test.ReadUser(num);
            Assert.NotNull(user);
            Assert.Equal(user.UserId, num);
        }
    }

    [Fact]
    public async Task ReadUserAsyncTest_OK()
    {
        var userdef = //context.Users.FirstOrDefault();
            await InMemoryData.WriteUserAsync(context);
        if (userdef != null)
        {
            //DataGetRepository test = new DataGetRepository(new ChronoContext(GetOptions(connectionString)));
            var test = new DataGetRepository(InMemoryData.GetTestContext(alias), null);
            var num = userdef.UserId;
            var user = await test.ReadUserAsync(num);
            Assert.NotNull(user);
            Assert.Equal(user.UserId, num);
        }
    }

    [Fact]
    public async Task ReadUserAsyncTest_NO()
    {
        //var userdef = context.Users.FirstOrDefault();
        //if (userdef != null)
        {
            //DataGetRepository test = new DataGetRepository(new ChronoContext(GetOptions(connectionString)));
            var test = new DataGetRepository(InMemoryData.GetTestContext(alias), null);
            var num = Guid.NewGuid(); // userdef.UserId;
            var user = await test.ReadUserAsync(num);
            Assert.Null(user);
            //Assert.Equal(user.UserId, num);
        }
    }

    [Fact]
    public async Task WriteUserAsyncTest_OK()
    {
        var info1 = new UserInfoIncomeInner
        {
            Sid = Guid.NewGuid().ToString(),
            LocationFrom = DateTime.UtcNow,
            LocationId = (await InMemoryData.GetDefaultLocationAsync(context)).Id,
            Deleted = false,
            PositionId = (await InMemoryData.GetDefaultPositionAsync(context)).Id,
            DisplayName = "Test " + Guid.NewGuid().ToString(),
            Email = "Test " + Guid.NewGuid().ToString(),
            UserName = "Test " + Guid.NewGuid().ToString(),
        };

        var user1 = await service.WriteUserAsync(info1);
        Assert.NotNull(user1);
        Assert.Equal(user1.Sid, info1.Sid);
        Assert.Equal(user1.LocationFrom, info1.LocationFrom);
        Assert.Equal(user1.LocationId, info1.LocationId);
        Assert.Equal(user1.Deleted, info1.Deleted);
        Assert.Equal(user1.PositionId, info1.PositionId);
        Assert.Equal(user1.DisplayName, info1.DisplayName);
        Assert.Equal(user1.Email, info1.Email);
        Assert.Equal(user1.UserName, info1.UserName);

        var info2 = new UserInfoIncomeInner
        {
            Sid = info1.Sid,
            LocationFrom = DateTime.UtcNow,
            LocationId = info1.LocationId,
            Deleted = true,
            PositionId = info1.PositionId,
            DisplayName = "Test " + Guid.NewGuid().ToString(),
            Email = "Test " + Guid.NewGuid().ToString(),
            UserName = "Test " + Guid.NewGuid().ToString(),
        };

        var user2 = await service.WriteUserAsync(info2);
        Assert.NotNull(user2);
        Assert.Equal(user1.UserId, user2.UserId);
        Assert.Equal(user1.Sid, user2.Sid);
        //Assert.NotEqual(info1.UserName, user2.UserName);
        //Assert.NotEqual(info1.LocationFrom, user2.LocationFrom);
        Assert.NotEqual(info1.DisplayName, user2.DisplayName);
        Assert.NotEqual(info1.UserName, user2.UserName);
    }

    //[Fact]
    //public async Task WriteUserAsyncTest_NO()
    //{
    //}
    [Fact]
    public async Task WriteProjectAsyncTest_OK()
    {
        //var collection = 
        var info1 = new ProjectInfoIncomeInner
        {
            IsDeleted = false,
            Name = "Test " + Guid.NewGuid().ToString(),
            Uri = "Test " + Guid.NewGuid().ToString(),
            ProjectCollectionId = (await InMemoryData.GetDefaultProjectCollectionAsync(context)).Id,
        };
        var test1 = await service.WriteProjectAsync(info1);
        Assert.NotNull(test1);
        Assert.Equal(test1.IsDeleted, info1.IsDeleted);
        Assert.Equal(test1.ProjectCollectionId, info1.ProjectCollectionId);
        Assert.Equal(test1.Name, info1.Name);
        Assert.Equal(test1.Uri, info1.Uri);
        var info2 = new ProjectInfoIncomeInner
        {
            IsDeleted = false,
            Name = "Test2 " + Guid.NewGuid().ToString(),
            Uri = "Test2 " + Guid.NewGuid().ToString(),
            ProjectCollectionId = (await InMemoryData.GetDefaultProjectCollectionAsync(context)).Id,
        };
        var test2 = await service.WriteProjectAsync(info2);
        Assert.NotNull(test2);
        Assert.Equal(test2.IsDeleted, info2.IsDeleted);
        Assert.Equal(test2.ProjectCollectionId, info2.ProjectCollectionId);
        Assert.Equal(test2.Name, info2.Name);
        Assert.Equal(test2.Uri, info2.Uri);
        Assert.NotEqual(info1.Uri, test2.Uri);
        Assert.NotEqual(info1.Name, test2.Name);
    }

    [Fact]
    public async Task WriteWorkItemAsyncTest_OK()
    {
        //var collection = 
        var info1 = new WorkItemInfoIncomeInner
        {
            ProjectId = (await InMemoryData.GetDefaultProjectAsync(context)).Id,
            Estimate = 1,
            Name = "Test " + Guid.NewGuid().ToString(),
            OutId = "11",
        };
        var test1 = await service.WriteWorkItemAsync(info1);
        Assert.NotNull(test1);
        Assert.Equal(test1.Estimate, info1.Estimate);
        Assert.Equal(test1.ProjectId, info1.ProjectId);
        Assert.Equal(test1.Name, info1.Name);
        Assert.Equal(test1.OutId, info1.OutId);
        var info2 = new WorkItemInfoIncomeInner
        {
            ProjectId = (await InMemoryData.GetDefaultProjectAsync(context)).Id,
            Estimate = 11,
            Name = "Test2 " + Guid.NewGuid().ToString(),
            OutId = "110",
        };
        var test2 = await service.WriteWorkItemAsync(info2);
        Assert.NotNull(test2);
        Assert.Equal(test2.Estimate, info2.Estimate);
        Assert.Equal(test2.ProjectId, info2.ProjectId);
        Assert.Equal(test2.Name, info2.Name);
        Assert.Equal(test2.OutId, info2.OutId);
        //Assert.NotEqual(info1.ItemId, test2.ItemId);
        Assert.NotEqual(info1.Name, test2.Name);
        Assert.NotEqual(info1.Estimate, test2.Estimate);
    }
    [Fact]
    public async Task ReadUserBySidTest()
    {
        //var userdef = context.Users.FirstOrDefault();
        //if (userdef != null)
        {

            //DataGetRepository test = new DataGetRepository(new ChronoContext(GetOptions(connectionString)));
            var test = new DataGetRepository(InMemoryData.GetTestContext(alias), null);
            var uu = await InMemoryData.WriteUserAsync(context);
            var num = uu.Sid; // userdef.UserId;
            var user = await test.ReadUserBySid(num);
            Assert.NotNull(user);
            //Assert.Equal(user.UserId, num);
        }
    }
    [Fact]
    public async Task ReadProjectByIdTest()
    {
        //var userdef = context.Users.FirstOrDefault();
        //if (userdef != null)
        {

            //DataGetRepository test = new DataGetRepository(new ChronoContext(GetOptions(connectionString)));
            var test = new DataGetRepository(InMemoryData.GetTestContext(alias), null);
            var uu = await InMemoryData.GetDefaultProjectAsync(context);
            var num = uu.Id; // userdef.UserId;
            var project = await test.ReadProjectById(num);
            Assert.NotNull(project);
            num = Guid.NewGuid(); // userdef.UserId;
            project = await test.ReadProjectById(num);
            Assert.Null(project);
            //Assert.Equal(user.UserId, num);
        }
    }
}