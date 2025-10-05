using HST.Controllers.RemovalTools;
using System;
using System.Diagnostics;
using System.Text.Json;

#pragma warning disable CS8600
#pragma warning disable CS8602

namespace HST.Controllers.Clear
{
    public class CleanUp
    {
        private readonly RemovalHelpers _removalHelpers;

        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public CleanUp(RemovalHelpers removalHelpers)
        {
            _removalHelpers = removalHelpers ?? throw new ArgumentNullException(nameof(removalHelpers));
        }

        public async Task RemoveTemp()
        {
            await Task.Run(() =>
            {
                _removalHelpers.DeleteDirectoryIfExists(Path.GetTempPath());
                _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\Temp");
                _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\Prefetch");
                _removalHelpers.DeleteDirectoryIfExists(localAppData + @"\Temp");
            });
        }

        public async Task CleanInternetCache()
        {
            string chromeCache = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cache");
            string chromeCodeCache = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Code Cache");
            string chromeCookies = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Network\Cookies");

            await Task.Run(() =>
            {
                try
                {
                    _removalHelpers.DeleteDirectoryIfExists(chromeCache);
                    _removalHelpers.DeleteDirectoryIfExists(chromeCodeCache);

                    if (File.Exists(chromeCookies))
                    {
                        try { File.Delete(chromeCookies); } catch { }
                    }
                }

                catch (Exception)
                {

                }
            });
        }

        public async Task ClearUpdateCache()
        {
            await Task.Run(() =>
            {
                try
                {
                    string tempPath = Path.GetTempPath();

                    foreach (string file in Directory.GetFiles(tempPath))
                    {
                        try { File.Delete(file); } catch { }
                    }

                    foreach (string dir in Directory.GetDirectories(tempPath))
                    {
                        try { Directory.Delete(dir, true); } catch { }
                    }

                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "ipconfig";
                        process.StartInfo.Arguments = "/flushdns";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        process.WaitForExit(1000);
                    }

                }
                catch (Exception)
                {

                }
            });
        }

        public async Task ClearRecycleBin()
        {
            await Task.Run(() =>
            {
                RunCommand("powershell.exe", "-Command Clear-RecycleBin -Force");
            });
        }

        public async Task ClearEventLogs()
        {
            await Task.Run(() =>
            {
                string[] logs = { "Application", "System", "Security", "Setup", "ForwardedEvents" };
                foreach (var log in logs)
                {
                    RunCommand("wevtutil", $"cl {log}");
                }
            });
        }

        public async Task ClearDefaultPowerPlans()
        {
            try
            {
                var json = await System.IO.File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GUIDConfig.json"));
                var config = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                var GUIDsToRemove = config["powerplanguid"];

                foreach (string guid in GUIDsToRemove)
                {
                    using var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "powercfg",
                            Arguments = $"-delete {guid}",
                            UseShellExecute = true,
                            Verb = "runas",
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        }
                    };

                    process.Start();
                    await process.WaitForExitAsync();
                }
            }
            catch (Exception)
            {
            }
        }

        public void RunCommand(string command, string args = "")
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                }
            }
            catch
            {
            }
        }
    }
}
