using Microsoft.VisualBasic.Devices;
using System.Management;

namespace HST_Windows_Tweaker.Models
{
    public class HardwareInfo
    {
        public string? Username { get; set; }
        public string? GPU { get; set; }
        public string? CPU { get; set; }
        public string? RAM { get; set; }
        public string? EmptySpace { get; set; }

        public void Load()
        {
            Username = Environment.UserName;

            try
            {
                var gpu = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>().First();
                GPU = gpu["Name"]?.ToString() ?? "Error";
            }
            catch
            {
                GPU = "Error";
            }

            try
            {
                var cpu = new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>().First();
                CPU = cpu["Name"]?.ToString() ?? "Error";
            }
            catch
            {
                CPU = "Error";
            }

            try
            {
                double ramGB = new ComputerInfo().TotalPhysicalMemory / (1024.0 * 1024 * 1024);
                RAM = $"{ramGB:F2}GB RAM";
            }
            catch
            {
                RAM = "Error";
            }

            try
            {
                var drive = new DriveInfo("C");
                double freeGB = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
                EmptySpace = $"{freeGB:F2}GB EMPTY SPACE";
            }
            catch
            {
                EmptySpace = "Error";
            }
        }
    }
}