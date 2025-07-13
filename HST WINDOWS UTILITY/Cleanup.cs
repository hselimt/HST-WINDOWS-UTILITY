using RemovalTools;
using System.Diagnostics;
using System.ServiceProcess;

namespace CleanupApp
{
    public class Cleanup
    {
        private readonly RemovalHelpers _removalHelpers;

        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public Cleanup(RemovalHelpers removalHelpers)
        {
            _removalHelpers = removalHelpers;
        }

        public async Task RunRemoval(bool cleanTemp, bool cleanInternetCache, bool cleanEventLogs, bool cleanDefaultPowerPlans)
        {
            await ClearRecycleBin();
            await ClearUpdateCache();

            if (cleanTemp)
                await RemoveTemp();

            if (cleanInternetCache)
                await CleanInternetCache();

            if (cleanEventLogs)
                await ClearEventLogs();

            if (cleanDefaultPowerPlans)
                await ClearDefaultPowerPlans();
        }

        private async Task RemoveTemp()
        {
            await Task.Run(() =>
            {
                _removalHelpers.DeleteDirectoryIfExists(Path.GetTempPath());
                _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\Temp");
                _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\Prefetch");
                _removalHelpers.DeleteDirectoryIfExists(localAppData + @"\Temp");
            });
        }

        private async Task CleanInternetCache()
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
                    MessageBox.Show("ERROR CLEANING INTERNET CACHE", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private async Task ClearUpdateCache()
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
                    MessageBox.Show("ERROR CLEARING TEMP", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private async Task ClearRecycleBin()
        {
            await Task.Run(() =>
            {
                RunCommand("powershell.exe", "-Command Clear-RecycleBin -Force");
            });
        }

        private async Task ClearEventLogs()
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

        private async Task ClearDefaultPowerPlans()
        {
            try
            {
                string[] planGUIDs = 
                {
                    "381b4222-f694-41f0-9685-ff5bb260df2e",
                    "a1841308-3541-4fab-bc81-f71556f20b4a",
                    "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"
                };

                foreach (string guid in planGUIDs)
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "powercfg",
                            Arguments = $"/delete {guid}",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    await process.WaitForExitAsync();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR CLEARING DEFAULT POWERPLANS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RunCommand(string command, string args = "")
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
