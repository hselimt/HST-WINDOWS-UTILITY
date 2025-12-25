using HST.Controllers.Helpers;
using Microsoft.Win32;
using static HST.Controllers.Helpers.Paths;

namespace HST.Controllers.Debloat
{
    public class Debloater
    {
        private readonly ProcessRunner _processRunner;

        public Debloater(ProcessRunner processRunner)
        {
            // Ensures process runner is not null
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        }

        // Removes configured Packages and stops them from reinstalling
        public async Task RemoveConfiguredPackagesAsync(List<string> packagesToRemove)
        {
            // Exits if list is empty
            if (!packagesToRemove.Any()) return;

            Logger.Log("Starting removal of configured packages");

            foreach (string package in packagesToRemove)
            {
                try
                {
                    // Removes the package for all existing users via PowerShell
                    await _processRunner.RunCommandAsync("powershell.exe",
                        $"-NoProfile -ExecutionPolicy Bypass -Command \"Get-AppxPackage -Name '{package}' -AllUsers | Remove-AppxPackage -AllUsers\"");

                    // Removes the provisioned package to prevent it from installing for new users
                    await _processRunner.RunCommandAsync("powershell.exe",
                        $"-NoProfile -ExecutionPolicy Bypass -Command \"Get-AppxProvisionedPackage -Online | Where-Object {{ $_.DisplayName -like '{package}' }} | ForEach-Object {{ Remove-AppxProvisionedPackage -Online -PackageName $_.PackageName }}\"");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to remove package {package}: {ex.Message}");
                }
            }

            Logger.Success("Removal of configured packages completed");
        }

