using System.Management;

namespace HST.Controllers.System
{
    public class SystemInfoDto
    {
        public string User { get; set; }
        public string Time { get; set; }
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

        // Gets current username and timestamp
        private SystemInfoDto GetBasicInfo()
        {
            return new SystemInfoDto
            {
                User = Environment.UserName,
                Time = DateTime.Now.ToString("yyyy.MM.dd - HH.mm")
            };
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

        // Gets total RAM size from WMI
        private SystemInfoDto GetRamInfo()
        {
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

        // Gets available storage on C drive
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