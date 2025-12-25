using System.Diagnostics;

namespace HST.Controllers.Helpers
{
    public class ProcessRunner
    {
        // Executes commands asynchronously with admin rights.
        public async Task<bool> RunCommandAsync(string command, string arguments, int timeoutMs = 30000)
        {
            try
            {
                // Creates a new Process instance wrapped in a using statement
                using (var process = new Process())
                {
                    // Configures how the process should be started
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = command, // Configures the process to start
                        Arguments = arguments, // Arguments to pass to the application
                        UseShellExecute = true, // UseShellExecute is set to true to allow the use of the "runas" verb for admin elevation
                        Verb = "runas", // "runas" requests Administrator privileges
                        CreateNoWindow = true, // Configuration to keep the execution window hidden from the user
                        WindowStyle = ProcessWindowStyle.Hidden // Configuration to keep the execution window hidden from the user
                    };
                    process.Start();

                    // Creates a CancellationTokenSource that cancels after the specified timeout (30 secs)
                    using var cts = new CancellationTokenSource(timeoutMs);

                    // Asynchronously waits for the process to exit, passing the cancellation token.
                    // If the timeout is reached, the token cancels and OperationCanceledException is thrown.
                    await process.WaitForExitAsync(cts.Token);
                    return process.ExitCode == 0;
                }
            }
            catch (OperationCanceledException ex) // Handles the specific case where the process took longer than timeoutMs
            {
                Logger.Error("RunCommand timeout", ex);
                throw new Exception("RunCommand timeout", ex);
            }
            catch (Exception ex) // Handles other errors
            {
                Logger.Error("RunCommand", ex);
                throw new Exception("RunCommand operation failed", ex);
            }
        }
    }
}