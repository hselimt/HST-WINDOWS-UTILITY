using HST.Controllers.RemovalTools;
using HST.Controllers.SetService;
using Microsoft.Win32;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text.Json;

#pragma warning disable CA1416

namespace HST.Controllers.DisableUpdate
{
    public class DisableWindowsUpdates
    {
        private readonly SetServices _setServices;

        public DisableWindowsUpdates(SetServices setServices)
        {
            _setServices = setServices ?? throw new ArgumentNullException(nameof(setServices));
        }

        public async Task<bool> DisableWUpdates()
        {
            try
            {
                // Load services from JSON
                var json = await System.IO.File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "servicesConfig.json"));
                var config = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                var WUservicesToDisable = config["windowsUpdate"];

                string serviceList = string.Join("', '", WUservicesToDisable);

                string script = $@"
            $services = @('{serviceList}')
            foreach ($service in $services) {{
                Stop-Service -Name $service -Force -ErrorAction SilentlyContinue
                Set-Service -Name $service -StartupType Disabled -ErrorAction SilentlyContinue
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

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-WindowStyle Hidden -ExecutionPolicy Bypass -Command \"{script.Replace("\"", "\"\"")}\"",
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    process.Start();
                    await Task.Run(() => process.WaitForExit());
                    return process.ExitCode == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
