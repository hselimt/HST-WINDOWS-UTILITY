using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Text.Json;
using static HST.Controllers.RemovalTools.Paths;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.Clear
{
    public class CleanUp
    {
        private readonly RemovalHelpers _removalHelpers;

        public CleanUp(RemovalHelpers removalHelpers)
        {
            _removalHelpers = removalHelpers ?? throw new ArgumentNullException(nameof(removalHelpers));
        }

        // Deletes temporary files from Windows, User, and Prefetch directories to free space
        public async Task RemoveTempAsync()
        {
            Logger.Log("Starting cleaning of Temp and Prefetch");
            try
            {
                await Task.Run(() =>
                {
                    _removalHelpers.DeleteDirectoryIfExists(Path.GetTempPath());
                    _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\Temp");
                    _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\Prefetch");
                    _removalHelpers.DeleteDirectoryIfExists(Path.Combine(LocalAppDataPath, "Temp"));
                });
                Logger.Success("Cleaning of Temp and Prefetch completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveTempAsync", ex);
            }
        }

        // Purges Google Chrome cache, code cache, and cookies from the local user profile
        public async Task CleanInternetCacheAsync()
        {
            Logger.Log("Starting cleaning of Chrome cache");
            try
            {
                await Task.Run(() =>
                {
                    string chromeCache = Path.Combine(LocalAppDataPath, @"Google\Chrome\User Data\Default\Cache");
                    string chromeCodeCache = Path.Combine(LocalAppDataPath, @"Google\Chrome\User Data\Default\Code Cache");
                    string chromeCookies = Path.Combine(LocalAppDataPath, @"Google\Chrome\User Data\Default\Network\Cookies");

                    _removalHelpers.DeleteDirectoryIfExists(chromeCache);
                    _removalHelpers.DeleteDirectoryIfExists(chromeCodeCache);
                    _removalHelpers.DeleteFileIfExists(chromeCookies);
                });
                Logger.Success("Cleaning of Chrome cache completed");
            }
            catch (Exception ex)
            {
                Logger.Error("CleanInternetCacheAsync", ex);
            }
        }

        // Clears Windows Update download cache
        public async Task ClearUpdateCacheAsync()
        {
            Logger.Log("Starting cleaning of Windows Update cache");
            try
            {
                // Stops Windows Update services before clearing cache
                await _removalHelpers.RunCommandAsync("net", "stop wuauserv");
                await _removalHelpers.RunCommandAsync("net", "stop bits");
                await Task.Delay(200);

                _removalHelpers.DeleteDirectoryIfExists(@"C:\Windows\SoftwareDistribution\Download");

                Logger.Success("Cleaning of Windows Update cache completed");
            }
            catch (Exception ex)
            {
                Logger.Error("ClearUpdateCacheAsync", ex);
            }
        }

        // Forcefully empties the Recycle Bin
        public async Task ClearRecycleBinAsync()
        {
            Logger.Log("Starting cleaning of Recycle Bin");
            try
            {
                await _removalHelpers.RunCommandAsync("powershell.exe",
                    "-NoProfile -ExecutionPolicy Bypass -Command Clear-RecycleBin -Force");

                Logger.Success("Recycle Bin cleaned");
            }
            catch (Exception ex)
            {
                Logger.Error("ClearRecycleBinAsync", ex);
            }
        }

        // Clears standard Windows Event Logs to reduce log bloat
        public async Task ClearEventLogsAsync()
        {
            Logger.Log("Starting cleaning of Event Logs");
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

                Logger.Success("Event logs cleaned");
            }
            catch (Exception ex)
            {
                Logger.Error("ClearEventLogsAsync", ex);
            }
        }

        // Removes default Windows power plans (Balanced, Power Saver, High Performance, Ultimate Performance)
        public async Task ClearDefaultPowerPlansAsync()
        {
            Logger.Log("Starting removal of default power plans");
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

                Logger.Success("Default power plans removed");
            }
            catch (Exception ex)
            {
                Logger.Error("ClearDefaultPowerPlansAsync", ex);
            }
        }
    }
}