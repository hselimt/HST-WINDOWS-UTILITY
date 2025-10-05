using HST.Controllers.RemovalTools;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<bool> RemoveConfiguredPackages(List<string> PackagesToRemove)
        {
            try
            {
                if (!PackagesToRemove.Any()) return false;

                string packageList = string.Join("', '", PackagesToRemove);

                string script = $@"
            $packages = @('{packageList}')
            foreach ($package in $packages) {{
                Get-AppxPackage -Name $package -AllUsers | Remove-AppxPackage -ErrorAction SilentlyContinue
                Get-AppxPackage -Name ""$package*"" -AllUsers | Remove-AppxPackage -ErrorAction SilentlyContinue
            }}
        ";

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-WindowStyle Hidden -ExecutionPolicy Bypass -Command \"{script.Replace("\"", "\"\"")}\"",
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    process.Start();
                    await Task.Run(() => process.WaitForExit());
                    return process.ExitCode == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
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

                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\EdgeUpdate\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Clients\\StartMenuInternet\\Microsoft Edge\" /f");
                await _removalTools.RunCommandAsync("reg", "delete \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Microsoft Edge\" /f");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while removing Edge: {ex.Message}");
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
                Debug.WriteLine($"An error occurred while removing OneDrive: {ex.Message}");
            }
        }
    }
}
