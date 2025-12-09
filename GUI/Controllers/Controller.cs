using HST.Controllers.DebloatApps;
using HST.Controllers.RegOptimizerMethods;
using HST.Controllers.RemovalTools;
using HST.Controllers.DisableUpdate;
using HST.Controllers.TaskSchedulerOptimizerMethods;
using HST.Controllers.SetService;
using HST.Controllers.PowerPlan;
using HST.Controllers.Clear;
using HST.Controllers.Tool;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

#pragma warning disable CA1416
#pragma warning disable CS8625
#pragma warning disable CS8602

namespace HST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly Debloater _debloater;
        private readonly RegistryOptimizer _regOptimizer;
        private readonly RemovalHelpers _removalHelpers;
        private readonly TaskSchedulerOptimizer _tsOptimizer;
        private readonly SetServices _setServices;
        private readonly DisableWindowsUpdates _disableWindowsUpdates;
        private readonly SetPowerPlan _setPowerPlan;
        private readonly CleanUp _cleanUp;
        private readonly SysInfo _sysInfo;
        private readonly RestorePointCreator _restorePointCreator;

        public SystemController(
            Debloater debloater,
            RegistryOptimizer regOptimizer,
            RemovalHelpers removalTools,
            TaskSchedulerOptimizer tsOptimizer,
            SetServices setServices,
            DisableWindowsUpdates disableWindowsUpdates,
            SetPowerPlan setPowerPlan,
            CleanUp cleanUp,
            SysInfo sysInfo,
            RestorePointCreator restorePointCreator)
        {
            _debloater = debloater;
            _regOptimizer = regOptimizer;
            _removalHelpers = removalTools;
            _tsOptimizer = tsOptimizer;
            _setServices = setServices;
            _disableWindowsUpdates = disableWindowsUpdates;
            _setPowerPlan = setPowerPlan;
            _cleanUp = cleanUp;
            _sysInfo = sysInfo;
            _restorePointCreator = restorePointCreator;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { status = "API online", timestamp = DateTime.Now });
        }

        [HttpGet("sysinfo")]
        public async Task<IActionResult> GetSystemInfo()
        {
            var systemInfo = await _sysInfo.GetSystemInfoParallel();
            return Ok(systemInfo);
        }

        [HttpPost("restore-point")]
        public async Task<IActionResult> CreateRestorePoint()
        {
            try
            {
                await _restorePointCreator.CreateRestorePointAsync();
                return Ok(new
                {
                    status = "CREATED HST RESTORE POINT",
                    message = "restore point has been created",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during restore point creation: {ex.Message}");
                return Ok(new
                {
                    status = "FAILED TO CREATE RESTORE POINT",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("optimize-registry")]
        public async Task<IActionResult> OptimizeRegistry()
        {
            try
            {
                await _regOptimizer.OptimizeRegistryAsync();
                return Ok(new
                {
                    status = "REGISTRY OPTIMIZED SUCCESSFULLY",
                    message = "Registry settings have been optimized",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during registry optimization: {ex.Message}");
                return Ok(new
                {
                    status = "REGISTRY OPTIMIZATION FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("optimize-taskscheduler")]
        public async Task<IActionResult> OptimizeTaskScheduler()
        {
            try
            {
                await _tsOptimizer.DisableAllScheduledTasksAsync();
                return Ok(new
                {
                    status = "TASK SCHEDULER HAS BEEN OPTIMIZED",
                    message = "Task scheduler has been optimized",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during task scheduler optimization: {ex.Message}");
                return Ok(new
                {
                    status = "TASK SCHEDULER OPTIMIZATION HAS BEEN FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("disable-updates")]
        public async Task<IActionResult> DisableUpdates()
        {
            try
            {
                await _disableWindowsUpdates.DisableWUpdates();
                return Ok(new
                {
                    status = "WINDOWS UPDATE HAS BEEN DISABLED",
                    message = "Windows update has been disabled",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while disabling windows updates: {ex.Message}");
                return Ok(new
                {
                    status = "DISABLING WINDOWS UPDATE HAS BEEN FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("lower-visuals")]
        public async Task<IActionResult> LowerVisuals()
        {
            try
            {
                await _regOptimizer.LowerVisualsAsync();
                return Ok(new
                {
                    status = "LOWERED VISUALS",
                    message = "Visual settings have been optimized",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while modifying visual settings: {ex.Message}");
                return Ok(new
                {
                    status = "LOWERING VISUALS HAS FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("set-darkmode")]
        public async Task<IActionResult> SetDarkMode()
        {
            try
            {
                await _regOptimizer.SwitchDarkModeAsync();
                return Ok(new
                {
                    status = "DARK MODE HAS BEEN SET",
                    message = "Dark mode has been set",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while setting dark mode: {ex.Message}");
                return Ok(new
                {
                    status = "SETTING DARK MODE HAS FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("set-powerplan")]
        public async Task<IActionResult> SetPowerPlan()
        {
            try
            {
                await _setPowerPlan.AddAndActivatePP();
                return Ok(new
                {
                    status = $"POWERPLAN HAS BEEN ADDED AND ACTIVATED",
                    message = $"powerplan has been set",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                return Ok(new
                {
                    status = "SETTING POWERPLAN HAS BEEN FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("remove-startup-apps")]
        public async Task<IActionResult> RemoveStartupApps()
        {
            try
            {
                await _debloater.RemoveStartupAppsAsync();
                return Ok(new
                {
                    status = "STARTUP APPS REMOVED",
                    message = "Startup apps have been removed",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                return Ok(new
                {
                    status = "REMOVING STARTUP APPS FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }


        [HttpPost("debloat-apps")]
        public async Task<IActionResult> DebloatApps([FromBody] DebloatOptions options = null)
        {
            if (options == null)
            {
                return Ok(new
                {
                    status = "NO OPTIONS PROVIDED",
                    message = "Please provide options",
                    success = false
                });
            }

            var packagesToRemove = new List<string>();

            try
            {
                var json = await System.IO.File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppsConfig.json"));
                var config = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

                if (options.MsApps && config.ContainsKey("msApps"))
                    packagesToRemove.AddRange(config["msApps"]);
                if (options.XboxApps && config.ContainsKey("xboxApps"))
                    packagesToRemove.AddRange(config["xboxApps"]);
                if (options.StoreApps && config.ContainsKey("storeApps"))
                    packagesToRemove.AddRange(config["storeApps"]);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = "CONFIG FILE ERROR",
                    message = $"Error reading configuration: {ex.Message}",
                    success = false
                });
            }

            if (options.Edge)
            {
                await _debloater.RemoveEdgeAsync();
            }

            if (options.Onedrive)
            {
                await _debloater.RemoveOneDriveAsync();
            }

            if (packagesToRemove.Any())
            {
                await _debloater.RemoveConfiguredPackages(packagesToRemove);
            }

            bool anythingSelected = options.Edge || options.Onedrive || packagesToRemove.Any();

            if (anythingSelected)
            {
                return Ok(new
                {
                    status = "APPS REMOVED SUCCESSFULLY",
                    message = "Apps have been removed",
                    success = true
                });
            }

            return Ok(new
            {
                status = "NO APPS SELECTED",
                message = "Please select apps to remove",
                success = false
            });
        }


        [HttpPost("cleanup")]
        public async Task<IActionResult> CleanUp([FromBody] CleanUpOptions? options = null)
        {
            try
            {
                if (options == null)
                {
                    return Ok(new
                    {
                        status = "PLEASE SELECT AN OPTION",
                        message = "Please select an option",
                        success = false
                    });
                }

                if (options.Temp)
                {
                    await _cleanUp.RemoveTemp();
                }

                if (options.Cache)
                {
                    await _cleanUp.CleanInternetCache();
                    await _cleanUp.ClearUpdateCache();
                }

                if (options.EventLog)
                {
                    await _cleanUp.ClearEventLogs();
                }

                if (options.PowerPlan)
                {
                    await _cleanUp.ClearDefaultPowerPlans();
                }

                await _cleanUp.ClearRecycleBin();

                return Ok(new
                {
                    status = "CLEANUP IS DONE",
                    message = "Cleanup is done",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                return Ok(new
                {
                    status = "CLEANUP HAS FAILED",
                    error = ex.Message,
                    success = false
                });
            }
        }

        [HttpPost("optimize-services")]
        public async Task<IActionResult> DisableServices([FromBody] ServiceOptions options = null)
        {
            if (options == null)
            {
                return Ok(new
                {
                    status = "NO OPTIONS PROVIDED",
                    message = "Please provide options",
                    success = false
                });
            }

            var servicesToDisable = new List<string>();

            try
            {
                var json = await System.IO.File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServicesConfig.json"));

                var jsonOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var config = JsonSerializer.Deserialize<Dictionary<string, List<ServiceInfo>>>(json, jsonOptions);

                if (options.Recommended && config.ContainsKey("recommended"))
                    servicesToDisable.AddRange(config["recommended"].Select(s => s.service));
                if (options.Bluetooth && config.ContainsKey("bluetooth"))
                    servicesToDisable.AddRange(config["bluetooth"].Select(s => s.service));
                if (options.Hyperv && config.ContainsKey("hyperv"))
                    servicesToDisable.AddRange(config["hyperv"].Select(s => s.service));
                if (options.Xbox && config.ContainsKey("xbox"))
                    servicesToDisable.AddRange(config["xbox"].Select(s => s.service));
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = "CONFIG FILE ERROR",
                    message = $"Error reading configuration: {ex.Message}",
                    success = false
                });
            }

            if (servicesToDisable.Any())
            {
                await _setServices.DisableConfiguredServices(servicesToDisable);
                return Ok(new
                {
                    status = "SERVICES MODIFIED SUCCESSFULLY",
                    message = "Services have been optimized",
                    success = true
                });
            }
            else
            {
                return Ok(new
                {
                    status = "NO SERVICES SELECTED",
                    message = "Please select services to optimize",
                    success = false
                });
            }
        }

        [HttpPost("revert-configurations")]
        public async Task<IActionResult> RevertConfigurations([FromBody] RevertOptions options = null)
        {
            if (options == null)
            {
                return Ok(new
                {
                    status = "NO OPTIONS PROVIDED",
                    message = "Please provide options",
                    success = false
                });
            }

            bool anyReverted = false;

            if (options.Service)
            {
                try
                {
                    await _setServices.DisableConfiguredServicesRevert();
                    anyReverted = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR: {ex.Message}");
                }
            }

            if (options.Task)
            {
                try
                {
                    await _tsOptimizer.DisableAllScheduledTasksRevertAsync();
                    anyReverted = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred during task scheduler revert: {ex.Message}");
                }
            }

            if (options.WUpdate)
            {
                try
                {
                    await _disableWindowsUpdates.DisableWUpdatesRevert();
                    anyReverted = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred while reverting windows updates: {ex.Message}");
                }
            }

            if (options.Registry)
            {
                try
                {
                    await _regOptimizer.OptimizeRegistryRevertAsync();
                    anyReverted = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred during registry revertion: {ex.Message}");
                }
            }

            if (anyReverted)
            {
                return Ok(new
                {
                    status = "CONFIGURATIONS REVERTED",
                    message = "Selected configurations have been reverted",
                    success = true
                });
            }

            return Ok(new
            {
                status = "NO OPTIONS SELECTED",
                message = "Please select at least one option to revert",
                success = false
            });
        }
    }

    public class SystemInfoDto
    {
        public string User { get; set; } = "";
        public string Time { get; set; } = "";
        public string Gpu { get; set; } = "";
        public string Cpu { get; set; } = "";
        public string Ram { get; set; } = "";
        public string Storage { get; set; } = "";
    }

    public class ServiceOptions
    {
        public bool Recommended { get; set; }
        public bool Bluetooth { get; set; }
        public bool Hyperv { get; set; }
        public bool Xbox { get; set; }
    }

    public class DebloatOptions
    {
        public bool MsApps { get; set; }
        public bool Edge { get; set; }
        public bool Onedrive { get; set; }
        public bool XboxApps { get; set; }
        public bool StoreApps { get; set; }
    }

    public class CleanUpOptions
    {
        public bool Temp { get; set; }
        public bool Cache { get; set; }
        public bool EventLog { get; set; }
        public bool PowerPlan { get; set; }
    }

    public class RevertOptions
    {
        public bool Service { get; set; }
        public bool Task { get; set; }
        public bool WUpdate { get; set; }
        public bool Registry { get; set; }
    }

}