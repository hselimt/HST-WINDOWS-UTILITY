using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            string powFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HST.pow");
            string tempOutputFilePath = Path.Combine(Path.GetTempPath(), $"powercfg_output_{Guid.NewGuid()}.txt");

            try
            {
                if (!File.Exists(powFilePath))
                {
                    Debug.WriteLine($"HST.pow file not found at: {powFilePath}");
                    return;
                }

                bool imported = await _removalTools.RunCommandAsync(
                    "cmd.exe",
                    $"/c powercfg -import \"{powFilePath}\" 2>&1 > \"{tempOutputFilePath}\""
                );

                if (!imported)
                {
                    Debug.WriteLine("Failed to import power plan");
                    return;
                }

                await Task.Delay(200);

                if (!File.Exists(tempOutputFilePath))
                {
                    Debug.WriteLine("Output file was not created");
                    return;
                }

                string output = await File.ReadAllTextAsync(tempOutputFilePath);
                Debug.WriteLine($"PowerCfg output: {output}");

                string? newGuid = ExtractGuidFromOutput(output);

                if (string.IsNullOrWhiteSpace(newGuid))
                {
                    Debug.WriteLine($"Failed to extract GUID from output: {output}");
                    return;
                }

                Debug.WriteLine($"Extracted GUID: {newGuid}");

                bool activated = await _removalTools.RunCommandAsync("powercfg", $"-setactive {newGuid}");

                if (!activated)
                {
                    Debug.WriteLine("Failed to activate power plan");
                    return;
                }

                Debug.WriteLine("Power plan successfully imported and activated!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error with power plan: {ex.Message}");
            }
            finally
            {
                if (File.Exists(tempOutputFilePath))
                {
                    try { File.Delete(tempOutputFilePath); } catch { }
                }
            }
        }

        private string? ExtractGuidFromOutput(string output)
        {
            var match = Regex.Match(output, @"[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}");
            return match.Success ? match.Value : null;
        }
    }
}