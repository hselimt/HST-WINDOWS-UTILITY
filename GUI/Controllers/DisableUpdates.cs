using HST.Controllers.RemovalTools;
using HST.Controllers.SetService;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Win32;
using HST.Controllers.Tool;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.DisableUpdate
{
    public class DisableWindowsUpdates
    {
        private readonly RemovalHelpers _removalHelpers;

        public DisableWindowsUpdates(RemovalHelpers removalHelpers)
        {
            _removalHelpers = removalHelpers ?? throw new ArgumentNullException(nameof(removalHelpers));
        }

        // Sets policies, modifies registry and disables Windows Update services
        public async Task DisableWUpdatesAsync()
        {
            Logger.Log("Starting to disable Windows Updates");
            try
            {
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");
                var wuServicesToDisable = config["windowsUpdate"].Select(s => s.service).ToList();
                string serviceList = string.Join("', '", wuServicesToDisable);

                string disableUpdatesScript = $@"
                    $services = @('{serviceList}')
                    foreach ($service in $services) {{
                        Stop-Service -Name $service -Force -ErrorAction SilentlyContinue
                        Set-Service -Name $service -StartupType Disabled -ErrorAction SilentlyContinue
        
                        # Set restrictive permissions - SYSTEM blocked, Admin allowed
                        sc.exe sdset $service 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                    }}

                    if (!(Test-Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU')) {{
                        New-Item -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Force | Out-Null
                    }}
                    Set-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Name 'NoAutoUpdate' -Value 1 -Type DWord -Force
                    Set-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Name 'AUOptions' -Value 1 -Type DWord -Force

                    if (!(Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config')) {{
                        New-Item -Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config' -Force | Out-Null
                    }}
                    Set-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config' -Name 'DODownloadMode' -Value 0 -Type DWord -Force
                ";

                await _removalHelpers.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{disableUpdatesScript.Replace("\"", "\"\"")}\"");
                Logger.Success("Disabling Windows Updates completed");
            }
            catch (Exception ex)
            {
                Logger.Error("DisableWUpdates", ex);
                throw new Exception("Failed to disable Windows Updates");
            }
        }

        // Modifies registry and enables Windows Update services
        public async Task DisableWUpdatesRevertAsync()
        {
            Logger.Log("Starting to enable Windows Updates");
            try
            {
                try
                {
                    using var key1 = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", true);
                    key1?.DeleteValue("NoAutoUpdate", false);
                    key1?.DeleteValue("AUOptions", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to delete WindowsUpdate policy values: {ex.Message}"); }

                try
                {
                    using var key2 = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config", true);
                    key2?.DeleteValue("DODownloadMode", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to delete DeliveryOptimization value: {ex.Message}"); }

                try
                {
                    Registry.LocalMachine.DeleteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\wuauserv\Security", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to remove security key for wuauserv: {ex.Message}"); }

                try
                {
                    Registry.LocalMachine.DeleteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\BITS\Security", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to remove security key for BITS: {ex.Message}"); }

                try
                {
                    Registry.LocalMachine.DeleteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\UsoSvc\Security", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to remove security key for UsoSvc: {ex.Message}"); }

                try
                {
                    Registry.LocalMachine.DeleteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\WaaSMedicSvc\Security", false);
                }
                catch (Exception ex) { Logger.Log($"Failed to remove security key for WaaSMedicSvc: {ex.Message}"); }

                try
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3, RegistryValueKind.DWord);
                }
                catch (Exception ex) { Logger.Log($"Failed to set Start value for wuauserv: {ex.Message}"); }

                try
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 2, RegistryValueKind.DWord);
                }
                catch (Exception ex) { Logger.Log($"Failed to set Start value for UsoSvc: {ex.Message}"); }

                try
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 3, RegistryValueKind.DWord);
                }
                catch (Exception ex) { Logger.Log($"Failed to set Start value for WaaSMedicSvc: {ex.Message}"); }

                try
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BITS", "Start", 2, RegistryValueKind.DWord);
                }
                catch (Exception ex) { Logger.Log($"Failed to set Start value for BITS: {ex.Message}"); }

                string[] tasksToEnable = new string[]
                {
                    @"\Microsoft\Windows\WindowsUpdate\Scheduled Start",
                    @"\Microsoft\Windows\UpdateOrchestrator\Schedule Scan",
                    @"\Microsoft\Windows\UpdateOrchestrator\Schedule Maintenance Work",
                    @"\Microsoft\Windows\UpdateOrchestrator\UpdateAssistant",
                    @"\Microsoft\Windows\WaaSMedic\PerformRemediation"
                };

                string taskList = string.Join("', '", tasksToEnable);
                string enableTasksScript = $@"
                    $ErrorActionPreference = 'SilentlyContinue'
                    $tasks = @('{taskList}')
                    foreach ($task in $tasks) {{
                        Enable-ScheduledTask -TaskName $task
                    }}
                ";

                await _removalHelpers.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{enableTasksScript.Replace("\"", "\"\"")}\"");
                Logger.Success("Enabling Windows Updates completed");
            }
            catch (Exception ex)
            {
                Logger.Error("DisableWUpdatesRevert", ex);
                throw new Exception("Failed to enable Windows Updates");
            }
        }
    }
}