        // Completely removes Microsoft Edge including files, shortcuts, and registry keys
        public async Task RemoveEdgeAsync()
        {
            Logger.Log("Starting removal of Microsoft Edge");

            try
            {
                // Force kills all running Edge and updater processes
                await _processRunner.RunCommandAsync("taskkill", "/f /im msedge.exe");
                await _processRunner.RunCommandAsync("taskkill", "/f /im msedgeupdate.exe");
                await _processRunner.RunCommandAsync("taskkill", "/f /im MicrosoftEdgeUpdate.exe");
                await Task.Delay(200);

                // Defines all directories where Edge stores data or installation files
                var edgeDirectories = new[]
                {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "Edge"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "EdgeCore"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "EdgeUpdate"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "Edge"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "EdgeUpdate"),
                    Path.Combine(ProgramDataPath, "Microsoft", "Edge"),
                    Path.Combine(ProgramDataPath, "Microsoft", "EdgeUpdate"),
                    Path.Combine(AppDataPath, "Internet Explorer")
                };

                // Deletes each directory if it exists
                foreach (var dir in edgeDirectories)
                {
                    FileManager.DeleteDirectoryIfExists(dir);
                }

                // Defines locations of Edge shortcuts
                var edgeShortcuts = new[]
                {
                    Path.Combine(AppDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"),
                    Path.Combine(ProgramDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"),
                    Path.Combine(PublicDesktopPath, "Microsoft Edge.lnk"),
                    Path.Combine(UserDesktopPath, "Microsoft Edge.lnk")
                };

                // Removes shortcuts
                foreach (var shortcut in edgeShortcuts)
                {
                    FileManager.DeleteFileIfExists(shortcut);
                }

                // Registry keys related to Edge updates and uninstallation
                var edgeRegistryKeys = new[]
                {
                    "HKLM\\Software\\Microsoft\\EdgeUpdate",
                    "HKLM\\Software\\Clients\\StartMenuInternet\\Microsoft Edge",
                    "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Microsoft Edge"
                };

                // Deletes the registry keys
                foreach (var regKey in edgeRegistryKeys)
                {
                    await _processRunner.RunCommandAsync("reg", $"delete \"{regKey}\" /f");
                }

                Logger.Success("Removal of Microsoft Edge completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveEdgeAsync", ex);
            }
        }

        // Completely removes OneDrive including files and registry keys
        public async Task RemoveOneDriveAsync()
        {
            Logger.Log("Starting removal of OneDrive");

            try
            {
                // Disables OneDrive file sync via Group Policy registry key
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\OneDrive"))
                {
                    key.SetValue("DisableFileSync", 0x1, RegistryValueKind.DWord);
                }

                // Stops the OneDrive process
                await _processRunner.RunCommandAsync("taskkill", "/f /im OneDrive.exe");

                // Potential paths for the OneDrive uninstaller
                var oneDriveSetupPaths = new[]
                {
                    Path.Combine(SystemRoot, "SysWOW64", "OneDriveSetup.exe"),
                    Path.Combine(SystemRoot, "System32", "OneDriveSetup.exe"),
                    Path.Combine(SystemRoot, "Microsoft OneDrive", "OneDriveSetup.exe")
                };

                // Runs the uninstaller if found
                foreach (var setupPath in oneDriveSetupPaths)
                {
                    if (File.Exists(setupPath))
                    {
                        await _processRunner.RunCommandAsync(setupPath, "/uninstall");
                    }
                }

                await Task.Delay(200);

                // Directories containing OneDrive data
                var oneDriveDirectories = new[]
                {
                    Path.Combine(UserProfilePath, "OneDrive"),
                    Path.Combine(LocalAppDataPath, "Microsoft", "OneDrive"),
                    Path.Combine(LocalAppDataPath, "OneDrive"),
                    Path.Combine(ProgramDataPath, "Microsoft OneDrive"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "OneDrive"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "OneDrive"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft OneDrive"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft OneDrive")
                };

                // Deletes OneDrive directories
                foreach (var dir in oneDriveDirectories)
                {
                    FileManager.DeleteDirectoryIfExists(dir);
                }

                // Registry keys for OneDrive integration and settings
                var oneDriveRegistryKeys = new[]
                {
                    "HKCR\\OneDrive",
                    "HKCU\\Software\\Microsoft\\OneDrive",
                    "HKLM\\Software\\Microsoft\\OneDrive",
                    "HKCR\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "HKCR\\Wow6432Node\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"
                };

                // Removes registry keys
                foreach (var regKey in oneDriveRegistryKeys)
                {
                    await _processRunner.RunCommandAsync("reg", $"delete \"{regKey}\" /f");
                }

                // Adds policy to prevent file sync (double check)
                await _processRunner.RunCommandAsync("reg", "add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\OneDrive\" /v DisableFileSyncNGSC /t REG_DWORD /d 1 /f");

                // Removes OneDrive shortcuts
                var oneDriveShortcuts = new[]
                {
                    Path.Combine(UserProfilePath, "Desktop", "OneDrive.lnk"),
                    Path.Combine(AppDataPath, "Microsoft", "Windows", "Start Menu", "Programs", "OneDrive.lnk"),
                    Path.Combine(ProgramDataPath, "Microsoft", "Windows", "Start Menu", "Programs", "OneDrive.lnk")
                };

                foreach (var shortcut in oneDriveShortcuts)
                {
                    FileManager.DeleteFileIfExists(shortcut);
                }

                // Removes OneDrive from the current user's startup run key
                await _processRunner.RunCommandAsync("powershell.exe",
                    "-NoProfile -Command \"Remove-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Run' -Name 'OneDrive' -ErrorAction SilentlyContinue\"");

                Logger.Success("Removal of OneDrive completed");
            }
            catch (Exception ex)
            {
                Logger.Error("RemoveOneDriveAsync", ex);
            }
        }

        // Checks registry startup keys and removes entries matching the list
        public async Task RemoveStartupAppsAsync(List<string> startupAppsToRemove)
        {
            Logger.Log("Starting removal of configured Startup Apps");

            try
            {
                await Task.Run(() =>
                {
                    string runPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                    // Checks Current User startup items
                    using (var key = Registry.CurrentUser.OpenSubKey(runPath, true))
                    {
                        if (key != null)
                        {
                            foreach (string name in key.GetValueNames().ToList())
                            {
                                // Deletes the value if it matches any app in the removal list
                                if (startupAppsToRemove.Any(app => name.Contains(app, StringComparison.OrdinalIgnoreCase)))
                                    key.DeleteValue(name, false);
                            }
                        }
                    }

                    // Checks Local Machine (all users) startup items
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