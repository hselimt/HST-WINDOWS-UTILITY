using HST.Controllers.Helpers;
using Microsoft.Win32;
using System.Text;

namespace HST.Controllers.Services
{
    public class DisableWindowsUpdates
    {
        private readonly ProcessRunner _processRunner;

        public DisableWindowsUpdates(ProcessRunner processRunner)
        {
            // Ensures process runner is not null
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        }

        // Disables Windows Update services, policies, and tasks
        public async Task DisableWUpdatesAsync()
        {
            Logger.Log("Starting to disable Windows Updates");
            try
            {
                // Loads the list of update-related services from configuration
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");
                var wuServices = config["windowsUpdate"];

                var scriptBuilder = new StringBuilder();
                // Prevents script termination on non-critical errors
                scriptBuilder.AppendLine("$ErrorActionPreference = 'SilentlyContinue'");

                foreach (var serviceInfo in wuServices)
                {
                    // Stops the service, disables it (Start=4), and locks permissions using sdset
                    scriptBuilder.AppendLine($@"
                        Stop-Service -Name '{serviceInfo.service}' -Force -ErrorAction SilentlyContinue
                        Set-ItemProperty -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\{serviceInfo.service}' -Name 'Start' -Value 4 -Type DWord -Force
                        sc.exe sdset '{serviceInfo.service}' 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                    ");
                }

                // Sets registry keys to disable Auto Update policies
                scriptBuilder.AppendLine(@"
                    if (!(Test-Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU')) {
                        New-Item -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Force | Out-Null
                    }
                    Set-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Name 'NoAutoUpdate' -Value 1 -Type DWord -Force
                    Set-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Name 'AUOptions' -Value 1 -Type DWord -Force

                    # Disables Delivery Optimization (P2P updates)
                    if (!(Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config')) {
                        New-Item -Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config' -Force | Out-Null
                    }
                    Set-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config' -Name 'DODownloadMode' -Value 0 -Type DWord -Force
                ");

                // Executes the constructed script
                await _processRunner.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{scriptBuilder.ToString().Replace("\"", "\"\"")}\"");
                Logger.Success("Disabling Windows Updates completed");
            }
            catch (Exception ex)
            {
                Logger.Error("DisableWUpdates", ex);
                throw new Exception("Failed to disable Windows Updates", ex);
            }
        }

        // Reverts changes to enable Windows Update functionality
        public async Task DisableWUpdatesRevertAsync()
        {
            Logger.Log("Starting to enable Windows Updates");
            try
            {
                // Removes the registry policies that blocked updates
                try
                {
                    using var key1 = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", true);
                    key1?.DeleteValue("NoAutoUpdate", false);
                    key1?.DeleteValue("AUOptions", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to delete WindowsUpdate policy values: {ex.Message}"); }

                // Removes the Delivery Optimization restriction
                try
                {
                    using var key2 = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config", true);
                    key2?.DeleteValue("DODownloadMode", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to delete DeliveryOptimization value: {ex.Message}"); }

                // Reloads service config to restore original startup types
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");
                var wuServices = config["windowsUpdate"];

                foreach (var serviceInfo in wuServices)
                {
                    try
                    {
                        // Removes the custom security descriptor (unlocks permissions)
                        Registry.LocalMachine.DeleteSubKeyTree(
                            $@"SYSTEM\CurrentControlSet\Services\{serviceInfo.service}\Security", false);

                        // Restores the default startup type
                        Registry.SetValue(
                            $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceInfo.service}",
                            "Start",
                            serviceInfo.defaultStartup,
                            RegistryValueKind.DWord);

                        Logger.Log($"Restored service: {serviceInfo.service} (Startup: {serviceInfo.defaultStartup})");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Failed to restore service {serviceInfo.service}: {ex.Message}");
                    }
                }

                // List of scheduled tasks to re-enable
                string[] tasksToEnable = new string[]
                {
                    @"\Microsoft\Windows\WindowsUpdate\Scheduled Start",
                    @"\Microsoft\Windows\UpdateOrchestrator\Schedule Scan",
                    @"\Microsoft\Windows\UpdateOrchestrator\Schedule Maintenance Work",
                    @"\Microsoft\Windows\UpdateOrchestrator\UpdateAssistant",
                    @"\Microsoft\Windows\WaaSMedic\PerformRemediation"
                };

                // Enables the scheduled tasks via PowerShell
                string taskList = string.Join("', '", tasksToEnable);
                string enableTasksScript = $@"
                    $ErrorActionPreference = 'SilentlyContinue'
                    $tasks = @('{taskList}')
                    foreach ($task in $tasks) {{
                        Enable-ScheduledTask -TaskName $task
                    }}
                ";

                await _processRunner.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{enableTasksScript.Replace("\"", "\"\"")}\"");
                Logger.Success("Enabling Windows Updates completed");
            }
            catch (Exception ex)
            {
                Logger.Error("DisableWUpdatesRevert", ex);
                throw new Exception("Failed to enable Windows Updates", ex);
            }
        }
    }
}
