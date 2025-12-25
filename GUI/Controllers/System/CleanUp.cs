using HST.Controllers.Helpers;

namespace HST.Controllers.System
{
    public class CleanUp
    {
        private readonly ProcessRunner _processRunner;

        public CleanUp(ProcessRunner processRunner)
        {
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        }

        // Deletes temporary files from Windows, User, and Prefetch directories
        public async Task RemoveTempAsync()
        {
            Logger.Log("Starting cleaning of Temp and Prefetch");
            try
            {
                await Task.Run(() =>
                {
                    FileManager.DeleteDirectoryIfExists(Path.GetTempPath());
                    FileManager.DeleteDirectoryIfExists(Path.Combine(Paths.SystemRoot, "Temp"));
                    FileManager.DeleteDirectoryIfExists(Path.Combine(Paths.SystemRoot, "Prefetch"));
                    FileManager.DeleteDirectoryIfExists(Path.Combine(Paths.LocalAppDataPath, "Temp"));
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
                    string chromeCache = Path.Combine(Paths.LocalAppDataPath, @"Google\Chrome\User Data\Default\Cache");
                    string chromeCodeCache = Path.Combine(Paths.LocalAppDataPath, @"Google\Chrome\User Data\Default\Code Cache");
                    string chromeCookies = Path.Combine(Paths.LocalAppDataPath, @"Google\Chrome\User Data\Default\Network\Cookies");

                    FileManager.DeleteDirectoryIfExists(chromeCache);
                    FileManager.DeleteDirectoryIfExists(chromeCodeCache);
                    FileManager.DeleteFileIfExists(chromeCookies);
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
                await _processRunner.RunCommandAsync("net", "stop wuauserv");
                await _processRunner.RunCommandAsync("net", "stop bits");
                await Task.Delay(200);

                FileManager.DeleteDirectoryIfExists(Path.Combine(Paths.SystemRoot, "SoftwareDistribution", "Download"));

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
                await _processRunner.RunCommandAsync("powershell.exe",
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

                await _processRunner.RunCommandAsync("powershell.exe",
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
                var config = await ConfigLoader.LoadStringConfigAsync("GUIDConfig.json");
                var guidsToRemove = config["powerPlanGuid"];

                foreach (var guid in guidsToRemove)
                {
                    await _processRunner.RunCommandAsync("powercfg", $"-delete {guid}");
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