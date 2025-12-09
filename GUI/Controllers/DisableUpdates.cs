using HST.Controllers.RemovalTools;
using HST.Controllers.SetService;
using System.Diagnostics;
using System.Text.Json;

#pragma warning disable CA1416
#pragma warning disable CS8602

namespace HST.Controllers.DisableUpdate
{
    public class DisableWindowsUpdates
    {
        private readonly RemovalHelpers _removalHelpers;
        public DisableWindowsUpdates(RemovalHelpers removalHelpers)
        {
            _removalHelpers = removalHelpers ?? throw new ArgumentNullException(nameof(removalHelpers));
        }

        public async Task DisableWUpdates()
        {
            try
            {
                var json = await System.IO.File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServicesConfig.json"));
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };
                var config = JsonSerializer.Deserialize<Dictionary<string, List<ServiceInfo>>>(json, options);
                var WUservicesToDisable = config["windowsUpdate"].Select(s => s.service).ToList();
                string serviceList = string.Join("', '", WUservicesToDisable);

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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error disabling Windows update services: {ex.Message}");
            }
        }
    }
}
