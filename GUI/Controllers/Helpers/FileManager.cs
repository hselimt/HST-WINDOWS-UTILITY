namespace HST.Controllers.Helpers
{
    public static class FileManager
    {
        // Deletes directory if it exists
        public static void DeleteDirectoryIfExists(string dirPath)
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
                Logger.Log($"Failed to delete directory {dirPath}: {ex.Message}");
            }
        }

        // Deletes file if it exists, supports environment variables
        public static void DeleteFileIfExists(string filePath)
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
                Logger.Log($"Failed to delete file {filePath}: {ex.Message}");
            }
        }
    }
}