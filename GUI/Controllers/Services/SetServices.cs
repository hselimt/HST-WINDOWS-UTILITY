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
            // Ensures process runner is not null
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        }

        // Disables configured services by stopping them and setting startup type to Disabled
        public async Task DisableConfiguredServicesAsync(List<string> servicesToDisable)
        {
            Logger.Log("Starting to disable configured services");
            if (!servicesToDisable.Any()) return;

            Logger.Log($"Services to disable: {servicesToDisable.Count}");

            var scriptBuilder = new StringBuilder();
            // Prevents script termination on non-critical errors
            scriptBuilder.AppendLine("$ErrorActionPreference = 'SilentlyContinue'");

            string serviceList = string.Join("', '", servicesToDisable);
            scriptBuilder.AppendLine($@"
                $services = @('{serviceList}')
                foreach ($svc in $services) {{
                    Stop-Service -Name $svc -Force
                    Set-Service -Name $svc -StartupType Disabled
                }}
            ");

            // Executes the constructed script
            await _processRunner.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{scriptBuilder.ToString().Replace("\"", "\"\"")}\"");
            Logger.Success("Disabling configured services complete");
        }

        // Reverts all configured services to their default startup state
        public async Task DisableConfiguredServicesRevertAsync()
        {
            Logger.Log("Starting to revert configured services");
            try
            {
                // Loads the full list of services
                var config = await ConfigLoader.LoadConfigAsync<ServiceInfo>("ServicesConfig.json");
                var allServices = config.Values.SelectMany(list => list);

                foreach (var serviceInfo in allServices)
                {
                    try
                    {
                        // Removes the Security subkey which may contain access restrictions set by sdset
                        Registry.LocalMachine.DeleteSubKeyTree(
                            $@"SYSTEM\CurrentControlSet\Services\{serviceInfo.service}\Security", false);

                        // Resets the startup type to the default value defined in config
                        Registry.SetValue(
                            $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceInfo.service}",
                            "Start",
                            serviceInfo.defaultStartup,
                            RegistryValueKind.DWord);

                        Logger.Log($"Restored: {serviceInfo.service} (Startup: {serviceInfo.defaultStartup})");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Failed to restore: {serviceInfo.service}", ex);
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