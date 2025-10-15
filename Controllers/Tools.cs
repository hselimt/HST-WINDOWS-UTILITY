using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Management;
using static HST.Controllers.SystemController;

#pragma warning disable CA1416

namespace HST.Controllers.Tool
{
    public class RestorePointCreator
    {
        private readonly RemovalHelpers _removalTools;

        public RestorePointCreator(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        public async Task<bool> CreateRestorePointAsync()
        {
            string createRestorePointScript = @"
                Set-ItemProperty -Path 'HKLM:\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore' -Name 'SystemRestorePointCreationFrequency' -Value 0 -Type DWord -Force
                Enable-ComputerRestore -Drive $env:SystemDrive
                Checkpoint-Computer -Description 'HST-WINDOWS-UTILITY' -RestorePointType 'MODIFY_SETTINGS'
            ";

            return await _removalTools.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{createRestorePointScript.Replace("\"", "\"\"")}\"");
        }
    }

    public class SysInfo
    {
        public async Task<SystemInfoDto> GetSystemInfoParallel()
        {
            var tasks = new[]
            {
                Task.Run(() => GetBasicInfo()),
                Task.Run(() => GetGpuInfo()),
                Task.Run(() => GetCpuInfo()),
                Task.Run(() => GetRamInfo()),
                Task.Run(() => GetStorageInfo())
            };

            var results = await Task.WhenAll(tasks);

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
            return new SystemInfoDto
            {
                User = Environment.UserName,
                Time = DateTime.Now.ToString("yyyy.MM.dd - HH.mm")
            };
        }

        private SystemInfoDto GetGpuInfo()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
                using var collection = searcher.Get();
                var gpu = collection.Cast<ManagementObject>().FirstOrDefault();
                return new SystemInfoDto { Gpu = gpu?["Name"]?.ToString() ?? "Unknown" };
            }
            catch
            {
                return new SystemInfoDto { Gpu = "Unknown" };
            }
        }

        private SystemInfoDto GetCpuInfo()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                using var collection = searcher.Get();
                var cpu = collection.Cast<ManagementObject>().FirstOrDefault();
                return new SystemInfoDto { Cpu = cpu?["Name"]?.ToString() ?? "Unknown" };
            }
            catch
            {
                return new SystemInfoDto { Cpu = "Unknown" };
            }
        }

        private SystemInfoDto GetRamInfo()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
                using var collection = searcher.Get();
                var os = collection.Cast<ManagementObject>().FirstOrDefault();
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