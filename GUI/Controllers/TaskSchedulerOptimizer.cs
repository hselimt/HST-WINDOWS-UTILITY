using HST.Controllers.RemovalTools;
using System.Diagnostics;
using System.Text.Json;

#pragma warning disable CS8600
#pragma warning disable CS8602

namespace HST.Controllers.TaskSchedulerOptimizerMethods
{
    public class TaskSchedulerOptimizer
    {
        private readonly RemovalHelpers _removalTools;

        public TaskSchedulerOptimizer(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        public async Task DisableAllScheduledTasksAsync()
        {
            var json = await File.ReadAllTextAsync(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScheduledTasksConfig.json"));
            var config = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
            var tasksToDisable = config["tasks"];

            string taskList = string.Join("', '", tasksToDisable);
            string disableTasksScript = $@"
            $ErrorActionPreference = 'SilentlyContinue'
            $tasks = @('{taskList}')
            foreach ($task in $tasks) {{
                Stop-ScheduledTask -TaskName $task
                Disable-ScheduledTask -TaskName $task
            }}
            ";

            await _removalTools.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{disableTasksScript.Replace("\"", "\"\"")}\"");
        }
    }
}