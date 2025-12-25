using HST.Controllers.Helpers;

namespace HST.Controllers.System
{
    public class TaskSchedulerOptimizer
    {
        private readonly ProcessRunner _processRunner;

        public TaskSchedulerOptimizer(ProcessRunner processRunner)
        {
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
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
                var config = await ConfigLoader.LoadStringConfigAsync("ScheduledTasksConfig.json");
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

                await _processRunner.RunCommandAsync("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"{psScript.Replace("\"", "\"\"")}\"");
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessTasks", ex);
            }
        }
    }
}