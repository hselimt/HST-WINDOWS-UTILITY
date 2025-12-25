using HST.Controllers.Helpers;
using Microsoft.Win32;
using System.Text;

namespace HST.Controllers.Services
{
    // DTO to hold service configuration data loaded from JSON
    public class ServiceInfo
    {
        public string service { get; set; }
        public string name { get; set; }
        public int defaultStartup { get; set; }
    }

    public class SetServices
    {
        private readonly ProcessRunner _processRunner;

        public SetServices(ProcessRunner processRunner)
        {
            // Validates that the process runner dependency is provided
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        }

        // Disables configured services
        public async Task DisableConfiguredServicesAsync(List<string> servicesToDisable)
        {
            Logger.Log("Starting to disable configured services");
            if (!servicesToDisable.Any()) return;

            // Separates services into exact names
            var wServices = servicesToDisable.Where(s => s.Contains("*")).ToList();
            var services = servicesToDisable.Where(s => !s.Contains("*")).ToList();

            Logger.Log($"Exact services to disable: {services.Count}");
            Logger.Log($"Wildcard patterns to disable: {wServices.Count}");

            var scriptBuilder = new StringBuilder();
            // Prevents script termination on non-critical errors
            scriptBuilder.AppendLine("$ErrorActionPreference = 'SilentlyContinue'");

            // Generates PowerShell commands for exact service matches
            foreach (var service in services)
            {
                // Stops the service and sets its startup type to Disabled (4) in the Registry
                scriptBuilder.AppendLine($@"
                    Stop-Service -Name '{service}' -Force -ErrorAction SilentlyContinue
                    Set-ItemProperty -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\{service}' -Name 'Start' -Value 4 -Type DWord -Force
                ");
            }

            // Generates PowerShell commands for wildcard matches
            foreach (string pattern in wServices)
            {
                // Finds all services matching the pattern, stops them, and disables them via Registry
                scriptBuilder.AppendLine($@"
                    Get-Service | Where-Object {{ $_.Name -like '{pattern}' }} | ForEach-Object {{
                        Stop-Service -Name $_.Name -Force -ErrorAction SilentlyContinue
                        $regPath = 'HKLM:\SYSTEM\CurrentControlSet\Services\' + $_.Name
                        Set-ItemProperty -Path $regPath -Name 'Start' -Value 4 -Type DWord -Force
                    }}
                ");
            }

            // Executes the script with admin privileges
            await _processRunner.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{scriptBuilder.ToString().Replace("\"", "\"\"")}\"");
            Logger.Success("Disabling configured services complete");
        }

        // Reverts all configured services to their default startup state
        public async Task DisableConfiguredServicesRevertAsync()
        {
            Logger.Log("Starting to enable configured services");
            try
            {
                // Loads the full list of services that this tool manages
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");
                var allServices = config.Values.SelectMany(list => list);

                foreach (var serviceInfo in allServices)
                {
                    try
                    {
                        var serviceNames = new List<string>();

                        // Handles wildcard services (e.g., per-user services like 'cbdhsvc_xxxxx')
                        if (serviceInfo.service.Contains("*"))
                        {
                            var baseName = serviceInfo.service.TrimEnd('*');
                            // Searches the registry for services starting with the base name
                            using var servicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services");
                            if (servicesKey != null)
                            {
                                serviceNames = servicesKey.GetSubKeyNames()
                                    .Where(n => n.StartsWith(baseName + "_", StringComparison.OrdinalIgnoreCase))
                                    .ToList();
                            }
                        }
                        else
                        {
                            // Adds the exact service name
                            serviceNames.Add(serviceInfo.service);
                        }

                        // Restores each identified service
                        foreach (var serviceName in serviceNames)
                        {
                            // Removes the 'Security' subkey which may contain access restrictions (like those set by sdset)
                            Registry.LocalMachine.DeleteSubKeyTree(
                                $@"SYSTEM\CurrentControlSet\Services\{serviceName}\Security", false);

                            // Resets the startup type to the default value defined in config
                            Registry.SetValue(
                                $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceName}",
                                "Start",
                                serviceInfo.defaultStartup,
                                RegistryValueKind.DWord);

                            Logger.Log($"Restored service to default: {serviceName} (Startup: {serviceInfo.defaultStartup})");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Failed to restore service: {serviceInfo.service}", ex);
                    }
                }
                Logger.Success("Restoring configured services complete");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load services config during revert", ex);
                throw;
            }
        }
    }
}