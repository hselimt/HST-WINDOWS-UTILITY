namespace HST.Controllers.Helpers
{
    public static class Logger
    {
        // Defines the path for the log file
        private static string LogPath => Path.Combine(
            Path.GetTempPath(),
            "HST-WINDOWS-UTILITY.log");

        public static void InitializeLog()
        {
            try
            {
                // Deletes the existing log file
                if (File.Exists(LogPath))
                {
                    File.Delete(LogPath);
                }
            }
            catch
            {
                // Silently ignore errors
            }
        }

        // Appends a timestamped message to the log file
        public static void Log(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(LogPath, logEntry);
            }
            catch
            {
                // Silently ignores errors
            }
        }

        // Log structures
        public static void Error(string operation, Exception ex)
        {
            Log($"ERROR in {operation}");
            Log($"  MESSAGE: {ex.Message}");
            Log($"  STACK: {ex.StackTrace}");
            Log("----------------------------------------");
        }

        public static void Success(string operation)
        {
            Log($"SUCCESS: {operation}");
        }

        // Returns the file path of the log file
        public static string GetLogLocation()
        {
            return LogPath;
        }
    }
}