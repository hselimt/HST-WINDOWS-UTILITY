using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RemovalTools
{
    public class RemovalHelpers
    {
        private System.Windows.Forms.TextBox tbStatus;

        // Add a parameterless constructor
        public RemovalHelpers()
        {
            // Default constructor with no TextBox
        }

        public RemovalHelpers(System.Windows.Forms.TextBox statusTextBox)
        {
            tbStatus = statusTextBox;
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
                if (tbStatus != null)
                {
                    tbStatus.Text = $"Error executing command: {ex.Message}";
                }
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
                if (tbStatus != null)
                {
                    tbStatus.Text = $"Error deleting directory {dirPath}: {ex.Message}";
                }
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
                if (tbStatus != null)
                {
                    tbStatus.Text = $"Error deleting file {filePath}: {ex.Message}";
                }
            }
        }
    }
}