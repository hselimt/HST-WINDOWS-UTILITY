using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PowerPlanApp;
public class PowerPlan
{
    private readonly RemovalTools.RemovalHelpers _helpers;

    public PowerPlan(RemovalTools.RemovalHelpers helpers)
    {
        _helpers = helpers;
    }

    public async Task AddAndActivatePP()
    {
        string powFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HST.pow");

        if (!File.Exists(powFilePath))
            throw new FileNotFoundException($"Power plan file not found: {powFilePath}");

        var importProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powercfg",
                Arguments = $"-import \"{powFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        importProcess.Start();
        string output = await importProcess.StandardOutput.ReadToEndAsync();
        string error = await importProcess.StandardError.ReadToEndAsync();
        importProcess.WaitForExit();

        if (importProcess.ExitCode != 0)
            return;

        string newGuid = ExtractGuidFromOutput(output);
        if (string.IsNullOrWhiteSpace(newGuid))
            return;

        var activateProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powercfg",
                Arguments = $"-setactive {newGuid}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        activateProcess.Start();
        string activationOutput = await activateProcess.StandardOutput.ReadToEndAsync();
        string activationError = await activateProcess.StandardError.ReadToEndAsync();
        activateProcess.WaitForExit();

        if (activateProcess.ExitCode != 0)
            return;
    }


    private string ExtractGuidFromOutput(string output)
    {
        var match = Regex.Match(output, @"[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}");
        return match.Success ? match.Value : null;
    }
}
