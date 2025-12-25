using HST.Controllers.Helpers;

namespace HST.Controllers.System
{
    public class RestorePointCreator
    {
        private readonly ProcessRunner _processRunner;

        public RestorePointCreator(ProcessRunner processRunner)
        {
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        }

        // Creates system restore point using PowerShell
        public async Task<bool> CreateRestorePointAsync()
        {
            Logger.Log("Starting restore point creation");
            string createRestorePointScript = @"
                Set-ItemProperty -Path 'HKLM:\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore' -Name 'SystemRestorePointCreationFrequency' -Value 0 -Type DWord -Force
                Enable-ComputerRestore -Drive $env:SystemDrive
                Checkpoint-Computer -Description 'HST-WINDOWS-UTILITY' -RestorePointType 'MODIFY_SETTINGS'
            ";

            try
            {
                bool result = await _processRunner.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{createRestorePointScript.Replace("\"", "\"\"")}\"");

                if (result)
                    Logger.Success("Restore point created");
                else
                    Logger.Log("Restore point creation failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("CreateRestorePoint", ex);
                throw new Exception("Failed to create restore point", ex);
            }
        }
    }
}