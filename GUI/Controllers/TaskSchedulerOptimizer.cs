using System.Text;
using System.Text.Json;
using HST.Controllers.RemovalTools;

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
            await ProcessTasksAsync(disable: true);
        }

        public async Task DisableAllScheduledTasksRevertAsync()
        {
            await ProcessTasksAsync(disable: false);
        }

        private async Task ProcessTasksAsync(bool disable)
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

            await _removalTools.RunCommandAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encodedScript}");
        }
    }
}