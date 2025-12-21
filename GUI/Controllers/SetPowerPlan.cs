using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.PowerPlan
{
    public class SetPowerPlan
    {
        private readonly RemovalHelpers _removalTools;

        public SetPowerPlan(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        // Imports, extracts GUID and activates HST power plan
        public async Task AddAndActivatePowerplanAsync()
        {
            string powFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HST.pow");
            string tempOutputFilePath = Path.Combine(Path.GetTempPath(), $"powercfg_output_{Guid.NewGuid()}.txt");

            Logger.Log("Starting to add and activate powerplan");
            try
            {
                if (!File.Exists(powFilePath))
                {
                    Logger.Log($"HST.pow file not found at: {powFilePath}");
                    throw new FileNotFoundException("HST.pow file not found");
                }

                bool imported = await _removalTools.RunCommandAsync(
                    "cmd.exe",
                    $"/c powercfg -import \"{powFilePath}\" 2>&1 > \"{tempOutputFilePath}\""
                );

                if (!imported)
                {
                    Logger.Log("Failed to import power plan");
                    throw new Exception("Failed to import power plan");
                }

                await Task.Delay(200);

                if (!File.Exists(tempOutputFilePath))
                {
                    Logger.Log("Output file was not created");
                    throw new Exception("Output file was not created");
                }

                string output = await File.ReadAllTextAsync(tempOutputFilePath);
                Logger.Log($"PowerCfg output: {output}");

                string? newGuid = ExtractGuidFromOutput(output);

                if (string.IsNullOrWhiteSpace(newGuid))
                {
                    Logger.Log($"Failed to extract GUID from output: {output}");
                    throw new Exception("Failed to extract GUID from output");
                }

                Logger.Log($"Extracted GUID: {newGuid}");

                bool activated = await _removalTools.RunCommandAsync("powercfg", $"-setactive {newGuid}");

                if (!activated)
                {
                    Logger.Log("Failed to activate power plan");
                    throw new Exception("Failed to activate power plan");
                }

                Logger.Success("Power plan successfully imported and activated");
            }
            catch (Exception ex)
            {
                Logger.Error("AddAndActivatePowerplanAsync", ex);
                throw;
            }
            finally
            {
                if (File.Exists(tempOutputFilePath))
                {
                    try { File.Delete(tempOutputFilePath); } catch { }
                }
            }
        }

        // Extracts power plan GUID from powercfg output
        private string? ExtractGuidFromOutput(string output)
        {
            var match = Regex.Match(output, @"[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}");
            return match.Success ? match.Value : null;
        }
    }
}