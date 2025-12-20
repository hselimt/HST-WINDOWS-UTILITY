using System.Diagnostics;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.RemovalTools
{
    public static class Paths
    {
        public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string ProgramDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static readonly string PublicDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        public static readonly string UserDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string UserProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static readonly string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
    }

    public class RemovalHelpers
    {
        // Runs command with admin privileges and timeout
        public async Task<bool> RunCommandAsync(string command, string arguments, int timeoutMs = 30000)
        {
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = arguments,
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    process.Start();
                    using var cts = new CancellationTokenSource(timeoutMs);
                    await process.WaitForExitAsync(cts.Token);
                    return process.ExitCode == 0;
                }
            }
            catch (OperationCanceledException ex)
            {
                Logger.Error("RunCommand timeout", ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("RunCommand", ex);
                return false;
            }
        }

        // Deletes directory if it exists
        public void DeleteDirectoryIfExists(string dirPath)
        {
            try
            {
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to delete directory {dirPath}: {ex.Message}");
            }
        }

        // Deletes file if it exists, supports environment variables
        public void DeleteFileIfExists(string filePath)
        {
            try
            {
                string fullPath = Environment.ExpandEnvironmentVariables(filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to delete file {filePath}: {ex.Message}");
            }
        }
    }
}