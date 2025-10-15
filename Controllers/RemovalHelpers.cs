using System.Diagnostics;

namespace HST.Controllers.RemovalTools
{
    public static class Paths
    {
        public static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string ProgramDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string PublicDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        public static string UserDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string UserProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
    }

    public class RemovalHelpers
    {
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
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"Command timed out after {timeoutMs}ms: {command} {arguments}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to run command: {ex.Message}");
                return false;
            }
        }

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
                Debug.WriteLine($"Failed to delete directory {dirPath}. Error: {ex.Message}");
            }
        }

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
                Debug.WriteLine($"Failed to delete file {filePath}. Error: {ex.Message}");
            }
        }
    }
}