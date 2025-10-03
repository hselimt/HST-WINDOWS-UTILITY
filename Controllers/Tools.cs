using System.Diagnostics;
using System.Management;
using static HST.Controllers.SystemController;

#pragma warning disable CA1416

namespace HST.Controllers.Tool
{
    public class RestorePointCreator
    {
        public async Task<bool> CreateRestorePointAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = @"-WindowStyle Hidden -ExecutionPolicy Bypass -Command ""Enable-ComputerRestore -Drive '$env:SystemDrive\'; Checkpoint-Computer -Description 'HST' -RestorePointType 'MODIFY_SETTINGS'; exit""",
                        UseShellExecute = true,
                        Verb = "runas",
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
                    process?.WaitForExit();
                    return process?.ExitCode == 0;
                }
                catch (Exception)
                {
                    return false;
                }
            });
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
            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
            using var collection = searcher.Get();
            var gpu = collection.Cast<ManagementObject>().FirstOrDefault();
            return new SystemInfoDto { Gpu = gpu?["Name"]?.ToString() ?? "Unknown" };
        }

        private SystemInfoDto GetCpuInfo()
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            using var collection = searcher.Get();
            var cpu = collection.Cast<ManagementObject>().FirstOrDefault();
            return new SystemInfoDto { Cpu = cpu?["Name"]?.ToString() ?? "Unknown" };
        }

        private SystemInfoDto GetRamInfo()
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

        private SystemInfoDto GetStorageInfo()
        {
            var drive = new DriveInfo("C");
            double freeGB = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
            return new SystemInfoDto { Storage = $"{freeGB:F2}GB FREE" };
        }
    }
}