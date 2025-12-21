using HST.Controllers.RemovalTools;
using HST.Controllers.Tool;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO.Packaging;
using static HST.Controllers.RemovalTools.Paths;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.DebloatApps
{
    public class Debloater
    {
        private readonly RemovalHelpers _removalTools;

        public Debloater(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        // Removes configured Microsoft Store apps and provisioned packages
        public async Task RemoveConfiguredPackagesAsync(List<string> PackagesToRemove)
        {
            if (!PackagesToRemove.Any()) return;

            Logger.Log("Starting removal of configured packages");
            try
            {
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
                        Logger.Log($"Failed to remove package {package}: {ex.Message}");
                    }
                }
                Logger.Success("Removal of configured packages completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveConfiguredPackagesAsync", ex);
            }
        }

        // Removes Microsoft Edge browser, shortcuts, and registry entries
        public async Task RemoveEdgeAsync()
        {
            Logger.Log("Starting removal of Microsoft Edge");
            try
            {
                await _removalTools.RunCommandAsync("taskkill", "/f /im msedge.exe");
                await _removalTools.RunCommandAsync("taskkill", "/f /im msedgeupdate.exe");
                await _removalTools.RunCommandAsync("taskkill", "/f /im MicrosoftEdgeUpdate.exe");
                await Task.Delay(200);

                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "Edge"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "EdgeCore"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "EdgeUpdate"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "Edge"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "EdgeUpdate"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(ProgramDataPath, "Microsoft", "Edge"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(ProgramDataPath, "Microsoft", "EdgeUpdate"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(AppDataPath, "Internet Explorer"));
                _removalTools.DeleteFileIfExists(Path.Combine(AppDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(ProgramDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(PublicDesktopPath, "Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(UserDesktopPath, "Microsoft Edge.lnk"));

                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Microsoft\\EdgeUpdate\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Clients\\StartMenuInternet\\Microsoft Edge\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Microsoft Edge\" /f");
                Logger.Success("Removal of Microsoft Edge completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveEdgeAsync", ex);
            }
        }

        // Removes OneDrive and cleans up sync settings
        public async Task RemoveOneDriveAsync()
        {
            Logger.Log("Starting removal of OneDrive");
            try
            {
                // --- Disabling File Sync ---
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\OneDrive"))
                {
                    key.SetValue("DisableFileSync", 0x1, RegistryValueKind.DWord);
                }

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

                await Task.Delay(200);

                _removalTools.DeleteDirectoryIfExists(Path.Combine(UserProfilePath, "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(LocalAppDataPath, "Microsoft", "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(LocalAppDataPath, "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(ProgramDataPath, "Microsoft OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft OneDrive"));

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
                Logger.Success("Removal of OneDrive completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveOneDriveAsync", ex);
            }
        }

        // Removes common startup applications from registry
        public async Task RemoveStartupAppsAsync(List<string> startupAppsToRemove)
        {
            Logger.Log("Starting removal of configured Startup Apps");
            try
            {
                await Task.Run(() =>
                {
                    string runPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                    using (var key = Registry.CurrentUser.OpenSubKey(runPath, true))
                    {
                        if (key != null)
                        {
                            foreach (string name in key.GetValueNames().ToList())
                            {
                                if (startupAppsToRemove.Any(app => name.Contains(app, StringComparison.OrdinalIgnoreCase)))
                                    key.DeleteValue(name, false);
                            }
                        }
                    }
                    using (var key = Registry.LocalMachine.OpenSubKey(runPath, true))
                    {
                        if (key != null)
                        {
                            foreach (string name in key.GetValueNames().ToList())
                            {
                                if (startupAppsToRemove.Any(app => name.Contains(app, StringComparison.OrdinalIgnoreCase)))
                                    key.DeleteValue(name, false);
                            }
                        }
                    }
                });
                Logger.Success("Removal of configured Startup Apps completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveStartupAppsAsync", ex);
            }
        }
    }
}