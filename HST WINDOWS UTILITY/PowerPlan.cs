using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace PowerPlanApp;

public class PowerPlan
{
    private readonly RemovalTools.RemovalHelpers _helpers;

    public PowerPlan(RemovalTools.RemovalHelpers helpers)
    {
        _helpers = helpers;
    }

    public async Task<(bool Success, string Message)> AddAndActivatePP()
    {
        string powFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HST.pow");

        if (!File.Exists(powFilePath))
            return (false, $"POWERPLAN FILE NOT FOUND {powFilePath}");

        try
        {
            using (var importProcess = new Process())
            {
                importProcess.StartInfo = new ProcessStartInfo
                {
                    FileName = "powercfg",
                    Arguments = $"-import \"{powFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                importProcess.Start();
                string output = await importProcess.StandardOutput.ReadToEndAsync();
                string error = await importProcess.StandardError.ReadToEndAsync();
                await importProcess.WaitForExitAsync();

                if (importProcess.ExitCode != 0)
                    return (false, "FAILED TO IMPORT POWERPLAN");

                string? newGuid = ExtractGuidFromOutput(output);

                if (string.IsNullOrWhiteSpace(newGuid))
                    return (false, "FAILED TO EXTRACT POWERPLAN");

                using (var activateProcess = new Process())
                {
                    activateProcess.StartInfo = new ProcessStartInfo
                    {
                        FileName = "powercfg",
                        Arguments = $"-setactive {newGuid}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    activateProcess.Start();
                    await activateProcess.WaitForExitAsync();

                    if (activateProcess.ExitCode != 0)
                    {
                        string activationError = await activateProcess.StandardError.ReadToEndAsync();
                        return (false, "FAILED TO ACTIVATE POWERPLAN");
                    }
                }

                return (true, "ADDED AND ACTIVATED POWERPLAN");
            }
        }
        catch (Exception ex)
        {
            return (false, $"ERROR {ex.Message}");
        }
    }

    private string? ExtractGuidFromOutput(string output)
    {
        var match = Regex.Match(output, @"[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}");
        return match.Success ? match.Value : null;
    }
}