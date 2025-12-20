using System.Text;
using System.Text.Json;
using HST.Controllers.RemovalTools;
using Logger = HST.Controllers.Tool.Logger;

namespace HST.Controllers.TaskSchedulerOptimizerMethods
{
    public class TaskSchedulerOptimizer
    {
        private readonly RemovalHelpers _removalTools;

        public TaskSchedulerOptimizer(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        // Disables configured Windows scheduled tasks
        public async Task DisableAllScheduledTasksAsync()
        {
            Logger.Log("Starting to disable scheduled tasks");
            try
            {
                await ProcessTasksAsync(disable: true);
                Logger.Success("Disabling scheduled tasks complete");
            }
            catch (Exception ex)
            {
                Logger.Error("DisableAllScheduledTasks", ex);
            }
        }

        // Re-enables previously disabled scheduled tasks
        public async Task DisableAllScheduledTasksRevertAsync()
        {
            Logger.Log("Starting to enable scheduled tasks");
            try
            {
                await ProcessTasksAsync(disable: false);
                Logger.Success("Enabling scheduled tasks complete");
            }
            catch (Exception ex)
            {
                Logger.Error("DisableAllScheduledTasksRevert", ex);
            }
        }

        private async Task ProcessTasksAsync(bool disable)
        {
            try
            {
                var json = await File.ReadAllTextAsync(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScheduledTasksConfig.json"));
                var config = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                var tasks = config["tasks"];

                var taskList = string.Join("', '", tasks.Select(t => t.Replace("'", "''")));
                var command = disable
                    ? "Stop-ScheduledTask -TaskName $task -ErrorAction SilentlyContinue; Disable-ScheduledTask -TaskName $task -ErrorAction SilentlyContinue"
                    : "Enable-ScheduledTask -TaskName $task -ErrorAction SilentlyContinue";

                string psScript = $@"
                $tasks = @('{taskList}')
                foreach ($task in $tasks) {{
                    {command}
                }}";

                var scriptBytes = Encoding.Unicode.GetBytes(psScript);
                var encodedScript = Convert.ToBase64String(scriptBytes);

                await _removalTools.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encodedScript}");
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessTasks", ex);
            }
        }
    }
}