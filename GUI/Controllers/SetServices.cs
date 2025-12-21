using HST.Controllers;
using HST.Controllers.RemovalTools;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text;
using HST.Controllers.Tool;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.SetService
{
    public class ServiceInfo
    {
        public string service { get; set; }
        public string name { get; set; }
        public int defaultStartup { get; set; }
    }

    public class SetServices
    {
        private readonly RemovalHelpers _removalTools;

        public SetServices(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        // Disables configured services
        public async Task DisableConfiguredServicesAsync(List<string> servicesToDisable)
        {
            Logger.Log("Starting to disable configured services");
            if (!servicesToDisable.Any()) return;

            // Splits list into Wildcards and Exact matches
            var wServices = servicesToDisable.Where(s => s.Contains("*")).ToList();
            var services = servicesToDisable.Where(s => !s.Contains("*")).ToList();

            Logger.Log($"Exact services to disable: {services.Count}");
            Logger.Log($"Wildcard patterns to disable: {wServices.Count}");

            var scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("$ErrorActionPreference = 'SilentlyContinue'");

            if (services.Any())
            {
                string serviceList = string.Join("', '", services);
                scriptBuilder.AppendLine($@"
                $services = @('{serviceList}')
                foreach ($svc in $services) {{
                    Stop-Service -Name $svc -Force
                    Set-Service -Name $svc -StartupType Disabled

                    # Restrict permissions to make it unrevertable
                    # sc.exe sdset $svc 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                }}
                ");
            }

            foreach (string pattern in wServices)
            {
                scriptBuilder.AppendLine($@"
                    Get-Service | Where-Object {{ $_.Name -like '{pattern}' }} | ForEach-Object {{
                        Stop-Service -Name $_.Name -Force
                        Set-Service -Name $_.Name -StartupType Disabled

                        # Restrict permissions to make it unrevertable
                        # sc.exe sdset $_.Name 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                    }}
                ");
            }

            string disableServicesScript = scriptBuilder.ToString();

            await _removalTools.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{disableServicesScript.Replace("\"", "\"\"")}\"");
            Logger.Success("Disabling configured services complete");
        }

        // Re-enables disabled services
        public async Task DisableConfiguredServicesRevertAsync()
        {
            Logger.Log("Starting to enable configured services");

            try
            {
                // Load all services from config
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");
                var allServices = config.Values.SelectMany(list => list);

                foreach (var serviceInfo in allServices)
                {
                    try
                    {
                        // Remove security restrictions
                        Registry.LocalMachine.DeleteSubKeyTree(
                            $@"SYSTEM\CurrentControlSet\Services\{serviceInfo.service}\Security", false);

                        // Restore Windows default startup from config
                        Registry.SetValue(
                            $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceInfo.service}",
                            "Start",
                            serviceInfo.defaultStartup,
                            RegistryValueKind.DWord);

                        Logger.Log($"Restored service to default: {serviceInfo.service} (Startup: {serviceInfo.defaultStartup})");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Failed to restore service: {serviceInfo.service}", ex);
                    }
                }

                Logger.Log("Finished restoring configured services to Windows defaults");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load services config during revert", ex);
                throw;
            }
        }
    }
}