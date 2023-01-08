using System;

using Chrono.DAL.EF.Model;
using Chrono.Services.Models;
using Chrono.Services.Repositories;
using Chrono.Services.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    var connectionString = builder.Configuration.GetConnectionString("TimeTrackDbConnection") ??
                           throw new Exception("Missed config value 'ConnectionStrings:TimeTrackDbConnection'");

    builder.Services.AddDbContext<ChronoContext>(options =>
        options.UseSqlServer(connectionString));


    builder.Services.AddControllers();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //
    builder.Services.AddAutoMapper(mapperConfiguration => { });
    //builder.Services.AddScoped<IChronoRepository, ChronoRepository>();
    builder.Services.AddScoped<IDataGetService, DataGetService>();
    builder.Services.AddScoped<IDataSaveService, DataSaveService>();
    builder.Services.AddScoped<IImportService, ImportService>();
    builder.Services.AddScoped<ITimeReportService, TimeReportService>();
    builder.Services.AddScoped<IDataGetRepository, DataGetRepository>();
    builder.Services.AddScoped<ITimeReportRepository, TimeReportRepository>();

    builder.Services.AddScoped<ITimeReportGetRequest, TimeReportGetRequest>();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}