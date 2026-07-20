using HST.Controllers.Helpers;
using System.Management;

namespace HST.Controllers.System
{
    public class SystemInfoDto
    {
        public string User { get; set; }
        public string Os { get; set; }
        public string Gpu { get; set; }
        public string Cpu { get; set; }
        public string Ram { get; set; }
        public string Storage { get; set; }
    }

    public class SysInfo
    {
        // Fetches system information
        public async Task<SystemInfoDto> GetSystemInfoParallel()
        {
            var tasks = new[]
            {
                Task.Run(() => GetUserInfo()),
                Task.Run(() => GetOsInfo()),
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
                Os = results[1].Os,
                Gpu = results[2].Gpu,
                Cpu = results[3].Cpu,
                Ram = results[4].Ram,
                Storage = results[5].Storage
            };
        }

        // Gets current username
        private SystemInfoDto GetUserInfo()
        {
            return new SystemInfoDto { User = Environment.UserName };
        }

        // Gets OS name and build number from WMI
        private SystemInfoDto GetOsInfo()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Caption, BuildNumber FROM Win32_OperatingSystem");
                using var collection = searcher.Get();
                using var os = collection.Cast<ManagementObject>().FirstOrDefault();
                if (os == null)
                {
                    return new SystemInfoDto { Os = "Unknown" };
                }

                string caption = os["Caption"]?.ToString()?.Trim() ?? "Unknown";
                string build = os["BuildNumber"]?.ToString();
                string osString = string.IsNullOrEmpty(build) ? caption : $"{caption} (Build {build})";
                return new SystemInfoDto { Os = osString };
            }
            catch
            {
                return new SystemInfoDto { Os = "Unknown" };
            }
        }

        // Gets GPU model from WMI
        private SystemInfoDto GetGpuInfo()
        {
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

        // Gets CPU model from WMI
        private SystemInfoDto GetCpuInfo()
        {
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

        // Gets used/total RAM and module speed from WMI
        private SystemInfoDto GetRamInfo()
        {
            try
            {
                using var osSearcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
                using var osCollection = osSearcher.Get();
                using var os = osCollection.Cast<ManagementObject>().FirstOrDefault();
                if (os == null)
                {
                    return new SystemInfoDto { Ram = "Unknown" };
                }

                double totalKB = Convert.ToDouble(os["TotalVisibleMemorySize"]);
                double freeKB = Convert.ToDouble(os["FreePhysicalMemory"]);
                double totalGB = totalKB / (1024.0 * 1024);
                double usedGB = (totalKB - freeKB) / (1024.0 * 1024);

                using var memSearcher = new ManagementObjectSearcher("SELECT Speed FROM Win32_PhysicalMemory");
                using var memCollection = memSearcher.Get();
                using var mem = memCollection.Cast<ManagementObject>().FirstOrDefault();
                string speed = mem?["Speed"]?.ToString() ?? "Unknown";

                return new SystemInfoDto { Ram = $"{usedGB:F2} / {totalGB:F2}GB @ {speed}MHz" };
            }
            catch
            {
                return new SystemInfoDto { Ram = "Unknown" };
            }
        }

        // Gets used/total storage on the actual Windows system drive
        private SystemInfoDto GetStorageInfo()
        {
            try
            {
                string driveRoot = Path.GetPathRoot(Paths.SystemRoot) ?? "C:\\";
                var drive = new DriveInfo(driveRoot);
                double totalGB = drive.TotalSize / (1024.0 * 1024 * 1024);
                double freeGB = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
                double usedGB = totalGB - freeGB;
                return new SystemInfoDto { Storage = $"{usedGB:F2} / {totalGB:F2}GB" };
            }
            catch
            {
                return new SystemInfoDto { Storage = "Unknown" };
            }
        }
    }
}