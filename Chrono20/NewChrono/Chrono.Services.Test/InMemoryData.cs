using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Chrono.DAL.EF.Model;

using Microsoft.EntityFrameworkCore;

namespace Chrono.Services.Test;

public static class InMemoryData
{
    public static ChronoContext GetTestContext(string alias)
    {
        var builder = new DbContextOptionsBuilder<ChronoContext>();

        builder.UseInMemoryDatabase(alias /*$"{Guid.NewGuid()}"*/); // каждому тесту своя база

        var context = new ChronoContext(builder.Options);

        context.SaveChanges();

        return context;
    }

    public static async Task<User> WriteUserAsync(ChronoContext context, User? user = null)
    {
        if (user == null)
            user = new User
            {
                UserId = Guid.NewGuid(),
                DisplayName = "Test",
                Location = await GetDefaultLocationAsync(context),
                Position = await GetDefaultPositionAsync(context),
                LocationFrom = DateTime.UtcNow,
                Sid = Guid.NewGuid().ToString(),
                Deleted = false,
            };
        var userold = await context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
        if (userold != null)
            userold = user;
        else
            await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public static async Task<User?> ReadUserAsync(ChronoContext context, Guid num)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await context.Users.FirstOrDefaultAsync(u => u.UserId == num);
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<Location> GetDefaultLocationAsync(ChronoContext context)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var location = await context.Locations.FirstOrDefaultAsync();
        if (location == null)
        {
            location = new Location
            {
                Name = "Test " + DateTime.UtcNow,
                Type = 1,
            };
            await context.Locations.AddAsync(location);
            await context.SaveChangesAsync();
        }

        return location;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<Position> GetDefaultPositionAsync(ChronoContext context)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var position = await context.Positions.FirstOrDefaultAsync();
        if (position == null)
        {
            position = new Position
            {
                Name = "Test " + DateTime.UtcNow,
            };
            await context.Positions.AddAsync(position);
            await context.SaveChangesAsync();
        }

        return position;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<ProjectCollection> GetDefaultProjectCollectionAsync(ChronoContext context)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var temp = await context.ProjectCollections.FirstOrDefaultAsync();
        if (temp == null)
        {
            temp = new ProjectCollection
            {
                Name = "Test " + DateTime.UtcNow,
            };
            await context.ProjectCollections.AddAsync(temp);
            await context.SaveChangesAsync();
        }

        return temp;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<Project> GetDefaultProjectAsync(ChronoContext context)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var temp = await context.Projects.FirstOrDefaultAsync();
        if (temp == null)
        {
            temp = new Project
            {
                Uri = "Test " + Guid.NewGuid(),
                Name = "Test " + DateTime.UtcNow,
                ProjectCollectionId = (await GetDefaultProjectCollectionAsync(context)).Id,
                OutId = Guid.NewGuid().ToString(),
                PmsystemId = (await GetDefaultPmsystemAsync(context)).Id,
                IsDeleted = false,
            };
            await context.Projects.AddAsync(temp);
            await context.SaveChangesAsync();
        }

        return temp;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<Pmsystem> GetDefaultPmsystemAsync(ChronoContext context)
    {
        var temp = await context.Pmsystems.FirstOrDefaultAsync();
        if (temp == null)
        {
            temp = new Pmsystem
            {
                Name = "Test " + DateTime.UtcNow,
            };
            await context.Pmsystems.AddAsync(temp);
            await context.SaveChangesAsync();
        }

        return temp;
    }

    public static async Task<Position> GetPositionByNameAsync(ChronoContext context, string name)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var position = await context.Positions.FirstOrDefaultAsync(x => x.Name == name);
        if (position == null)
        {
            position = new Position
            {
                Name = name,
            };
            await context.Positions.AddAsync(position);
            await context.SaveChangesAsync();
        }

        return position;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<Country> GetCountryByNameAsync(ChronoContext context, string name)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var country = await context.Countries.FirstOrDefaultAsync(x => x.Name == name);
        if (country == null)
        {
            country = new Country
            {
                Name = name,
            };
            await context.Countries.AddAsync(country);
            await context.SaveChangesAsync();
        }

        return country;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<Department> GetDepartmentByNameAsync(ChronoContext context, string name)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var department = await context.Departments.FirstOrDefaultAsync(x => x.Name == name);
        if (department == null)
        {
            department = new Department
            {
                Name = name,
            };
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();
        }

        return department;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task<ReportStatus> GetReportStatusByNameAsync(ChronoContext context, string name)
    {
#pragma warning disable CS8603 // Possible null reference return.
        //var status = await context.ReportStatuses.Where(x => x.Status == name).FirstOrDefaultAsync();
        var status = await context.ReportStatuses.FirstOrDefaultAsync(x => x.Status == name);
        if (status == null)
        {
            status = new ReportStatus
            {
                Status = name,
            };
            await context.ReportStatuses.AddAsync(status);
            await context.SaveChangesAsync();
        }

        return status;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static async Task InitDictionary(ChronoContext context)
    {
        var dictionary = new List<string>();
        dictionary.AddRange(new[] { "Country1", "Country2", "Country3", "Country4", });
        foreach (var name in dictionary) await GetCountryByNameAsync(context, name);
        dictionary.Clear();
        dictionary.AddRange(new[] { "IT Infrastructure Services", ".Net", "QA", "Ruby", });
        foreach (var name in dictionary) await GetDepartmentByNameAsync(context, name);
        dictionary.Clear();
        dictionary.AddRange(new[] { ".Net Developer", ".Net Team Lead", "Account Assistant", "Account Manager", });
        foreach (var name in dictionary) await GetPositionByNameAsync(context, name);
        dictionary.Clear();
        dictionary.AddRange(new[] { "Edited", "WaitingForApprove", "Declined", "Approved", "Billed", });
        foreach (var name in dictionary) await GetReportStatusByNameAsync(context, name);
        await GetDefaultLocationAsync(context);
    }
    //        public static async Task<T?> ReadFirstOrCreateAsync(ChronoContext context, Guid num) where T : class
    //        {
    //#pragma warning disable CS8603 // Possible null reference return.
    //            return await context.Users.FirstOrDefaultAsync(u => u.UserId == num);
    //#pragma warning restore CS8603 // Possible null reference return.
    //        }
}