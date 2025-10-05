using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;

namespace HST.Controllers.PowerPlan
{
    public class SetPowerPlan
    {
        private readonly RemovalHelpers _removalTools;

        public SetPowerPlan(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        public async Task AddAndActivatePP()
        {
            string powFilePath = Path.Combine(Directory.GetCurrentDirectory(), "HST.pow");

            try
            {
                if (!File.Exists(powFilePath))
                {
                    throw new FileNotFoundException($"The HST.pow file was not found at: {powFilePath}");
                }

                string tempOutputFilePath = Path.Combine(Path.GetTempPath(), "powercfg_output.txt");

                try
                {
                    string commandArguments = $"/c powercfg -import \"{powFilePath}\" > \"{tempOutputFilePath}\"";

                    using (var importProcess = new Process())
                    {
                        importProcess.StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = commandArguments,
                            UseShellExecute = true,
                            CreateNoWindow = true,
                            Verb = "runas"
                        };

                        importProcess.Start();
                        await importProcess.WaitForExitAsync();

                        if (importProcess.ExitCode != 0)
                        {
                            string errorOutput = File.Exists(tempOutputFilePath) ? await File.ReadAllTextAsync(tempOutputFilePath) : "No error output file found.";
                            throw new InvalidOperationException($"FAILED TO IMPORT POWERPLAN");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("An error occurred during the power plan import process. See inner exception for details.", ex);
                }

                string output = await File.ReadAllTextAsync(tempOutputFilePath);
                File.Delete(tempOutputFilePath);

                string? newGuid = ExtractGuidFromOutput(output);

                if (string.IsNullOrWhiteSpace(newGuid))
                {
                    throw new InvalidOperationException($"FAILED TO EXTRACT POWERPLAN GUID");
                }

                try
                {
                    using (var activateProcess = new Process())
                    {
                        activateProcess.StartInfo = new ProcessStartInfo
                        {
                            FileName = "powercfg",
                            Arguments = $"-setactive {newGuid}",
                            UseShellExecute = true,
                            Verb = "runas",
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        activateProcess.Start();
                        await activateProcess.WaitForExitAsync();

                        if (activateProcess.ExitCode != 0)
                        {
                            throw new InvalidOperationException($"FAILED TO ACTIVATE POWERPLAN. Exit Code: {activateProcess.ExitCode}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("An error occurred during the power plan activation process. See inner exception for details.", ex);
                }

            }
            catch (Exception)
            {
            }
        }

        private string? ExtractGuidFromOutput(string output)
        {
            var match = Regex.Match(output, @"[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}");
            return match.Success ? match.Value : null;
        }
    }
}