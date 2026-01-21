using HST.Controllers.Helpers;
using Microsoft.Win32;

namespace HST.Controllers.RegistryManager
{
    public class RegistryOptimizer
    {
        // Applies registry optimizations for gaming and performance
        public async Task OptimizeRegistryAsync()
        {
            Logger.Log("Starting to modify registry");
            try
            {
                await Task.Run(() =>
                {
                    OptimizePriorityAndPower();
                    DisableGameFeatures();
                    OptimizeLatency();
                    OptimizeInputDevices();
                    DisableTelemetryAndPrivacy();
                    DisableContentDelivery();
                    DisableNotifications();
                    OptimizeBrowsers();
                    OptimizeTaskbarAndVisuals();
                    DisableSoundAlerts();
                });
                Logger.Success("Modifying registry completed");
            }
            catch (Exception ex)
            {
                Logger.Error("OptimizeRegistryAsync", ex);
            }
        }

        private void OptimizePriorityAndPower()
        {
            // Setting Win32Priority
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\PriorityControl"))
            {
                key.SetValue("Win32PrioritySeparation", 0x26, RegistryValueKind.DWord);
            }

            // Disabling Fast Startup
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power"))
            {
                key.SetValue("SleepStudyDisabled", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Hibernation
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power"))
            {
                key.SetValue("HibernateEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Automatic Maintenance
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"))
            {
                key.SetValue("MaintenanceDisabled", 0x1, RegistryValueKind.DWord);
            }

            // Disabling PowerThrottling
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"))
            {
                key.SetValue("PowerThrottlingOff", 0x1, RegistryValueKind.DWord);
            }

            // Disabling HAGS
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers"))
            {
                key.SetValue("HwSchMode", 0x1, RegistryValueKind.DWord);
            }
        }

        private void DisableGameFeatures()
        {
            // Disabling Game Mode
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\GameBar"))
            {
                key.SetValue("AllowAutoGameMode", 0x0, RegistryValueKind.DWord);
                key.SetValue("AutoGameModeEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("AllowGameBarControllerButton", 0x0, RegistryValueKind.DWord);
                key.SetValue("UseNexusForGameBarEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Game DVR
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"System\GameConfigStore"))
            {
                key.SetValue("GameDVR_Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_FSEBehaviorMode", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_FSEBehavior", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_HonorUserFSEBehaviorMode", 0x1, RegistryValueKind.DWord);
                key.SetValue("GameDVR_DXGIHonorFSEWindowsCompatible", 0x1, RegistryValueKind.DWord);
                key.SetValue("GameDVR_EFSEFeatureFlags", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_DSEBehavior", 0x0, RegistryValueKind.DWord);
                key.SetValue("GameDVR_DXGI_AGILITY_FACTOR", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Game DVR Policy
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR"))
            {
                key.SetValue("AllowGameDVR", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Game DVR App Capture
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"))
            {
                key.SetValue("AppCaptureEnabled", 0x0, RegistryValueKind.DWord);
            }
        }

        private void OptimizeLatency()
        {
            // Reducing Menu Delay
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
            {
                key.SetValue("MenuShowDelay", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Accessibility Shortcuts
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\HighContrast"))
            {
                key.SetValue("Flags", 0x0, RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\ToggleKeys"))
            {
                key.SetValue("Flags", 0x0, RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\StickyKeys"))
            {
                key.SetValue("Flags", 0x0, RegistryValueKind.String);
            }

            // Network and System Profile Settings
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"))
            {
                key.SetValue("NoLazyMode", 0x1, RegistryValueKind.DWord);
                key.SetValue("AlwaysOn", 0x1, RegistryValueKind.DWord);
                key.SetValue("NetworkThrottlingIndex", unchecked((int)0xffffffff), RegistryValueKind.DWord);
                key.SetValue("SystemResponsiveness", 0xa, RegistryValueKind.DWord);
            }

            // Gaming Task Priority Settings
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
            
            // Disabling Ulps (Ultra Low Power State for AMD GPUs)
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000"))
            {
                key.SetValue("EnableUlps", 0x0, RegistryValueKind.DWord);
                key.SetValue("EnableUlps_NA", 0x0, RegistryValueKind.String);
            }

            // Enabling Distribute Timers
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\kernel"))
            {
                key.SetValue("DistributeTimers", 0x1, RegistryValueKind.DWord);
            }

            // Lower Priority for Background Services - Windows Update
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\wuauclt.exe\PerfOptions"))
            {
                key.SetValue("CpuPriorityClass", 0x1, RegistryValueKind.DWord);
                key.SetValue("IoPriority", 0x0, RegistryValueKind.DWord);
            }

            // Lower Priority for Background Services - Search Indexer
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\SearchIndexer.exe\PerfOptions"))
            {
                key.SetValue("CpuPriorityClass", 0x1, RegistryValueKind.DWord);
                key.SetValue("IoPriority", 0x0, RegistryValueKind.DWord);
            }
        }

        private void OptimizeInputDevices()
        {
            // Keyboard Repeat Rate
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Keyboard"))
            {
                key.SetValue("KeyboardDelay", "0", RegistryValueKind.String);
                key.SetValue("KeyboardSpeed", "31", RegistryValueKind.String);
            }

            // Mouse and Keyboard Data Queue Size
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\mouclass\Parameters"))
            {
                key.SetValue("MouseDataQueueSize", 0x10, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\kbdclass\Parameters"))
            {
                key.SetValue("KeyboardDataQueueSize", 0x10, RegistryValueKind.DWord);
            }

            // Disabling Mouse Acceleration
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Mouse"))
            {
                key.SetValue("MouseSpeed", "0", RegistryValueKind.String);
                key.SetValue("MouseThreshold1", "0", RegistryValueKind.String);
                key.SetValue("MouseThreshold2", "0", RegistryValueKind.String);
            }
        }

        private void DisableTelemetryAndPrivacy()
        {
            // Disabling Telemetry
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection"))
            {
                key.SetValue("AllowTelemetry", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Telemetry and Data Collection
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection"))
            {
                key.SetValue("AllowTelemetry", 0x0, RegistryValueKind.DWord);
                key.SetValue("AllowDeviceNameInTelemetry", 0x0, RegistryValueKind.DWord);
                key.SetValue("AllowCommercialDataPipeline", 0x0, RegistryValueKind.DWord);
                key.SetValue("LimitEnhancedDiagnosticDataWindowsAnalytics", 0x0, RegistryValueKind.DWord);
                key.SetValue("DoNotShowFeedbackNotifications", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Tailored Experiences
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy"))
            {
                key.SetValue("TailoredExperiencesWithDiagnosticDataEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Activity History
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System"))
            {
                key.SetValue("PublishUserActivities", 0x0, RegistryValueKind.DWord);
                key.SetValue("UploadUserActivities", 0x0, RegistryValueKind.DWord);
                key.SetValue("EnableActivityFeed", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Advertising ID
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo"))
            {
                key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling HTTP Accept Language Header
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\International\User Profile"))
            {
                key.SetValue("HttpAcceptLanguageOptOut", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Start Menu App Tracking
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Privacy"))
            {
                key.SetValue("Start_TrackProgs", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Personalization Privacy Policy
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Personalization\Settings"))
            {
                key.SetValue("AcceptedPrivacyPolicy", 0x0, RegistryValueKind.DWord);
                key.SetValue("RestrictImplicitInkCollection", 0x1, RegistryValueKind.DWord);
                key.SetValue("RestrictImplicitTextCollection", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Event Transcript
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack\EventTranscriptKey"))
            {
                key.SetValue("EnableEventTranscript", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Customer Experience Improvement Program
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Siuf\Rules"))
            {
                key.SetValue("NumberOfSIUFInPeriod", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Dynamic Scrollbars
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility"))
            {
                key.SetValue("DynamicScrollbars", 0, RegistryValueKind.DWord);
            }

            // Disabling Account Notifications
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SystemSettings\AccountNotifications"))
            {
                key.SetValue("EnableAccountNotifications", 0x0, RegistryValueKind.DWord);
            }

            // Disabling App Suggestions
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Privacy"))
            {
                key.SetValue("AppSuggestions", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Smart Clipboard
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"))
            {
                key.SetValue("Disabled", 0x1, RegistryValueKind.DWord);
            }

            // Capability Access Manager Consent Store
            string[] consentStoreKeys = new string[]
            {
                "appointments", "appDiagnostics", "broadFileSystemAccess", "bluetoothSync", "chat", "contacts",
                "documentsLibrary", "downloadsFolder", "email", "graphicsCaptureProgrammatic", "graphicsCaptureWithoutBorder",
                "location", "microphone", "musicLibrary", "phoneCall", "phoneCallHistory", "picturesLibrary", "radios", "webcam",
                "userAccountInformation", "userDataTasks", "userNotificationListener", "videosLibrary"
            };

            foreach (string subKey in consentStoreKeys)
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\{subKey}"))
                {
                    key.SetValue("Value", subKey == "microphone" ? "Allow" : "Deny", RegistryValueKind.String);
                }
            }

            // Disabling Online Speech Privacy
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"))
            {
                key.SetValue("HasAccepted", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Voice Activation
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"))
            {
                key.SetValue("AgentActivationEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("AgentActivationLastUsed", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Sync
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

            // Disabling Error Reporting
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"))
            {
                key.SetValue("Disabled", 0x1, RegistryValueKind.DWord);
                key.SetValue("DoReport", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Storage Sense
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\StorageSense"))
            {
                key.SetValue("AllowStorageSenseGlobal", 0x0, RegistryValueKind.DWord);
            }

            // Configuring Search
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search"))
            {
                key.SetValue("HistoryViewEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("DeviceHistoryEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("BingSearchEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SearchboxTaskbarMode", 0x0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SearchSettings"))
            {
                key.SetValue("IsDynamicSearchBoxEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("IsDeviceSearchHistoryEnabled", 0x0, RegistryValueKind.DWord);
            }
        }

        private void DisableContentDelivery()
        {
            // Disabling Content Delivery Manager Features
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"))
            {
                key.SetValue("SubscribedContent-338393Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-338388Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-314559Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-280815Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-202914Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-353694Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-353696Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-338387Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-353698Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-338389Enabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("SubscribedContent-310093Enabled", 0x0, RegistryValueKind.DWord);
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

            // Disabling News and Interests (Widgets)
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"))
            {
                key.SetValue("EnableFeeds", 0x0, RegistryValueKind.DWord);
            }

            // Hiding Most Used Apps in Start Menu
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                key.SetValue("ShowOrHideMostUsedApps", 0x2, RegistryValueKind.DWord);
            }

            // Disabling Windows Spotlight (User)
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

            // Disabling Windows Consumer Features
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CloudContent"))
            {
                key.SetValue("DisableThirdPartySuggestions", 0x2, RegistryValueKind.DWord);
                key.SetValue("DisableWindowsConsumerFeatures", 0x0, RegistryValueKind.DWord);
            }

            // Disabling News and Interests
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Dsh"))
            {
                key.SetValue("AllowNewsAndInterests", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Startup Sound
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"))
            {
                key.SetValue("DisableStartupSound", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Remote Assistance
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Remote Assistance"))
            {
                key.SetValue("fAllowToGetHelp", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Frequent/Recent Files
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"))
            {
                key.SetValue("ShowFrequent", 0x0, RegistryValueKind.DWord);
                key.SetValue("ShowRecent", 0x0, RegistryValueKind.DWord);
                key.SetValue("TelemetrySalt", 0x0, RegistryValueKind.DWord);
                key.SetValue("NoRecentDocsHistory", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Recent Documents History
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
            {
                key.SetValue("NoRecentDocsHistory", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Map Auto-Update
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\Maps"))
            {
                key.SetValue("AutoUpdateEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Driver Updates from Windows Update
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"))
            {
                key.SetValue("SearchOrderConfig", 0x0, RegistryValueKind.DWord);
                key.SetValue("DontSearchWindowsUpdate", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Store Auto Downloads
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\WindowsStore"))
            {
                key.SetValue("AutoDownload", 0x2, RegistryValueKind.DWord);
            }
        }

        private void DisableNotifications()
        {
            // Disabling Toast Notifications
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\PushNotifications"))
            {
                key.SetValue("ToastEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Security and Maintenance Notifications
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance"))
            {
                key.SetValue("Enabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Notification Center
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                key.SetValue("DisableNotificationCenter", 0x1, RegistryValueKind.DWord);
            }

            // Disabling Background Access for Apps
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"))
            {
                key.SetValue("GlobalUserDisabled", 0x1, RegistryValueKind.DWord);
            }

            // Enabling Auto-Endtask (Windows 11 Feature)
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings"))
            {
                key.SetValue("TaskbarEndTask", 0x1, RegistryValueKind.DWord);
            }
        }

        private void OptimizeBrowsers()
        {
            // Disabling Edge Startup Boost and Background Mode
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Edge"))
            {
                key.SetValue("StartupBoostEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("HardwareAccelerationModeEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("BackgroundModeEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Edge Elevation Service
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\MicrosoftEdgeElevationService"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            // Disabling Edge Update Services
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\edgeupdate"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\edgeupdatem"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            // Deleting Edge Update Scheduled Tasks
            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\MicrosoftEdgeUpdateTaskMachineCore", false);
            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\MicrosoftEdgeUpdateTaskMachineUA", false);

            // Disabling Chrome Startup Boost and Background Mode
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Google\Chrome"))
            {
                key.SetValue("StartupBoostEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("HardwareAccelerationModeEnabled", 0x0, RegistryValueKind.DWord);
                key.SetValue("BackgroundModeEnabled", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Chrome Elevation Service
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\GoogleChromeElevationService"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            // Disabling Chrome Update Services
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\gupdate"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\gupdatem"))
            {
                key.SetValue("Start", 0x4, RegistryValueKind.DWord);
            }
        }

        private void OptimizeTaskbarAndVisuals()
        {
            // Setting Taskbar Alignment to Left, Hide Widgets and Task View
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"))
            {
                key.SetValue("TaskbarMn", 0x0, RegistryValueKind.DWord);
                key.SetValue("ShowTaskViewButton", 0x0, RegistryValueKind.DWord);
            }

            // Hiding Meet Now
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
            {
                key.SetValue("HideSCAMeetNow", 0x1, RegistryValueKind.DWord);
            }

            // Hiding Lock and Sleep Options from Power Menu
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"))
            {
                key.SetValue("ShowLockOption", 0x0, RegistryValueKind.DWord);
                key.SetValue("ShowSleepOption", 0x0, RegistryValueKind.DWord);
            }

            // Disabling Transparency Effects
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                key.SetValue("EnableTransparency", 0x0, RegistryValueKind.DWord);
            }
        }

        private void DisableSoundAlerts()
        {
            // Setting Sound Scheme to None
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"AppEvents\Schemes"))
            {
                key.SetValue("", ".None", RegistryValueKind.String);
            }

            // Remove Sound Events
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
        }

        // Modifies registry to lower visual settings
        public async Task LowerVisualsAsync()
        {
            Logger.Log("Starting to lower visuals");
            try
            {
                await Task.Run(() =>
                {
                    // Setting Visual Effects to Custom
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"))
                    {
                        key.SetValue("VisualFXSetting", 0x3, RegistryValueKind.DWord);
                    }

                    // Disabling Animations and Visual Effects
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

                    // Disabling Explorer Visual Effects
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"))
                    {
                        key.SetValue("TaskbarAnimations", 0x0, RegistryValueKind.DWord);
                        key.SetValue("IconsOnly", 0x1, RegistryValueKind.DWord);
                        key.SetValue("ListviewAlphaSelect", 0x0, RegistryValueKind.DWord);
                        key.SetValue("ListviewShadow", 0x0, RegistryValueKind.DWord);
                    }

                    // Disabling Desktop Window Manager Effects
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\DWM"))
                    {
                        key.SetValue("EnableAeroPeek", 0x0, RegistryValueKind.DWord);
                        key.SetValue("AlwaysHibernateThumbnails", 0x0, RegistryValueKind.DWord);
                    }

                    // Disabling Window Minimize/Maximize Animations
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop\WindowMetrics"))
                    {
                        key.SetValue("MinAnimate", "0", RegistryValueKind.String);
                    }
                });
                Logger.Success("Lowering visuals completed");
            }
            catch (Exception ex)
            {
                Logger.Error("LowerVisualsAsync", ex);
            }
        }

        // Modifies registry to set dark mode
        public async Task SwitchDarkModeAsync()
        {
            Logger.Log("Setting dark mode");
            try
            {
                await Task.Run(() =>
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                    {
                        key.SetValue("AppsUseLightTheme", 0, RegistryValueKind.DWord);
                        key.SetValue("SystemUsesLightTheme", 0, RegistryValueKind.DWord);
                    }
                });
                Logger.Success("Dark mode set");
            }
            catch (Exception ex)
            {
                Logger.Error("SwitchDarkModeAsync", ex);
            }
        }

        // Reverts registry modifications to Windows defaults
        public async Task OptimizeRegistryRevertAsync()
        {
            Logger.Log("Starting to revert registry modifications");
            try
            {
                await Task.Run(() =>
                {
                    RevertPriorityAndPower();
                    RevertGameFeatures();
                    RevertLatency();
                    RevertInputDevices();
                    RevertTelemetryAndPrivacy();
                    RevertContentDelivery();
                    RevertNotifications();
                    RevertBrowsers();
                    RevertTaskbarAndVisuals();
                    RevertSoundAlerts();
                });
                Logger.Success("Registry revert completed");
            }
            catch (Exception ex)
            {
                Logger.Error("OptimizeRegistryRevertAsync", ex);
            }
        }

        private void RevertPriorityAndPower()
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\PriorityControl"))
            {
                if (key != null) key.SetValue("Win32PrioritySeparation", 2, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power"))
            {
                if (key != null)
                {
                    key.SetValue("SleepStudyDisabled", 0, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power"))
            {
                if (key != null)
                {
                    key.SetValue("HibernateEnabled", 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"))
            {
                if (key != null) key.SetValue("MaintenanceDisabled", 0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"))
            {
                if (key != null) key.DeleteValue("PowerThrottlingOff", false);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers"))
            {
                if (key != null) key.SetValue("HwSchMode", 2, RegistryValueKind.DWord);
            }
        }

        private void RevertGameFeatures()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\GameBar"))
            {
                if (key != null)
                {
                    key.SetValue("AllowAutoGameMode", 1, RegistryValueKind.DWord);
                    key.SetValue("AutoGameModeEnabled", 1, RegistryValueKind.DWord);
                    key.SetValue("AllowGameBarControllerButton", 1, RegistryValueKind.DWord);
                    key.SetValue("UseNexusForGameBarEnabled", 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"System\GameConfigStore"))
            {
                if (key != null)
                {
                    key.SetValue("GameDVR_Enabled", 1, RegistryValueKind.DWord);
                    key.SetValue("GameDVR_FSEBehaviorMode", 2, RegistryValueKind.DWord);
                    key.SetValue("GameDVR_FSEBehavior", 2, RegistryValueKind.DWord);
                    key.SetValue("GameDVR_HonorUserFSEBehaviorMode", 0, RegistryValueKind.DWord);
                    key.SetValue("GameDVR_DXGIHonorFSEWindowsCompatible", 0, RegistryValueKind.DWord);
                    key.SetValue("GameDVR_EFSEFeatureFlags", 1, RegistryValueKind.DWord);
                    key.SetValue("GameDVR_DSEBehavior", 2, RegistryValueKind.DWord);
                    key.DeleteValue("GameDVR_DXGI_AGILITY_FACTOR", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR"))
            {
                if (key != null) key.DeleteValue("AllowGameDVR", false);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"))
            {
                if (key != null) key.SetValue("AppCaptureEnabled", 1, RegistryValueKind.DWord);
            }
        }

        private void RevertLatency()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
            {
                if (key != null) key.SetValue("MenuShowDelay", "400", RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\HighContrast"))
            {
                if (key != null) key.SetValue("Flags", "126", RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\ToggleKeys"))
            {
                if (key != null) key.SetValue("Flags", "62", RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\StickyKeys"))
            {
                if (key != null) key.SetValue("Flags", "510", RegistryValueKind.String);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"))
            {
                if (key != null)
                {
                    key.DeleteValue("NoLazyMode", false);
                    key.DeleteValue("AlwaysOn", false);
                    key.SetValue("NetworkThrottlingIndex", 10, RegistryValueKind.DWord);
                    key.SetValue("SystemResponsiveness", 20, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"))
            {
                if (key != null)
                {
                    key.DeleteValue("Affinity", false);
                    key.DeleteValue("Background Only", false);
                    key.DeleteValue("GPU Priority", false);
                    key.DeleteValue("Priority", false);
                    key.DeleteValue("Scheduling Category", false);
                    key.DeleteValue("SFIO Priority", false);
                    key.DeleteValue("Latency Sensitive", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000"))
            {
                if (key != null)
                {
                    key.DeleteValue("EnableUlps", false);
                    key.DeleteValue("EnableUlps_NA", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\FileSystem"))
            {
                if (key != null)
                {
                    key.SetValue("NtfsDisableLastAccessUpdate", 1, RegistryValueKind.DWord);
                    key.SetValue("NtfsDisable8dot3NameCreation", 2, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\kernel"))
            {
                if (key != null) key.DeleteValue("DistributeTimers", false);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\wuauclt.exe\PerfOptions"))
            {
                if (key != null)
                {
                    key.DeleteValue("CpuPriorityClass", false);
                    key.DeleteValue("IoPriority", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\SearchIndexer.exe\PerfOptions"))
            {
                if (key != null)
                {
                    key.DeleteValue("CpuPriorityClass", false);
                    key.DeleteValue("IoPriority", false);
                }
            }
        }

        private void RevertInputDevices()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Keyboard"))
            {
                if (key != null)
                {
                    key.SetValue("KeyboardDelay", "1", RegistryValueKind.String);
                    key.SetValue("KeyboardSpeed", "31", RegistryValueKind.String);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\mouclass\Parameters"))
            {
                if (key != null) key.DeleteValue("MouseDataQueueSize", false);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\kbdclass\Parameters"))
            {
                if (key != null) key.DeleteValue("KeyboardDataQueueSize", false);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Mouse"))
            {
                if (key != null)
                {
                    key.SetValue("MouseSpeed", "1", RegistryValueKind.String);
                    key.SetValue("MouseThreshold1", "6", RegistryValueKind.String);
                    key.SetValue("MouseThreshold2", "10", RegistryValueKind.String);
                }
            }
        }

        private void RevertTelemetryAndPrivacy()
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection"))
            {
                if (key != null) key.SetValue("AllowTelemetry", 3, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection"))
            {
                if (key != null)
                {
                    key.SetValue("AllowTelemetry", 3, RegistryValueKind.DWord);
                    key.DeleteValue("AllowDeviceNameInTelemetry", false);
                    key.DeleteValue("AllowCommercialDataPipeline", false);
                    key.DeleteValue("LimitEnhancedDiagnosticDataWindowsAnalytics", false);
                    key.DeleteValue("DoNotShowFeedbackNotifications", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy"))
            {
                if (key != null)
                {
                    key.SetValue("TailoredExperiencesWithDiagnosticDataEnabled", 1, RegistryValueKind.DWord);
                    key.SetValue("Start_TrackProgs", 1, RegistryValueKind.DWord);
                    key.DeleteValue("AppSuggestions", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System"))
            {
                if (key != null)
                {
                    key.DeleteValue("PublishUserActivities", false);
                    key.DeleteValue("UploadUserActivities", false);
                    key.DeleteValue("EnableActivityFeed", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo"))
            {
                if (key != null) key.SetValue("Enabled", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\International\User Profile"))
            {
                if (key != null) key.SetValue("HttpAcceptLanguageOptOut", 0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Personalization\Settings"))
            {
                if (key != null)
                {
                    key.SetValue("AcceptedPrivacyPolicy", 1, RegistryValueKind.DWord);
                    key.SetValue("RestrictImplicitInkCollection", 0, RegistryValueKind.DWord);
                    key.SetValue("RestrictImplicitTextCollection", 0, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack\EventTranscriptKey"))
            {
                if (key != null) key.SetValue("EnableEventTranscript", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Siuf\Rules"))
            {
                if (key != null) key.DeleteValue("NumberOfSIUFInPeriod", false);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility"))
            {
                if (key != null) key.SetValue("DynamicScrollbars", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SystemSettings\AccountNotifications"))
            {
                if (key != null) key.SetValue("EnableAccountNotifications", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"))
            {
                if (key != null) key.SetValue("Disabled", 0, RegistryValueKind.DWord);
            }

            string[] capabilities = {
                "appointments", "appDiagnostics", "broadFileSystemAccess", "bluetoothSync", "chat",
                "contacts", "documentsLibrary", "downloadsFolder", "email", "graphicsCaptureProgrammatic",
                "graphicsCaptureWithoutBorder", "location", "microphone", "musicLibrary", "phoneCall",
                "phoneCallHistory", "picturesLibrary", "radios", "webcam", "userAccountInformation",
                "userDataTasks", "userNotificationListener", "videosLibrary"
            };

            foreach (var cap in capabilities)
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\{cap}"))
                {
                    if (key != null) key.DeleteValue("Value", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"))
            {
                if (key != null) key.SetValue("HasAccepted", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"))
            {
                if (key != null)
                {
                    key.SetValue("AgentActivationEnabled", 1, RegistryValueKind.DWord);
                    key.DeleteValue("AgentActivationLastUsed", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync"))
            {
                if (key != null) key.SetValue("SyncPolicy", 0, RegistryValueKind.DWord);
            }

            string[] syncGroups = { "Accessibility", "AppSync", "Personalization", "BrowserSettings", "Credentials", "Language", "Windows" };

            foreach (var sg in syncGroups)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\{sg}"))
                {
                    if (key != null) key.SetValue("Enabled", 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"))
            {
                if (key != null)
                {
                    key.DeleteValue("Disabled", false);
                    key.DeleteValue("DoReport", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\StorageSense"))
            {
                if (key != null) key.DeleteValue("AllowStorageSenseGlobal", false);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search"))
            {
                if (key != null)
                {
                    key.SetValue("HistoryViewEnabled", 1, RegistryValueKind.DWord);
                    key.SetValue("DeviceHistoryEnabled", 1, RegistryValueKind.DWord);
                    key.SetValue("BingSearchEnabled", 1, RegistryValueKind.DWord);
                    key.SetValue("SearchboxTaskbarMode", 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SearchSettings"))
            {
                if (key != null)
                {
                    key.SetValue("IsDynamicSearchBoxEnabled", 1, RegistryValueKind.DWord);
                    key.SetValue("IsDeviceSearchHistoryEnabled", 1, RegistryValueKind.DWord);
                }
            }
        }

        private void RevertContentDelivery()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"))
            {
                if (key != null)
                {
                    string[] subs = {
                        "SubscribedContent-338393Enabled", "SubscribedContent-338388Enabled", "SubscribedContent-314559Enabled",
                        "SubscribedContent-280815Enabled", "SubscribedContent-202914Enabled", "SubscribedContent-353694Enabled",
                        "SubscribedContent-353696Enabled", "SubscribedContent-338387Enabled", "SubscribedContent-353698Enabled",
                        "SubscribedContent-338389Enabled", "SubscribedContent-310093Enabled", "SubscribedContent-314563Enabled",
                        "RotatingLockScreenOverlayEnabled", "RotatingLockScreenEnabled", "ContentDeliveryAllowed",
                        "OemPreInstalledAppsEnabled", "PreInstalledAppsEnabled", "PreInstalledAppsEverEnabled",
                        "SilentInstalledAppsEnabled", "SoftLandingEnabled", "SubscribedContentEnabled",
                        "FeatureManagementEnabled", "SystemPaneSuggestionsEnabled", "RemediationRequired"
                    };
                    foreach (var s in subs) key.SetValue(s, 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"))
            {
                if (key != null) key.DeleteValue("EnableFeeds", false);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                if (key != null) key.DeleteValue("ShowOrHideMostUsedApps", false);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CloudContent"))
            {
                if (key != null)
                {
                    key.DeleteValue("ConfigureWindowsSpotlight", false);
                    key.DeleteValue("IncludeEnterpriseSpotlight", false);
                    key.DeleteValue("DisableWindowsSpotlightFeatures", false);
                    key.DeleteValue("DisableWindowsSpotlightWindowsWelcomeExperience", false);
                    key.DeleteValue("DisableWindowsSpotlightOnActionCenter", false);
                    key.DeleteValue("DisableWindowsSpotlightOnSettings", false);
                    key.DeleteValue("DisableThirdPartySuggestions", false);
                    key.DeleteValue("DisableTailoredExperiencesWithDiagnosticData", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CloudContent"))
            {
                if (key != null)
                {
                    key.DeleteValue("DisableThirdPartySuggestions", false);
                    key.DeleteValue("DisableWindowsConsumerFeatures", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Dsh"))
            {
                if (key != null) key.DeleteValue("AllowNewsAndInterests", false);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"))
            {
                if (key != null) key.SetValue("DisableStartupSound", 0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Remote Assistance"))
            {
                if (key != null) key.SetValue("fAllowToGetHelp", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"))
            {
                if (key != null)
                {
                    key.SetValue("ShowFrequent", 1, RegistryValueKind.DWord);
                    key.SetValue("ShowRecent", 1, RegistryValueKind.DWord);
                    key.SetValue("NoRecentDocsHistory", 0, RegistryValueKind.DWord);
                    key.DeleteValue("TelemetrySalt", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
            {
                if (key != null)
                {
                    key.SetValue("NoRecentDocsHistory", 0, RegistryValueKind.DWord);
                    key.DeleteValue("HideSCAMeetNow", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\Maps"))
            {
                if (key != null) key.SetValue("AutoUpdateEnabled", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"))
            {
                if (key != null)
                {
                    key.SetValue("DontSearchWindowsUpdate", 0, RegistryValueKind.DWord);
                    key.DeleteValue("SearchOrderConfig", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\WindowsStore"))
            {
                if (key != null) key.DeleteValue("AutoDownload", false);
            }
        }

        private void RevertNotifications()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\PushNotifications"))
            {
                if (key != null) key.SetValue("ToastEnabled", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance"))
            {
                if (key != null) key.SetValue("Enabled", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                if (key != null) key.DeleteValue("DisableNotificationCenter", false);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"))
            {
                if (key != null) key.SetValue("GlobalUserDisabled", 0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings"))
            {
                if (key != null) key.DeleteValue("TaskbarEndTask", false);
            }
        }

        private void RevertBrowsers()
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Edge"))
            {
                if (key != null)
                {
                    key.DeleteValue("StartupBoostEnabled", false);
                    key.DeleteValue("HardwareAccelerationModeEnabled", false);
                    key.DeleteValue("BackgroundModeEnabled", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\MicrosoftEdgeElevationService"))
            {
                if (key != null) key.SetValue("Start", 3, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\edgeupdate"))
            {
                if (key != null) key.SetValue("Start", 2, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\edgeupdatem"))
            {
                if (key != null) key.SetValue("Start", 3, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Google\Chrome"))
            {
                if (key != null)
                {
                    key.DeleteValue("StartupBoostEnabled", false);
                    key.DeleteValue("HardwareAccelerationModeEnabled", false);
                    key.DeleteValue("BackgroundModeEnabled", false);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\GoogleChromeElevationService"))
            {
                if (key != null) key.SetValue("Start", 3, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\gupdate"))
            {
                if (key != null) key.SetValue("Start", 2, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\gupdatem"))
            {
                if (key != null) key.SetValue("Start", 3, RegistryValueKind.DWord);
            }
        }

        private void RevertTaskbarAndVisuals()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"))
            {
                if (key != null)
                {
                    key.SetValue("TaskbarMn", 1, RegistryValueKind.DWord);
                    key.SetValue("ShowTaskViewButton", 1, RegistryValueKind.DWord);
                    key.DeleteValue("TaskbarAnimations", false);
                    key.DeleteValue("IconsOnly", false);
                    key.DeleteValue("ListviewAlphaSelect", false);
                    key.DeleteValue("ListviewShadow", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
            {
                if (key != null) key.DeleteValue("HideSCAMeetNow", false);
            }

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"))
            {
                if (key != null)
                {
                    key.DeleteValue("ShowLockOption", false);
                    key.DeleteValue("ShowSleepOption", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                if (key != null) key.SetValue("EnableTransparency", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"))
            {
                if (key != null) key.SetValue("VisualFXSetting", 0, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
            {
                if (key != null)
                {
                    key.SetValue("UserPreferencesMask", new byte[] { 0x9E, 0x3E, 0x07, 0x80, 0x12, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
                    key.SetValue("MinAnimate", "1", RegistryValueKind.String);
                    key.SetValue("DragFullWindows", "1", RegistryValueKind.String);
                    key.SetValue("FontSmoothing", "2", RegistryValueKind.String);
                    key.DeleteValue("WindowAnimation", false);
                    key.DeleteValue("MenuAnimation", false);
                    key.DeleteValue("TaskbarAnimations", false);
                    key.DeleteValue("IconAnimation", false);
                    key.DeleteValue("ScrollAnimation", false);
                    key.DeleteValue("ScrollSmoothness", false);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\DWM"))
            {
                if (key != null)
                {
                    key.SetValue("EnableAeroPeek", 1, RegistryValueKind.DWord);
                    key.SetValue("AlwaysHibernateThumbnails", 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop\WindowMetrics"))
            {
                if (key != null) key.SetValue("MinAnimate", "1", RegistryValueKind.String);
            }
        }

        private void RevertSoundAlerts()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"AppEvents\Schemes"))
            {
                if (key != null) key.SetValue("", ".Default", RegistryValueKind.String);
            }
        }
    }
}