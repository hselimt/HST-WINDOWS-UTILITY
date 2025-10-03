using System.Diagnostics;

namespace HST.Controllers.RemovalTools
{
    public class RemovalHelpers
    {
        public async Task RunCommandAsync(string command, string arguments)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    await Task.Run(() => process.WaitForExit());

                    if (!string.IsNullOrEmpty(output)) Debug.WriteLine($"[CMD INFO: {command}] {output}");
                    if (!string.IsNullOrEmpty(error)) Debug.WriteLine($"[CMD ERROR: {command}] {error}");
                }
            }
            catch (Exception)
            {

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
            catch (Exception)
            {

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
            catch (Exception)
            {

            }
        }
    }
}