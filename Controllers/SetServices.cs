using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text.Json;
using HST.Controllers;

#pragma warning disable CA1416
#pragma warning disable CS8600

namespace HST.Controllers.SetService
{
    public class SetServices
    {
        public async Task<bool> DisableConfiguredServices(List<string> servicesToDisable)
        {
            try
            {
                if (!servicesToDisable.Any()) return false;

                string serviceList = string.Join("', '", servicesToDisable);

                string script = $@"
            $services = @('{serviceList}')
            foreach ($service in $services) {{
                Stop-Service -Name $service -Force -ErrorAction SilentlyContinue
                Set-Service -Name $service -StartupType Disabled -ErrorAction SilentlyContinue
            }}
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
