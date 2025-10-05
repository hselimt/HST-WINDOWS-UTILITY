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

        public async Task OptimizeTaskSchedulerAsync(string taskPath)
        {
            string[] commands = {
                $@"schtasks /end /tn ""{taskPath}""",
                $@"schtasks /change /tn ""{taskPath}"" /disable"
            };

            var tasks = new List<Task>();

            foreach (string command in commands)
            {
                tasks.Add(_removalTools.RunCommandAsync("cmd.exe", "/c " + command));
            }

            await Task.WhenAll(tasks);
        }

        public async Task DisableAllScheduledTasksAsync()
        {
            string[] tasks = {
                @"\GoogleSystem\GoogleUpdater",
                @"\Microsoft\Windows\ApplicationData\appuriverifierdaily",
                @"\Microsoft\Windows\ApplicationData\appuriverifierinstall",
                @"\Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser",
                @"\Microsoft\Windows\Application Experience\StartupAppTask",
                @"\Microsoft\Windows\Application Experience\MareBackup",
                @"\Microsoft\Windows\AppListBackup\Backup",
                @"\Microsoft\Windows\Autochk\Proxy",
                @"\Microsoft\Windows\Bluetooth\UninstallDeviceTask",
                @"\Microsoft\Windows\CloudExperienceHost\CreateObjectTask",
                @"\Microsoft\Windows\CloudRestore\Backup",
                @"\Microsoft\Windows\CloudRestore\Restore",
                @"\Microsoft\Windows\Customer Experience Improvement Program\Consolidator",
                @"\Microsoft\Windows\Customer Experience Improvement Program\UsbCeip",
                @"\Microsoft\Windows\Defrag\ScheduledDefrag",
                @"\Microsoft\Windows\Diagnosis\RecommendedTroubleshootingScanner",
                @"\Microsoft\Windows\Diagnosis\Scheduled",
                @"\Microsoft\Windows\DiskCleanup\SilentCleanup",
                @"\Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector",
                @"\Microsoft\Windows\DiskFootprint\Diagnostics",
                @"\Microsoft\Windows\DiskFootprint\StorageSense",
                @"\Microsoft\Windows\FileHistory\File History (maintenance mode)",
                @"\Microsoft\Windows\InstallService\RestoreDevice",
                @"\Microsoft\Windows\InstallService\ScanForUpdates",
                @"\Microsoft\Windows\InstallService\ScanForUpdatesAsUser",
                @"\Microsoft\Windows\InstallService\SmartRetry",
                @"\Microsoft\Windows\InstallService\WakeUpAndContinueUpdates",
                @"\Microsoft\Windows\InstallService\WakeUpAndScanForUpdates",
                @"\Microsoft\Windows\International\Synchronize Language Settings",
                @"\Microsoft\Windows\Maintenance\WinSAT",
                @"\Microsoft\Windows\Maps\MapsToastTask",
                @"\Microsoft\Windows\Maps\MapsUpdateTask",
                @"\Microsoft\Windows\MemoryDiagnostic\RunFullMemoryDiagnostic",
                @"\Microsoft\Windows\Offline Files\Background Synchronization",
                @"\Microsoft\Windows\Offline Files\Logon Synchronization",
                @"\Microsoft\Windows\Power Efficiency Diagnostics\AnalyzeSystem",
                @"\Microsoft\Windows\Printing\EduPrintProv",
                @"\Microsoft\Windows\Printing\PrinterCleanupTask",
                @"\Microsoft\Windows\Printing\PrintJobCleanupTask",
                @"\Microsoft\Windows\Shell\FamilySafetyMonitor",
                @"\Microsoft\Windows\Registry\RegIdleBackup",
                @"\Microsoft\Windows\Shell\FamilySafetyRefreshTask",
                @"\Microsoft\Windows\Time Synchronization\ForceSynchronizeTime",
                @"\Microsoft\Windows\Time Synchronization\SynchronizeTime",
                @"\Microsoft\Windows\Time Zone\SynchronizeTimeZone",
                @"\Microsoft\Windows\WaaSMedic\PerformRemediation",
                @"\Microsoft\Windows\Windows Defender\Windows Defender Cache Maintenance",
                @"\Microsoft\Windows\Windows Defender\Windows Defender Cleanup",
                @"\Microsoft\Windows\Windows Defender\Windows Defender Scheduled Scan",
                @"\Microsoft\Windows\Windows Defender\Windows Defender Verification",
                @"\Microsoft\Windows\Windows Error Reporting\QueueReporting",
                @"\Microsoft\Windows\Windows Media Sharing\UpdateLibrary",
                @"\Microsoft\Windows\WindowsUpdate\Scheduled Start",
                @"\Microsoft\XblGameSave\XblGameSaveTask"
            };

            var disableTasks = tasks.Select(task => OptimizeTaskSchedulerAsync(task)).ToArray();

            await Task.WhenAll(disableTasks);
        }
    }
}
