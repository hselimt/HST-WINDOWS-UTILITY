using RemovalTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PackageRemoverApp
{
    public class PackagesRemover
    {
        private readonly RemovalHelpers _removalTools;

        public PackagesRemover(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        private static readonly string[] MsPackages = new[]
        {
            "Clipchamp",
            "Disney",
            "LinkedInforWindows",
            "Microsoft",
            "Microsoft.BingNews",
            "Microsoft.BingSearch",
            "Microsoft.BingWeather",
            "Microsoft.Copilot",
            "Microsoft.Edge.GameAssist",
            "Microsoft.GamingApp",
            "Microsoft.GetHelp",
            "Microsoft.Getstarted",
            "Microsoft.MSPaint",
            "Microsoft.Microsoft3DViewer",
            "Microsoft.MicrosoftOfficeHub",
            "Microsoft.MicrosoftSolitaireCollection",
            "Microsoft.MicrosoftStickyNotes",
            "Microsoft.MixedReality.Portal",
            "Microsoft.Office.OneNote",
            "Microsoft.OutlookforWindows",
            "Microsoft.Paint",
            "Microsoft.People",
            "Microsoft.PowerAutomateDesktop",
            "Microsoft.Windows.RemoteDesktop",
            "Microsoft.ScreenSketch",
            "Microsoft.SkypeApp",
            "Microsoft.SkyDrive.Desktop",
            "Microsoft.WindowsTerminal",
            "Microsoft.Todos",
            "Microsoft.Wallet",
            "Microsoft.WindowsAlarms",
            "Microsoft.WindowsCalculator",
            "Microsoft.WindowsCamera",
            "MicrosoftCorporationII.MicrosoftFamily",
            "Microsoft.WindowsFeedbackHub",
            "Microsoft.WindowsMaps",
            "Microsoft.Windows.DevHome",
            "Microsoft.WindowsSoundRecorder",
            "Microsoft.YourPhone",
            "Microsoft.ZuneMusic",
            "Microsoft.ZuneVideo",
            "MicrosoftCorporationII.QuickAssist",
            "MicrosoftTeams",
            "MSTeams",
            "MicrosoftWindows.Client.WebExperience",
            "SpotifyAB.SpotifyMusic",
            "Microsoft.WindowsCommunicationsApps"
        };

        private static readonly string[] XBOXPackages = new[]
        {
            "Microsoft.GamingApp",
            "Microsoft.Xbox.App",
            "Microsoft.Xbox.TCUI",
            "Microsoft.XboxApp",
            "Microsoft.XboxGameOverlay",
            "Microsoft.XboxGamingOverlay",
            "Microsoft.XboxSpeechToTextOverlay",
            "Microsoft.XboxIdentityProvider"
        };

        private static readonly string[] STOREPackages = new[]
        {
            "Microsoft.WindowsStore",
            "Microsoft.StorePurchaseApp",
        };

        public async Task RemoveMicrosoftAppsAsync()
        {
            await RemovePackagesAsync(MsPackages);
        }

        public async Task RemoveXboxAppsAsync()
        {
            await RemovePackagesAsync(XBOXPackages);
        }

        public async Task RemoveStoreAppsAsync()
        {
            await RemovePackagesAsync(STOREPackages);
        }

        public async Task RemoveEdgeAsync()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string publicPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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

                string iePath = Path.Combine(appDataPath, "Internet Explorer");
                _removalTools.DeleteDirectoryIfExists(iePath);

                _removalTools.DeleteFileIfExists(Path.Combine(appDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(programDataPath, @"Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(publicPath, "Microsoft Edge.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(userProfilePath, "Microsoft Edge.lnk"));

                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\EdgeUpdate\" / f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Clients\\StartMenuInternet\\Microsoft Edge\" / f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Microsoft Edge\" /f");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR REMOVING EDGE: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task RemoveOneDriveAsync()
        {
            try
            {
                await _removalTools.RunCommandAsync("taskkill", "/f /im OneDrive.exe");
                await Task.Delay(100);

                string systemRoot = Environment.GetEnvironmentVariable("SYSTEMROOT");

                if (File.Exists(Path.Combine(systemRoot, "SysWOW64", "OneDriveSetup.exe")))
                    await _removalTools.RunCommandAsync(Path.Combine(systemRoot, "SysWOW64", "OneDriveSetup.exe"), "/uninstall");

                if (File.Exists(Path.Combine(systemRoot, "System32", "OneDriveSetup.exe")))
                    await _removalTools.RunCommandAsync(Path.Combine(systemRoot, "System32", "OneDriveSetup.exe"), "/uninstall");

                if (File.Exists(Path.Combine(systemRoot, "Microsoft OneDrive", "OneDriveSetup.exe")))
                    await _removalTools.RunCommandAsync(Path.Combine(systemRoot, "Microsoft OneDrive", "OneDriveSetup.exe"), "/uninstall");

                await Task.Delay(100);

                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                _removalTools.DeleteDirectoryIfExists(Path.Combine(userProfile, "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(localAppData, "Microsoft", "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(localAppData, "OneDrive"));
                _removalTools.DeleteDirectoryIfExists(Path.Combine(programData, "Microsoft OneDrive"));
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files\Microsoft\OneDrive");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft\OneDrive");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files\Microsoft OneDrive");
                _removalTools.DeleteDirectoryIfExists(@"C:\Program Files (x86)\Microsoft OneDrive");

                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_CLASSES_ROOT\\OneDrive\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_CURRENT_USER\\Software\\Microsoft\\OneDrive\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\OneDrive\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_CLASSES_ROOT\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_CLASSES_ROOT\\Wow6432Node\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\" /f");

                await _removalTools.RunCommandAsync("reg", "add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\OneDrive\" /v DisableFileSyncNGSC /t REG_DWORD /d 1 /f");

                _removalTools.DeleteFileIfExists(Path.Combine(userProfile, "Desktop", "OneDrive.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(appData, "Microsoft", "Windows", "Start Menu", "Programs", "OneDrive.lnk"));
                _removalTools.DeleteFileIfExists(Path.Combine(programData, "Microsoft", "Windows", "Start Menu", "Programs", "OneDrive.lnk"));

                await _removalTools.RunCommandAsync("powershell", "-Command \"Stop-Service -Name OneDrive -ErrorAction SilentlyContinue; Set-Service -Name OneDrive -StartupType Disabled -ErrorAction SilentlyContinue\"");
                await _removalTools.RunCommandAsync("powershell", "-Command \"Remove-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Run' -Name 'OneDrive' -ErrorAction SilentlyContinue\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR REMOVING ONEDRIVE: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RemovePackagesAsync(string[] packages)
        {
            foreach (string package in packages)
            {
                try
                {
                    await RemoveAppxPackageAsync(package);
                }
                catch
                {

                }
            }
        }

        private async Task RemoveAppxPackageAsync(string packageName)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"-Command \"Get-AppxPackage -Name '{packageName}' -AllUsers | Remove-AppxPackage\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (!string.IsNullOrEmpty(error) || process.ExitCode != 0)
                {
                    process = new Process();
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.Arguments = $"-Command \"Get-AppxPackage -Name '{packageName}*' -AllUsers | Remove-AppxPackage\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    process.Start();
                    await process.WaitForExitAsync();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}