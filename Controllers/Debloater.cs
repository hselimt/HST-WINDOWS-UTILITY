using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.IO.Packaging;
using static HST.Controllers.RemovalTools.Paths;

#pragma warning disable CS8600
#pragma warning disable CS8625
#pragma warning disable CS8604

namespace HST.Controllers.DebloatApps
{
    public class Debloater
    {
        private readonly RemovalHelpers _removalTools;

        public Debloater(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        public async Task RemoveConfiguredPackages(List<string> PackagesToRemove)
        {
            if (!PackagesToRemove.Any()) return;

            foreach (string package in PackagesToRemove)
            {
                try
                {
                    await _removalTools.RunCommandAsync("powershell.exe",
                        $"-NoProfile -ExecutionPolicy Bypass -Command \"Get-AppxPackage -Name '{package}' -AllUsers | Remove-AppxPackage -AllUsers\"");

                    await _removalTools.RunCommandAsync("powershell.exe",
                        $"-NoProfile -ExecutionPolicy Bypass -Command \"Get-AppxProvisionedPackage -Online | Where-Object {{ $_.DisplayName -like '{package}' }} | ForEach-Object {{ Remove-AppxProvisionedPackage -Online -PackageName $_.PackageName }}\"");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error removing {package}: {ex.Message}");
                }
            }
        }

        public async Task RemoveEdgeAsync()
        {
            try
            {
                await _removalTools.RunCommandAsync("taskkill", "/f /im msedge.exe");
                await _removalTools.RunCommandAsync("taskkill", "/f /im msedgeupdate.exe");
                await _removalTools.RunCommandAsync("taskkill", "/f /im MicrosoftEdgeUpdate.exe");
                await Task.Delay(100);

                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\Edge");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\EdgeCore");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\EdgeUpdate");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files\Microsoft\Edge");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files\Microsoft\EdgeUpdate");
                _removalTools.DeleteDirectoryIfExists(@"C:\ProgramData\Microsoft\Edge");
                _removalTools.DeleteDirectoryIfExists(@"C:\ProgramData\Microsoft\EdgeUpdate");

                _removalTools.DeleteDirectoryIfExists(Path.Combine(AppDataPath, "Internet Explorer"));

                _removalTools.DeleteFileIfExists(Path.Combine(AppDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(ProgramDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(PublicDesktopPath, "Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(UserDesktopPath, "Microsoft Edge.lnk"));

                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Microsoft\\EdgeUpdate\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Clients\\StartMenuInternet\\Microsoft Edge\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Microsoft Edge\" /f");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing Edge: {ex.Message}");
            }
        }

        public async Task RemoveOneDriveAsync()
        {
            try
            {
                await _removalTools.RunCommandAsync("taskkill", "/f /im OneDrive.exe");

                string oneDriveSetup64 = Path.Combine(SystemRoot, "SysWOW64", "OneDriveSetup.exe");
                string oneDriveSetup32 = Path.Combine(SystemRoot, "System32", "OneDriveSetup.exe");
                string oneDriveSetupAlt = Path.Combine(SystemRoot, "Microsoft OneDrive", "OneDriveSetup.exe");

                if (File.Exists(oneDriveSetup64))
                    await _removalTools.RunCommandAsync(oneDriveSetup64, "/uninstall");
                if (File.Exists(oneDriveSetup32))
                    await _removalTools.RunCommandAsync(oneDriveSetup32, "/uninstall");
                if (File.Exists(oneDriveSetupAlt))
                    await _removalTools.RunCommandAsync(oneDriveSetupAlt, "/uninstall");

                await Task.Delay(100);

                _removalTools.DeleteDirectoryIfExists(Path.Combine(UserProfilePath, "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(LocalAppDataPath, "Microsoft", "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(LocalAppDataPath, "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(ProgramDataPath, "Microsoft OneDrive"));
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files\Microsoft\OneDrive");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\OneDrive");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files\Microsoft OneDrive");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft OneDrive");

                await _removalTools.RunCommandAsync("reg", "delete \"HKCR\\OneDrive\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKCU\\Software\\Microsoft\\OneDrive\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Microsoft\\OneDrive\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKCR\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKCR\\Wow6432Node\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\" /f");
                await _removalTools.RunCommandAsync("reg", "add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\OneDrive\" /v DisableFileSyncNGSC /t REG_DWORD /d 1 /f");

                _removalTools.DeleteFileIfExists(Path.Combine(UserProfilePath, "Desktop", "OneDrive.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(AppDataPath, "Microsoft", "Windows", "Start Menu", "Programs", "OneDrive.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(ProgramDataPath, "Microsoft", "Windows", "Start Menu", "Programs", "OneDrive.lnk"));

                string removeOneDriveScript = "Stop-Service -Name OneDrive -ErrorAction SilentlyContinue; Set-Service -Name OneDrive -StartupType Disabled -ErrorAction SilentlyContinue; Remove-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Run' -Name 'OneDrive' -ErrorAction SilentlyContinue";
                await _removalTools.RunCommandAsync("powershell.exe", $"-NoProfile -Command \"{removeOneDriveScript}\"");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing OneDrive: {ex.Message}");
            }
        }
    }
}