using HST.Controllers.RemovalTools;
using HST.Controllers.SetService;
using HST.Controllers.DebloatApps;
using HST.Controllers.RegOptimizerMethods;
using HST.Controllers.DisableUpdate;
using HST.Controllers.TaskSchedulerOptimizerMethods;
using HST.Controllers.PowerPlan;
using HST.Controllers.Clear;
using HST.Controllers.Tool;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<RemovalHelpers>();
builder.Services.AddScoped<SetServices>();
builder.Services.AddScoped<Debloater>();
builder.Services.AddScoped<RegistryOptimizer>();
builder.Services.AddScoped<TaskSchedulerOptimizer>();
builder.Services.AddScoped<DisableWindowsUpdates>();
builder.Services.AddScoped<SetPowerPlan>();
builder.Services.AddScoped<CleanUp>();
builder.Services.AddScoped<SysInfo>();
builder.Services.AddScoped<RestorePointCreator>();

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();