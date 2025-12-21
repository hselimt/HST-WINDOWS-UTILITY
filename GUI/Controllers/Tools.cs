using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Management;
using System.Diagnostics;
using System.Text.Json;
using static HST.Controllers.SystemController;

namespace HST.Controllers.Tool
{
    public static class Logger
    {
        private static string LogPath => Path.Combine(
        Path.GetTempPath(),
        "HST-WINDOWS-UTILITY.log"
    );

        // Delete old log file on application startup
        public static void InitializeLog()
        {
            try
            {
                if (File.Exists(LogPath))
                {
                    File.Delete(LogPath);
                }
            }
            catch
            {
                // If deletion fails, continue silently
            }
        }

        public static void Log(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(LogPath, logEntry);
            }
            catch
            {
                // If it fails continue silently
            }
        }

        public static void Error(string operation, Exception ex)
        {
            Log($"ERROR in {operation}");
            Log($"  MESSAGE: {ex.Message}");
            Log($"  STACK: {ex.StackTrace}");
            Log("----------------------------------------");
        }

        public static void Success(string operation)
        {
            Log($"SUCCESS: {operation}");
        }

        // Returns the log file location
        public static string GetLogLocation()
        {
            return LogPath;
        }
    }

    public class ConfigLoader
    {
        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;

        public static async Task<Dictionary<string, List<string>>> LoadStringConfigAsync(string filename)
        {
            var json = await File.ReadAllTextAsync(Path.Combine(_baseDir, filename));
            return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
        }

        public static async Task<Dictionary<string, List<T>>> LoadConfigAsync<T>(string filename)
        {
            var json = await File.ReadAllTextAsync(Path.Combine(_baseDir, filename));
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };
            return JsonSerializer.Deserialize<Dictionary<string, List<T>>>(json, options);
        }
    }

    public class RestorePointCreator
    {
        private readonly RemovalHelpers _removalTools;

        public RestorePointCreator(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        // Creates system restore point using PowerShell
        public async Task<bool> CreateRestorePointAsync()
        {
            Logger.Log("Starting restore point creation");
            string createRestorePointScript = @"
                Set-ItemProperty -Path 'HKLM:\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore' -Name 'SystemRestorePointCreationFrequency' -Value 0 -Type DWord -Force
                Enable-ComputerRestore -Drive $env:SystemDrive
                Checkpoint-Computer -Description 'HST-WINDOWS-UTILITY' -RestorePointType 'MODIFY_SETTINGS'
            ";

            try
            {
                bool result = await _removalTools.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{createRestorePointScript.Replace("\"", "\"\"")}\"");

                if (result)
                    Logger.Success("Restore point created");
                else
                    Logger.Log("Restore point creation failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("CreateRestorePoint", ex);
                throw new Exception("Failed to create restore point");
            }
        }
    }

    public class SysInfo
    {
        public async Task<SystemInfoDto> GetSystemInfoParallel()
        {
            // Fetches system information
            var tasks = new[]
            {
                Task.Run(() => GetBasicInfo()),
                Task.Run(() => GetGpuInfo()),
                Task.Run(() => GetCpuInfo()),
                Task.Run(() => GetRamInfo()),
                Task.Run(() => GetStorageInfo())
            };

            var results = await Task.WhenAll(tasks);

            // Returns system information
            return new SystemInfoDto
            {
                User = results[0].User,
                Time = results[0].Time,
                Gpu = results[1].Gpu,
                Cpu = results[2].Cpu,
                Ram = results[3].Ram,
                Storage = results[4].Storage
            };
        }

        private SystemInfoDto GetBasicInfo()
        {
            // Gets current username and timestamp
            return new SystemInfoDto
            {
                User = Environment.UserName,
                Time = DateTime.Now.ToString("yyyy.MM.dd - HH.mm")
            };
        }

        private SystemInfoDto GetGpuInfo()
        {
            // Gets GPU model from WMI
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
                using var collection = searcher.Get();
                using var gpu = collection.Cast<ManagementObject>().FirstOrDefault();
                return new SystemInfoDto { Gpu = gpu?["Name"]?.ToString() ?? "Unknown" };
            }
            catch
            {
                return new SystemInfoDto { Gpu = "Unknown" };
            }
        }

        private SystemInfoDto GetCpuInfo()
        {
            // Gets CPU model from WMI
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                using var collection = searcher.Get();
                using var cpu = collection.Cast<ManagementObject>().FirstOrDefault();
                return new SystemInfoDto { Cpu = cpu?["Name"]?.ToString() ?? "Unknown" };
            }
            catch
            {
                return new SystemInfoDto { Cpu = "Unknown" };
            }
        }

        private SystemInfoDto GetRamInfo()
        {
            // Gets total RAM size from WMI
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
                using var collection = searcher.Get();
                using var os = collection.Cast<ManagementObject>().FirstOrDefault();
                if (os != null)
                {
                    double ramGB = Convert.ToDouble(os["TotalVisibleMemorySize"]) / (1024.0 * 1024);
                    return new SystemInfoDto { Ram = $"{ramGB:F2}GB RAM" };
                }
                return new SystemInfoDto { Ram = "Unknown" };
            }
            catch
            {
                return new SystemInfoDto { Ram = "Unknown" };
            }
        }

        private SystemInfoDto GetStorageInfo()
        {
            // Gets available storage on C drive
            try
            {
                var drive = new DriveInfo("C");
                double freeGB = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
                return new SystemInfoDto { Storage = $"{freeGB:F2}GB FREE" };
            }
            catch
            {
                return new SystemInfoDto { Storage = "Unknown" };
            }
        }
    }
}