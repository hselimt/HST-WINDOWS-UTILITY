using HST.Controllers;
using HST.Controllers.RemovalTools;
using System.Diagnostics;
#pragma warning disable CA1416
#pragma warning disable CS8600
#pragma warning disable CS8618
namespace HST.Controllers.SetService
{
    public class ServiceInfo
    {
        public string service { get; set; }
        public string name { get; set; }
    }
    public class SetServices
    {
        private readonly RemovalHelpers _removalTools;
        public SetServices(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }
        public async Task DisableConfiguredServices(List<string> servicesToDisable)
        {
            if (!servicesToDisable.Any()) return;
            var wServices = servicesToDisable.Where(s => s.Contains("*")).ToList();
            var services = servicesToDisable.Where(s => !s.Contains("*")).ToList();
            string disableServicesScript = "$ErrorActionPreference = 'SilentlyContinue'\n";

            if (services.Any())
            {
                string serviceList = string.Join("', '", services);
                disableServicesScript += $@"
                $services = @('{serviceList}')
                foreach ($svc in $services) {{
                    Stop-Service -Name $svc -Force
                    Set-Service -Name $svc -StartupType Disabled

                    # Set restrictive permissions - SYSTEM blocked, Admin allowed
                    sc.exe sdset $service 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                }}
                ";
            }

            foreach (string pattern in wServices)
            {
                string cleanPattern = pattern.Replace("*", "");
                disableServicesScript += $@"
                    Get-Service | Where-Object {{ $_.Name -like '{cleanPattern}*' }} | ForEach-Object {{
                    Stop-Service -Name $_.Name -Force
                    Set-Service -Name $_.Name -StartupType Disabled

                    # Set restrictive permissions - SYSTEM blocked, Admin allowed
                    sc.exe sdset $service 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                }}
                ";
            }

            await _removalTools.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{disableServicesScript.Replace("\"", "\"\"")}\"");
        }
    }
}