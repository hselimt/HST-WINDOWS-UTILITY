using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Text.Json;
using static HST.Controllers.RemovalTools.Paths;

namespace HST.Controllers.Clear
{
    public class CleanUp
    {
        private readonly RemovalHelpers _removalHelpers;

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
                _removalHelpers.DeleteDirectoryIfExists(Path.Combine(LocalAppDataPath, "Temp"));
            });
        }

        public async Task CleanInternetCache()
        {
            await Task.Run(() =>
            {
                try
                {
                    string chromeCache = Path.Combine(LocalAppDataPath, @"Google\Chrome\User Data\Default\Cache");
                    string chromeCodeCache = Path.Combine(LocalAppDataPath, @"Google\Chrome\User Data\Default\Code Cache");
                    string chromeCookies = Path.Combine(LocalAppDataPath, @"Google\Chrome\User Data\Default\Network\Cookies");

                    _removalHelpers.DeleteDirectoryIfExists(chromeCache);
                    _removalHelpers.DeleteDirectoryIfExists(chromeCodeCache);
                    _removalHelpers.DeleteFileIfExists(chromeCookies);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error clearing internet cache: {ex.Message}");
                }
            });
        }

        public async Task ClearUpdateCache()
        {
            try
            {
                await _removalHelpers.RunCommandAsync("net", "stop wuauserv");
                await _removalHelpers.RunCommandAsync("net", "stop bits");
                await Task.Delay(200);

                _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\SoftwareDistribution\Download");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error clearing update cache: {ex.Message}");
            }
        }

        public async Task ClearRecycleBin()
        {
            try
            {
                await _removalHelpers.RunCommandAsync("powershell.exe",
                    "-NoProfile -ExecutionPolicy Bypass -Command Clear-RecycleBin -Force");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error clearing recycle bin: {ex.Message}");
            }
        }

        public async Task ClearEventLogs()
        {
            try
            {
                string[] logs = { "Application", "System", "Security", "Setup", "ForwardedEvents" };
                string logList = string.Join("', '", logs);
                string clearEventLogsScript = $@"
                $ErrorActionPreference = 'SilentlyContinue'
                $logs = @('{logList}')
                foreach ($log in $logs) {{
                    wevtutil cl $log
                }}
                ";

                await _removalHelpers.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{clearEventLogsScript.Replace("\"", "\"\"")}\"");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error clearing event logs: {ex.Message}");
            }
        }

        public async Task ClearDefaultPowerPlans()
        {
            try
            {
                var json = await File.ReadAllTextAsync(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GUIDConfig.json"));
                var config = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                var guidsToRemove = config["powerPlanGuid"];

                foreach (var guid in guidsToRemove)
                {
                    await _removalHelpers.RunCommandAsync("powercfg", $"-delete {guid}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error clearing power plans: {ex.Message}");
            }
        }
    }
}