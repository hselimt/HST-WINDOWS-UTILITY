using System.Text.Json;

namespace HST.Controllers.Helpers
{
    public static class ConfigLoader
    {
        // Gets the base directory of the application for file paths
        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;

        public static async Task<Dictionary<string, List<string>>> LoadStringConfigAsync(string filename)
        {
            // Reads the JSON file text asynchronously
            var json = await File.ReadAllTextAsync(Path.Combine(_baseDir, filename));
            // Deserializes the JSON into a dictionary of string lists
            return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
        }

        public static async Task<Dictionary<string, List<T>>> LoadConfigAsync<T>(string filename)
        {
            // Reads the file content
            var json = await File.ReadAllTextAsync(Path.Combine(_baseDir, filename));

            // Configures options to handle comments and trailing commas in JSON
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // Deserializes JSON with options
            return JsonSerializer.Deserialize<Dictionary<string, List<T>>>(json, options);
        }
    }
}