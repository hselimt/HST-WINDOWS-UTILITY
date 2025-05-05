using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using System.Management.Automation;
using Microsoft.Win32;
using System.ServiceProcess;
using System.Management;
using Microsoft.VisualBasic.Devices;
using System.Drawing.Text;
using OptimizeTaskSchedulerApp;
using SetServicesApp;
using CleanupApp;
using RemovalTools;
using PowerPlanApp;

namespace HST_WINDOWS_UTILITY
{
    public partial class Form1 : Form
    {
        private readonly RemovalHelpers _removalTools;

        public Form1()
        {
            InitializeComponent();
            _removalTools = new RemovalHelpers();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadHardwareInfo();
        }
        public void LoadHardwareInfo()
        {
            string Username = $"{Environment.UserName}";

            if (!string.IsNullOrWhiteSpace(Username))
            {
                tbStatus.Text = $"WELCOME {Username} PLEASE CREATE RESTORE POINT TO REVERT CHANGES IF NEEDED";
            }
            else
            {
                tbStatus.Text = "WELCOME USER PLEASE CREATE RESTORE POINT TO REVERT CHANGES IF NEEDED";
            }

            try
            {
                var gpu = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>().First();
                tbGPU.Text = $"{gpu["Name"]}";
            }
            catch
            {
                tbGPU.Text = "ERROR";
            }

            try
            {
                var cpu = new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>().First();
                tbCPU.Text = $"{cpu["Name"]}";
            }
            catch
            {
                tbCPU.Text = "ERROR";
            }

            try
            {
                double ramGB = new ComputerInfo().TotalPhysicalMemory / (1024.0 * 1024 * 1024);
                tbRAM.Text = $"{ramGB:F2}GB RAM";
                //ramKB = (long)(new ComputerInfo().TotalPhysicalMemory / 1024);
            }
            catch
            {
                tbRAM.Text = "ERROR";
            }

            try
            {
                var drive = new DriveInfo("C");
                double freeGB = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
                tbES.Text = $"{freeGB:F2}GB EMPTY SPACE";
            }
            catch
            {
                tbES.Text = "ERROR";
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void llGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://www.github.com/hselimt",
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception)
            {
                MessageBox.Show("COULDN'T VISIT LINK", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnCRP_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("systempropertiesprotection");
            }
            catch (Exception)
            {
            }
        }
        private async void btnREG_Click(object sender, EventArgs e)
        {
            tbStatus.Text = "OPTIMIZING REGISTRY";

            // --- Setting  svchost.exe RAM threshold ---
            /*if (ramKB > (5L * 1024L * 1024L * 1024L) && ramKB < (150L * 1024L * 1024L * 1024L))
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\ControlSet001\Control"))
                {
                    //key.SetValue("SvcHostSplitThresholdInKB", ramKB, RegistryValueKind.DWord);
                }
            }*/

            // --- Setting Win32Priority ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\PriorityControl"))
            {
                key.SetValue("Win32PrioritySeparation", 0x26, RegistryValueKind.DWord);
            }

            // --- Disabling Fast Startup ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power"))
            {
                key.SetValue("HiberbootEnabled", 0x0, RegistryValueKind.DWord);
            }

            // --- Disabling Hibernation ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power"))
            {
                key.SetValue("HibernateEnabled", 0x0, RegistryValueKind.DWord);
            }

            // --- Disabling Automatic Maintenance ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"))
            {
                key.SetValue("MaintenanceDisabled", 0x1, RegistryValueKind.DWord);
            }

            // --- Disabling PowerThrottling ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"))
            {
                key.SetValue("PowerThrottlingOff", 0x1, RegistryValueKind.DWord);
            }

            // --- Disabling HAGS ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers"))
            {
                key.SetValue("HwSchMode", 0x1, RegistryValueKind.DWord);
            }

            // --- Disable Game Mode ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\GameBar"))
            {
                key.SetValue("AllowAutoGameMode", 0x0, RegistryValueKind.DWord);
                key.SetValue("AutoGameModeEnabled", 0x0, RegistryValueKind.DWord);
            }

            // --- Disable XBOX Gamebar ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"System\GameConfigStore"))
            {
                key.SetValue("GameDVR_Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_FSEBehaviorMode", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_FSEBehavior", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_HonorUserFSEBehaviorMode", 0x1, RegistryValueKind.DWord);
                key.SetValue("GameDVR_DXGIHonorFSEWindowsCompatible", 0x1, RegistryValueKind.DWord);
                key.SetValue("GameDVR_EFSEFeatureFlags", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_DSEBehavior", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR"))
            {
                key.SetValue("AllowGameDVR", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"))
            {
                key.SetValue("AppCaptureEnabled", 0x0, RegistryValueKind.DWord);
            }

            // --- Reducing Menu Delay ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
            {
                key.SetValue("MenuShowDelay", 0x0, RegistryValueKind.DWord);
            }

            // --- Lower Latency ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"))
            {
                key.SetValue("NoLazyMode", 0x1, RegistryValueKind.DWord);
                key.SetValue("AlwaysOn", 0x1, RegistryValueKind.DWord);
                key.SetValue("NetworkThrottlingIndex", unchecked((int)0xffffffff), RegistryValueKind.DWord);
                key.SetValue("SystemResponsiveness", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"))
            {
                key.SetValue("Affinity", 0x0, RegistryValueKind.DWord);
                key.SetValue("Background Only", "False", RegistryValueKind.String);
                key.SetValue("GPU Priority", 0x8, RegistryValueKind.DWord);
                key.SetValue("Priority", 0x6, RegistryValueKind.DWord);
                key.SetValue("Scheduling Category", "High", RegistryValueKind.String);
                key.SetValue("SFIO Priority", "High", RegistryValueKind.String);
                key.SetValue("Latency Sensitive", "True", RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power"))
            {
                key.SetValue("ExitLatency", 0x1, RegistryValueKind.DWord);
                key.SetValue("ExitLatencyCheckEnabled", 0x1, RegistryValueKind.DWord);
                key.SetValue("Latency", 0x1, RegistryValueKind.DWord);
                key.SetValue("LatencyToleranceDefault", 0x1, RegistryValueKind.DWord);
                key.SetValue("LatencyToleranceFSVP", 0x1, RegistryValueKind.DWord);
                key.SetValue("LatencyTolerancePerfOverride", 0x1, RegistryValueKind.DWord);
                key.SetValue("LatencyToleranceScreenOffIR", 0x1, RegistryValueKind.DWord);
                key.SetValue("RtlCapabilityCheckLatency", 0x1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power"))
            {
                key.SetValue("DefaultD3TransitionLatencyActivelyUsed", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultD3TransitionLatencyIdleLongTime", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultD3TransitionLatencyIdleMonitorOff", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultD3TransitionLatencyIdleNoContext", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultD3TransitionLatencyIdleShortTime", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultD3TransitionLatencyIdleVeryLongTime", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceIdle0", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceIdle0MonitorOff", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceIdle1", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceIdle1MonitorOff", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceMemory", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceNoContext", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceNoContextMonitorOff", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceOther", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultLatencyToleranceTimerPeriod", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultMemoryRefreshLatencyToleranceActivelyUsed", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultMemoryRefreshLatencyToleranceMonitorOff", 0x1, RegistryValueKind.DWord);
                key.SetValue("DefaultMemoryRefreshLatencyToleranceNoContext", 0x1, RegistryValueKind.DWord);
                key.SetValue("Latency", 0x1, RegistryValueKind.DWord);
                key.SetValue("MaxIAverageGraphicsLatencyInOneBucket", 0x1, RegistryValueKind.DWord);
                key.SetValue("MiracastPerfTrackGraphicsLatency", 0x1, RegistryValueKind.DWord);
                key.SetValue("MonitorLatencyTolerance", 0x1, RegistryValueKind.DWord);
                key.SetValue("MonitorRefreshLatencyTolerance", 0x1, RegistryValueKind.DWord);
                key.SetValue("TransitionLatency", 0x1, RegistryValueKind.DWord);
            }

            // --- Removing Windows Content ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"))
            {
                key.SetValue("SubscribedContent-338393Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-314559Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-280815Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-202914Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-353694Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-353696Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-338387Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-353698Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-338389Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-310093Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-338388Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-314563Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("RotatingLockScreenOverlayEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("RotatingLockScreenEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("ContentDeliveryAllowed", 0x0, RegistryValueKind.DWord);
                key.SetValue("OemPreInstalledAppsEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("PreInstalledAppsEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("PreInstalledAppsEverEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SilentInstalledAppsEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SoftLandingEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContentEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("FeatureManagementEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SystemPaneSuggestionsEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("RemediationRequired", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy"))
            {
                key.SetValue("TailoredExperiencesWithDiagnosticDataEnabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"))
            {
                key.SetValue("EnableFeeds", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                key.SetValue("ShowOrHideMostUsedApps", 0x2, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CloudContent"))
            {
                key.SetValue("ConfigureWindowsSpotlight", 0x2, RegistryValueKind.DWord);
                key.SetValue("IncludeEnterpriseSpotlight", 0x0, RegistryValueKind.DWord);
                key.SetValue("DisableWindowsSpotlightFeatures", 0x1, RegistryValueKind.DWord);
                key.SetValue("DisableWindowsSpotlightWindowsWelcomeExperience", 0x1, RegistryValueKind.DWord);
                key.SetValue("DisableWindowsSpotlightOnActionCenter", 0x1, RegistryValueKind.DWord);
                key.SetValue("DisableWindowsSpotlightOnSettings", 0x1, RegistryValueKind.DWord);
                key.SetValue("DisableThirdPartySuggestions", 0x1, RegistryValueKind.DWord);
                key.SetValue("DisableTailoredExperiencesWithDiagnosticData", 0x1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CloudContent"))
            {
                key.SetValue("DisableThirdPartySuggestions", 0x2, RegistryValueKind.DWord);
                key.SetValue("DisableWindowsConsumerFeatures", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Dsh"))
            {
                key.SetValue("AllowNewsAndInterests", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"))
            {
                key.SetValue("DisableStartupSound", 0x1, RegistryValueKind.DWord);
            }

            // --- Disable Remote Assistance ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Remote Assistance"))
            {
                key.SetValue("fAllowToGetHelp", 0x0, RegistryValueKind.DWord);
            }

            // --- Disabling Frequent/Recent Files ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"))
            {
                key.SetValue("ShowFrequent", 0x0, RegistryValueKind.DWord);
                key.SetValue("ShowRecent", 0x0, RegistryValueKind.DWord);
                key.SetValue("TelemetrySalt", 0x0, RegistryValueKind.DWord);
                key.SetValue("NoRecentDocsHistory", 0x1, RegistryValueKind.DWord);
            }

            // --- Disable Map Update ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\Maps"))
            {
                key.SetValue("AutoUpdateEnabled", 0x0, RegistryValueKind.DWord);
            }

            // --- Disable Driver Updates ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"))
            {
                key.SetValue("SearchOrderConfig", 0x0, RegistryValueKind.DWord);
                key.SetValue("DontSearchWindowsUpdate", 0x1, RegistryValueKind.DWord);
            }

            // --- Disable Telemetry ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection"))
            {
                key.SetValue("AllowTelemetry", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection"))
            {
                key.SetValue("AllowTelemetry", 0x0, RegistryValueKind.DWord);
                key.SetValue("AllowDeviceNameInTelemetry", 0x0, RegistryValueKind.DWord);
                key.SetValue("AllowCommercialDataPipeline", 0x0, RegistryValueKind.DWord);
                key.SetValue("LimitEnhancedDiagnosticDataWindowsAnalytics", 0x0, RegistryValueKind.DWord);
                key.SetValue("DoNotShowFeedbackNotifications", 0x1, RegistryValueKind.DWord);
            }

            // --- Disabling History Logging ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search"))
            {
                key.SetValue("HistoryViewEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("DeviceHistoryEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("BingSearchEnabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings"))
            {
                key.SetValue("IsDeviceSearchHistoryEnabled", 0x0, RegistryValueKind.DWord);
            }

            // --- Disabling Notifications ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\PushNotifications"))
            {
                key.SetValue("ToastEnabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance"))
            {
                key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\windows.immersivecontrolpanel_cw5n1h2txyewy!microsoft.windows.immersivecontrolpanel"))
            {
                key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.CapabilityAccess"))
            {
                key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                key.SetValue("DisableNotificationCenter", 0x1, RegistryValueKind.DWord);
            }

            // --- Disable Background Access ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"))
            {
                key.SetValue("GlobalUserDisabled", 0x1, RegistryValueKind.DWord);
            }

            // --- Privacy Settings ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Personalization\Settings"))
            {
                key.SetValue("AcceptedPrivacyPolicy", 0x0, RegistryValueKind.DWord);
                key.SetValue("RestrictImplicitInkCollection", 0x1, RegistryValueKind.DWord);
                key.SetValue("RestrictImplicitTextCollection", 0x1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack\EventTranscriptKey"))
            {
                key.SetValue("EnableEventTranscript", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Siuf\Rules"))
            {
                key.SetValue("NumberOfSIUFInPeriod", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System"))
            {
                key.SetValue("PublishUserActivities", 0x0, RegistryValueKind.DWord);
                key.SetValue("UploadUserActivities", 0x0, RegistryValueKind.DWord);
                key.SetValue("EnableActivityFeed", 0x0, RegistryValueKind.DWord);
            }

            // --- Privacy Settings2 ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo"))
            {
                key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"))
            {
                key.SetValue("Disabled", 0x1, RegistryValueKind.DWord);
            }

            // Capability Access Manager Consent Store
            string[] consentStoreKeys = new string[]
            {
            "appointments", "appDiagnostics", "broadFileSystemAccess", "bluetoothSync", "chat", "contacts",
            "documentsLibrary", "downloadsFolder", "email", "graphicsCaptureProgrammatic", "graphicsCaptureWithoutBorder",
            "location", "musicLibrary", "phoneCall", "phoneCallHistory", "picturesLibrary", "radios", "webcam",
            "userAccountInformation", "userDataTasks", "userNotificationListener", "videosLibrary"
            };

            foreach (string subKey in consentStoreKeys)
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\{subKey}"))
                {
                    key.SetValue("Value", subKey == "microphone" ? "Allow" : "Deny", RegistryValueKind.String);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"))
            {
                key.SetValue("HasAccepted", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"))
            {
                key.SetValue("AgentActivationEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("AgentActivationLastUsed", 0x0, RegistryValueKind.DWord);
            }

            // --- Disable Sync ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync"))
            {
                key.SetValue("SyncPolicy", 0x5, RegistryValueKind.DWord);
            }

            string[] syncGroups = new string[] { "Accessibility", "AppSync", "Personalization", "BrowserSettings", "Credentials", "Language", "Windows" };

            foreach (string group in syncGroups)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\{group}"))
                {
                    key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
                }
            }

            // --- Disable Error Reporting ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"))
            {
                key.SetValue("Disabled", 0x1, RegistryValueKind.DWord);
                key.SetValue("DoReport", 0x0, RegistryValueKind.DWord);
            }

            // --- Disable Mouse Accel ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Mouse"))
            {
                key.SetValue("MouseSpeed", "0", RegistryValueKind.String);
                key.SetValue("MouseThreshold1", "0", RegistryValueKind.String);
                key.SetValue("MouseThreshold2", "0", RegistryValueKind.String);
            }

            // --- Disable Storage Sense ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\StorageSense"))
            {
                key.SetValue("AllowStorageSenseGlobal", 0x0, RegistryValueKind.DWord);
            }

            // --- Disable Browser Stuff ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Edge"))
            {
                key.SetValue("StartupBoostEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("HardwareAccelerationModeEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("BackgroundModeEnabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\MicrosoftEdgeElevationService"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\edgeupdate"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\edgeupdatem"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\MicrosoftEdgeUpdateTaskMachineCore", false);
            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\MicrosoftEdgeUpdateTaskMachineUA", false);

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Google\Chrome"))
            {
                key.SetValue("StartupBoostEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("HardwareAccelerationModeEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("BackgroundModeEnabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\GoogleChromeElevationService"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\gupdate"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\gupdatem"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            // --- Disable Startup Apps ---
            Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

            // --- Disable Store Auto Downloads ---
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\WindowsStore"))
            {
                key.SetValue("AutoDownload", 0x2, RegistryValueKind.DWord);
            }

            // --- Customize Taskbar ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"))
            {
                key.SetValue("TaskbarAl", 0x1, RegistryValueKind.DWord);
                key.SetValue("TaskbarMn", 0x0, RegistryValueKind.DWord);
                key.SetValue("ShowTaskViewButton", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SearchSettings"))
            {
                key.SetValue("IsDynamicSearchBoxEnabled", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Search"))
            {
                key.SetValue("SearchboxTaskbarMode", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
            {
                key.SetValue("HideSCAMeetNow", 0x1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"))
            {
                key.SetValue("ShowLockOption", 0x0, RegistryValueKind.DWord);
                key.SetValue("ShowSleepOption", 0x0, RegistryValueKind.DWord);
            }

            // --- Switch to Dark Mode ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                key.SetValue("EnableTransparency", 0x0, RegistryValueKind.DWord);
            }

            // --- Disable Sound Alerts ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"AppEvents\Schemes"))
            {
                key.SetValue("", ".None", RegistryValueKind.String);
            }

            string[] soundEvents = new string[]
            {
            @".Default\.Default\.Current", @"CriticalBatteryAlarm\.Current", @"DeviceConnect\.Current",
            @"DeviceDisconnect\.Current", @"DeviceFail\.Current", @"FaxBeep\.Current", @"LowBatteryAlarm\.Current",
            @"MailBeep\.Current", @"MessageNudge\.Current", @"Notification.Default\.Current",
            @"Notification.IM\.Current", @"Notification.Mail\.Current", @"Notification.Proximity\.Current",
            @"Notification.Reminder\.Current", @"Notification.SMS\.Current", @"ProximityConnection\.Current",
            @"SystemAsterisk\.Current", @"SystemExclamation\.Current", @"SystemHand\.Current",
            @"SystemNotification\.Current", @"WindowsUAC\.Current", @"sapisvr\DisNumbersSound\.current",
            @"sapisvr\HubOffSound\.current", @"sapisvr\HubOnSound\.current", @"sapisvr\HubSleepSound\.current",
            @"sapisvr\MisrecoSound\.current", @"sapisvr\PanelSound\.current"
            };

            foreach (string eventPath in soundEvents)
            {
                Registry.CurrentUser.DeleteSubKeyTree($@"AppEvents\Schemes\Apps\.Default\{eventPath}", false);
                Registry.CurrentUser.CreateSubKey($@"AppEvents\Schemes\Apps\.Default\{eventPath}");
            }
            tbStatus.Text = "REGISTRY OPTIMIZED";
            await Task.Delay(1000);
            tbStatus.Text = "";
        }
        private async void btnTS_Click(object sender, EventArgs e)
        {
            var optimizeTaskScheduler = new OptimizeTaskScheduler();

            tbStatus.Text = "OPTIMIZING TASK SCHEDULER";
            await optimizeTaskScheduler.DisableAllScheduledTasksAsync();
            tbStatus.Text = "TASK SCHEDULER HAS BEEN OPTIMIZED";
            await Task.Delay(1000);
            tbStatus.Text = "";
        }
        private async void btnDU_Click(object sender, EventArgs e)
        {
            try
            {
                btnDU.Enabled = false;

                var app = new DisableWindowsUpdatesApp();
                await app.DisableWindowsUpdates(tbStatus);

                tbStatus.Text = "DISABLED WINDOWS UPDATES";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text += $"Critical error: {ex.Message}";
            }
            finally
            {
                btnDU.Enabled = true;
            }
        }
        private async void btnLV_Click(object sender, EventArgs e)
        {
            tbStatus.Text = "LOWERING VISUALS";
            // --- Visuals Effects ---
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"))
            {
                key.SetValue("VisualFXSetting", 0x3, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
            {
                key.SetValue("UserPreferencesMask", new byte[] { 0x90, 0x12, 0x03, 0x80, 0x10, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
                key.SetValue("MinAnimate", "0", RegistryValueKind.String);
                key.SetValue("DragFullWindows", "0", RegistryValueKind.String);
                key.SetValue("FontSmoothing", "2", RegistryValueKind.String);
                key.SetValue("WindowAnimation", 0x0, RegistryValueKind.DWord);
                key.SetValue("MenuAnimation", 0x0, RegistryValueKind.DWord);
                key.SetValue("TaskbarAnimations", 0x0, RegistryValueKind.DWord);
                key.SetValue("IconAnimation", 0x0, RegistryValueKind.DWord);
                key.SetValue("ScrollAnimation", 0x0, RegistryValueKind.DWord);
                key.SetValue("ScrollSmoothness", 0x1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"))
            {
                key.SetValue("TaskbarAnimations", 0x0, RegistryValueKind.DWord);
                key.SetValue("IconsOnly", 0x1, RegistryValueKind.DWord);
                key.SetValue("ListviewAlphaSelect", 0x0, RegistryValueKind.DWord);
                key.SetValue("ListviewShadow", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\DWM"))
            {
                key.SetValue("EnableAeroPeek", 0x0, RegistryValueKind.DWord);
                key.SetValue("AlwaysHibernateThumbnails", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop\WindowMetrics"))
            {
                key.SetValue("MinAnimate", "0", RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"))
            {
                key.SetValue("StartupDelayInMSec", 0x0, RegistryValueKind.DWord);
            }
            await Task.Delay(1000);

            tbStatus.Text = "";
        }
        private async void btnDM_Click(object sender, EventArgs e)
        {
            tbStatus.Text = "SWITCHING TO DARKMODE";
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                key.SetValue("AppsUseLightTheme", 0, RegistryValueKind.DWord);
                key.SetValue("SystemUsesLightTheme", 0, RegistryValueKind.DWord);
            }

            await Task.Delay(1000);

            tbStatus.Text = "";
        }
        private async void btnAPP_Click(object sender, EventArgs e)
        {
            btnAPP.Enabled = false;
            var removalHelpers = new RemovalTools.RemovalHelpers();
            var _powerPlan = new PowerPlan(removalHelpers);

            try
            {
                await _powerPlan.AddAndActivatePP();
                tbStatus.Text = "ACTIVATED POWERPLAN";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = "ERROR ACTIVATING POWERPLAN";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnSS_Click(object sender, EventArgs e)
        {
            try
            {
                tbStatus.Text = "OPTIMIZING SERVICES";
                var setServices = new SetServices();
                var additionalServices = new List<string>();

                if (clbSS.GetItemChecked(0))
                {
                    additionalServices.AddRange(new[] { "BTAGService", "bthserv", "BluetoothUserService" });
                }

                if (clbSS.GetItemChecked(1))
                {
                    additionalServices.AddRange(new[] { "HvHost", "vmickvpexchange", "vmicguestinterface" });
                }

                if (clbSS.GetItemChecked(2))
                {
                    additionalServices.AddRange(new[] { "XboxGipSvc", "XblAuthManager", "XblGameSave", "XboxNetApiSvc" });
                }

                await setServices.DisableMultipleServices(additionalServices.ToArray());
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR SETTING SERVICES", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            tbStatus.Text = "SERVICES ARE OPTIMIZED";
            await Task.Delay(1000);
            tbStatus.Text = "";
        }
        /*private async void btnDEB_Click(object sender, EventArgs e)
        {
            if (clbDEB.CheckedItems.Count == 0)
            {
                tbStatus.Text = "YOU HAVEN'T CHECKED ANY BOX";
                await Task.Delay(1000);
                tbStatus.Text = "";
                return;
            }

            for (int i = 0; i < clbDEB.Items.Count; i++)
            {
                if (clbDEB.GetItemChecked(i))
                {
                    switch (i)
                    {
                        case 0:
                            tbStatus.Text = "REMOVING MICROSOFT PACKAGES";
                            await Task.Delay(1000);
                            tbStatus.Text = "";
                            break;
                        case 1:
                            tbStatus.Text = "REMOVING EDGE";
                            await Task.Delay(1000);
                            tbStatus.Text = "";
                            break;
                        case 2:
                            tbStatus.Text = "REMOVING ONEDRIVE";
                            await Task.Delay(1000);
                            tbStatus.Text = "";
                            break;
                        case 3:
                            tbStatus.Text = "REMOVING XBOX";
                            await Task.Delay(1000);
                            tbStatus.Text = "";
                            break;
                        case 4:
                            tbStatus.Text = "REMOVING STORE";
                            await Task.Delay(1000);
                            tbStatus.Text = "";
                            break;
                    }
                }
            }

            tbStatus.Text = "DEBLOATING HAS BEEN COMPLETED";
            await Task.Delay(1000);
            tbStatus.Text = "";
        }*/
        private async void btnCU_Click(object sender, EventArgs e)
        {
            var removalHelpers = new RemovalTools.RemovalHelpers();
            var cleanup = new CleanupApp.Cleanup(removalHelpers);

            bool cleanTemp = clbCU.GetItemChecked(0);
            bool cleanInternetCache = clbCU.GetItemChecked(1);
            bool cleanEventLogs = clbCU.GetItemChecked(2);
            bool cleanDefaultPowerPlans = clbCU.GetItemChecked(3);

            await cleanup.RunRemoval(cleanTemp, cleanInternetCache, cleanEventLogs, cleanDefaultPowerPlans);

            tbStatus.Text = "CLEANUP IS DONE";
            await Task.Delay(1000);
            tbStatus.Text = "";
        }
    }
}