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
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers
{
    [ApiController] // Enables API-specific behaviors like attribute routing and automatic 400 responses
    [Route("api/[controller]")] // Sets the base URL path
    public class SystemController : ControllerBase
    {
        // Fields marked 'private' are hidden from other classes
        // 'readonly' prevents reassignment after the constructor finishes
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

        // Constructor injection: The .NET IoC container resolves and passes these dependencies
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
            // Store the provided service instances into local fields for class-wide access
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

        // Verifies API availability and runtime status
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { status = "API online", timestamp = DateTime.Now });
        }

        // Retrieves system hardware and software information
        [HttpGet("sysinfo")]
        public async Task<IActionResult> GetSystemInfo()
        {
            var systemInfo = await _sysInfo.GetSystemInfoParallel();
            return Ok(systemInfo);
        }

        // Triggers creation of a Windows System Restore point for rollback safety
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
                Logger.Error("CreateRestorePoint", ex);
                return StatusCode(500, new
                {
                    status = "FAILED TO CREATE RESTORE POINT",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Applies registry tweaks targeting system performance and responsiveness
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
                Logger.Error("OptimizeRegistry", ex);
                return StatusCode(500, new
                {
                    status = "REGISTRY OPTIMIZATION FAILED",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Disables configured scheduled tasks to reduce background resource usage
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
                Logger.Error("OptimizeTaskScheduler", ex);
                return StatusCode(500, new
                {
                    status = "TASK SCHEDULER OPTIMIZATION HAS BEEN FAILED",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Disables Windows Update services to reduce bloat and prevent interference with optimizations
        [HttpPost("disable-updates")]
        public async Task<IActionResult> DisableUpdates()
        {
            try
            {
                await _disableWindowsUpdates.DisableWUpdatesAsync();
                return Ok(new
                {
                    status = "WINDOWS UPDATE HAS BEEN DISABLED",
                    message = "Windows update has been disabled",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Logger.Error("DisableUpdatesAsync", ex);
                return StatusCode(500, new
                {
                    status = "DISABLING WINDOWS UPDATE HAS BEEN FAILED",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Adjusts Windows visual effects to prioritize performance over aesthetics
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
                Logger.Error("LowerVisuals", ex);
                return StatusCode(500, new
                {
                    status = "LOWERING VISUALS HAS FAILED",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Enables system-wide Dark Mode
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
                Logger.Error("SetDarkMode", ex);
                return StatusCode(500, new
                {
                    status = "SETTING DARK MODE HAS FAILED",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Imports and activates the HST power plan for maximum performance and lowest latency/lag
        [HttpPost("set-powerplan")]
        public async Task<IActionResult> SetPowerPlan()
        {
            try
            {
                await _setPowerPlan.AddAndActivatePowerplanAsync();
                return Ok(new
                {
                    status = $"POWERPLAN HAS BEEN ADDED AND ACTIVATED",
                    message = $"powerplan has been set",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Logger.Error("SetPowerPlan", ex);
                return StatusCode(500, new
                {
                    status = "SETTING POWERPLAN HAS BEEN FAILED",
                    message = "Operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Removes specified applications (Steam, Whatsapp etc...) from startup to decrease boot time
        [HttpPost("remove-startup-apps")]
        public async Task<IActionResult> RemoveStartupApps()
        {
            var startupAppsToRemove = new List<string>();
            try
            {
                var config = await ConfigLoader.LoadStringConfigAsync("AppsConfig.json");
                startupAppsToRemove.AddRange(config["startupApps"]);
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveStartupApps config loading", ex);
                return StatusCode(500, new
                {
                    status = "CONFIG FILE ERROR",
                    message = $"Error reading configuration. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }

            try
            {
                await _debloater.RemoveStartupAppsAsync(startupAppsToRemove);
                return Ok(new
                {
                    status = "STARTUP APPS REMOVED",
                    message = "Startup apps have been removed",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveStartupApps operation", ex);
                return StatusCode(500, new
                {
                    status = "OPERATION FAILED",
                    message = "Startup app removal failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Removes pre-installed Windows bloatware (Edge, OneDrive, Store Apps) based on user's configuration
        [HttpPost("debloat-apps")]
        public async Task<IActionResult> DebloatApps([FromBody] DebloatOptions options = null)
        {
            if (options == null)
            {
                return StatusCode(400, new
                {
                    status = "NO OPTIONS PROVIDED",
                    message = "Please provide options",
                    success = false
                });
            }

            var packagesToRemove = new List<string>();
            try
            {
                var config = await ConfigLoader.LoadStringConfigAsync("AppsConfig.json");

                if (options.MsApps && config.ContainsKey("msApps"))
                    packagesToRemove.AddRange(config["msApps"]);
                if (options.XboxApps && config.ContainsKey("xboxApps"))
                    packagesToRemove.AddRange(config["xboxApps"]);
                if (options.StoreApps && config.ContainsKey("storeApps"))
                    packagesToRemove.AddRange(config["storeApps"]);
            }
            catch (Exception ex)
            {
                Logger.Error("DebloatApps config loading", ex);
                return StatusCode(500, new
                {
                    status = "CONFIG FILE ERROR",
                    message = $"Error reading configuration. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }

            try
            {
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
                    await _debloater.RemoveConfiguredPackagesAsync(packagesToRemove);
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

                return StatusCode(400, new
                {
                    status = "NO APPS SELECTED",
                    message = "Please select apps to remove",
                    success = false
                });
            }
            catch (Exception ex)
            {
                Logger.Error("DebloatApps operation", ex);
                return StatusCode(500, new
                {
                    status = "OPERATION FAILED",
                    message = "Debloat operation failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Executes system cleanup routines: temp files, caches, logs, and power plans that comes with Windows
        [HttpPost("cleanup")]
        public async Task<IActionResult> CleanUp([FromBody] CleanUpOptions? options = null)
        {
            try
            {
                if (options == null)
                {
                    return StatusCode(400, new
                    {
                        status = "PLEASE SELECT AN OPTION",
                        message = "Please select an option",
                        success = false
                    });
                }

                if (options.Temp)
                {
                    await _cleanUp.RemoveTempAsync();
                }

                if (options.Cache)
                {
                    await _cleanUp.CleanInternetCacheAsync();
                    await _cleanUp.ClearUpdateCacheAsync();
                }

                if (options.EventLog)
                {
                    await _cleanUp.ClearEventLogsAsync();
                }

                if (options.PowerPlan)
                {
                    await _cleanUp.ClearDefaultPowerPlansAsync();
                }

                await _cleanUp.ClearRecycleBinAsync();

                return Ok(new
                {
                    status = "CLEANUP IS DONE",
                    message = "Cleanup is done",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Logger.Error("CleanUp", ex);
                return StatusCode(500, new
                {
                    status = "CLEANUP HAS FAILED",
                    message = "Cleanup failed. Check HST-WINDOWS-UTILITY.log for details.",
                    success = false
                });
            }
        }

        // Disables selected Windows services based on user's configuration (Recommended, Bluetooth, Hyper-V, Xbox)
        [HttpPost("optimize-services")]
        public async Task<IActionResult> DisableServices([FromBody] ServiceOptions options = null)
        {
            if (options == null)
            {
                return StatusCode(400, new
                {
                    status = "NO OPTIONS PROVIDED",
                    message = "Please provide options",
                    success = false
                });
            }

            var servicesToDisable = new List<string>();

            try
            {
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");

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
                Logger.Error("DisableServices config loading", ex);
                return StatusCode(500, new
                {
                    status = "CONFIG FILE ERROR",
                    message = $"Error reading configuration: {ex.Message}",
                    success = false
                });
            }

            if (servicesToDisable.Any())
            {
                try
                {
                    await _setServices.DisableConfiguredServicesAsync(servicesToDisable);
                    return Ok(new
                    {
                        status = "SERVICES DISABLED SUCCESSFULLY",
                        message = "Services have been optimized",
                        success = true
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("DisableServices operation", ex);
                    return StatusCode(500, new
                    {
                        status = "OPERATION FAILED",
                        message = "Service modification failed. Check HST-WINDOWS-UTILITY.log for details.",
                        success = false
                    });
                }
            }
            else
            {
                return StatusCode(400, new
                {
                    status = "NO SERVICES SELECTED",
                    message = "Please select services to optimize",
                    success = false
                });
            }
        }

        // Restores system configurations (Services, Tasks, Updates, Registry) to their default states
        [HttpPost("revert-configurations")]
        public async Task<IActionResult> RevertConfigurations([FromBody] RevertOptions options = null)
        {
            if (options == null)
            {
                return StatusCode(400, new
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
                    await _setServices.DisableConfiguredServicesRevertAsync();
                    anyReverted = true;
                }
                catch (Exception ex)
                {
                    Logger.Error("ServiceRevert", ex);
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
                    Logger.Error("TaskSchedulerRevert", ex);
                }
            }

            if (options.WUpdate)
            {
                try
                {
                    await _disableWindowsUpdates.DisableWUpdatesRevertAsync();
                    anyReverted = true;
                }
                catch (Exception ex)
                {
                    Logger.Error("WindowsUpdateRevert", ex);
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
                    Logger.Error("RegistryRevert", ex);
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

            return StatusCode(400, new
            {
                status = "NO OPTIONS SELECTED",
                message = "Please select at least one option to revert",
                success = false
            });
        }
    }

    // Data transfer object for system information
    public class SystemInfoDto
    {
        public string User { get; set; } = "";
        public string Time { get; set; } = "";
        public string Gpu { get; set; } = "";
        public string Cpu { get; set; } = "";
        public string Ram { get; set; } = "";
        public string Storage { get; set; } = "";
    }

    // Data transfer object for service check-boxes
    public class ServiceOptions
    {
        public bool Recommended { get; set; }
        public bool Bluetooth { get; set; }
        public bool Hyperv { get; set; }
        public bool Xbox { get; set; }
    }

    // Data transfer object for debloat check-boxes
    public class DebloatOptions
    {
        public bool MsApps { get; set; }
        public bool Edge { get; set; }
        public bool Onedrive { get; set; }
        public bool XboxApps { get; set; }
        public bool StoreApps { get; set; }
    }

    // Data transfer object for cleanup check-boxes
    public class CleanUpOptions
    {
        public bool Temp { get; set; }
        public bool Cache { get; set; }
        public bool EventLog { get; set; }
        public bool PowerPlan { get; set; }
    }

    // Data transfer object for revertion check-boxes
    public class RevertOptions
    {
        public bool Service { get; set; }
        public bool Task { get; set; }
        public bool WUpdate { get; set; }
        public bool Registry { get; set; }
    }

}