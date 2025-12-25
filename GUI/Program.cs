using HST.Controllers.Debloat;
using HST.Controllers.Helpers;
using HST.Controllers.RegistryManager;
using HST.Controllers.Services;
using HST.Controllers.System;

var builder = WebApplication.CreateBuilder(args);

Logger.InitializeLog();

builder.Services.AddControllers();

// Helpers
builder.Services.AddScoped<ProcessRunner>();

// Services
builder.Services.AddScoped<SetServices>();
builder.Services.AddScoped<DisableWindowsUpdates>();

// Registry
builder.Services.AddScoped<RegistryOptimizer>();

// Debloat
builder.Services.AddScoped<Debloater>();

// System
builder.Services.AddScoped<TaskSchedulerOptimizer>();
builder.Services.AddScoped<SetPowerPlan>();
builder.Services.AddScoped<CleanUp>();
builder.Services.AddScoped<SysInfo>();
builder.Services.AddScoped<RestorePointCreator>();

builder.WebHost.UseUrls("http://localhost:5200");

try
{
    var app = builder.Build();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapControllers();
    app.MapFallbackToFile("index.html");
    Logger.Log("Starting HST WINDOWS UTILITY backend on port 5200");
    app.Run();
}
catch (Exception ex)
{
    Logger.Error("Application startup", ex);
    Environment.Exit(1);
}