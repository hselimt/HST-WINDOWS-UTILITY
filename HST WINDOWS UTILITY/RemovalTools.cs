using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RemovalTools
{
    public class RemovalHelpers
    {
        private System.Windows.Forms.TextBox tbStatus;
        public RemovalHelpers()
        {

        }

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
                }
            }
            catch (Exception ex)
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