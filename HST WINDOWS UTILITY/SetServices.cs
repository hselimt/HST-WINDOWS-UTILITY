using Microsoft.Win32;
using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SetServicesApp
{
    public class SetServices
    {
        private static readonly string[] DefaultServicesToDisable = new[]
        {
            "tzautoupdate",   // Auto Time Zone Updater
            "BthAvctpSvc",    // AVCTP
            "BDESVC",         // BitLocker Drive Encryption
            "wbengine",       // Block Level Backup Engine
            "autotimesvc",    // Cellular Time
            "ClipSVC",        // Client License
            "cbdhsvc*",       // Clipboard User (pattern matching)
            "DiagTrack",      // Connected User Experiences and Telemetry
            "ConsentUxUserSvc*", // ConsentUX (pattern matching)
            "PimIndexMaintenanceSvc*", // Contact Data (pattern matching)
            "CDPUserSvc*",    // Connected Devices Platform User (pattern matching)
            "DsSvc",          // Data Sharing
            "DusmSvc*",       // Data Usage (pattern matching)
            "DoSvc",          // Delivery Optimization
            "DmEnrollmentSvc", // Device Management Enrollment
            "diagsvc",        // Diagnostic Execution
            "DialogBlockingService", // Dialog Blocking
            "DisplayEnhancementService", // Display Enhancement
            "fhsvc",          // File History
            "BcastDVRUserService*", // GameDVR and Broadcast (pattern matching)
            "lfsvc",          // Geolocation
            "iphlpsvc",       // IP Helper
            "LxpSvc*",        // Language Experience (pattern matching)
            "MicrosoftEdgeElevationService", // Microsoft Edge Elevation
            "edgeupdate",     // Microsoft Edge Update
            "edgeupdatem",    // Microsoft Edge Update
            "MsKeyboardFilter", // Microsoft Keyboard Filter
            "NgcSvc",         // Microsoft Passport
            "NgcCtnrSvc",     // Microsoft Passport Container
            "InstallService", // Microsoft Store Install
            "uhssvc",         // Microsoft Update Health
            "SmsRouter",      // Microsoft Windows SMS Router
            "NetTcpPortSharing", // Net.Tcp Port Sharing
            "Netlogon",       // Netlogon
            "NcbService",     // Network Connection Broker
            "CscService",     // Offline Files
            "defragsvc",      // Optimize drives
            "WpcMonSvc",      // Parental Controls
            "SEMgrSvc",       // Payments and NFC/SE Manager
            "PhoneSvc",       // Phone
            "Spooler",        // Print Spooler
            "PrintDeviceConfigurationService", //Print Device Configuration Service
            "PrintNotify",    // Printer Extensions and Notifications
            "PrintWorkflowUserSvc*", // Print Workflow (pattern matching)
            "wercplsupport",  // Problem Reports Control Panel Support
            "PcaSvc",         // Program Compatibility Assistant
            "QWAVE",          // Quality Windows Audio Visual Experience
            "RmSvc",          // Radio Management
            "TroubleshootingSvc", // Recommended Troubleshooting
            "RasAuto",        // Remote Access Auto Connection Manager
            "RasMan",         // Remote Access Connection Manager
            "SessionEnv",     // Remote Desktop Configuration
            "UmRdpService",   // Remote Desktop
            "RemoteRegistry", // Remote Registry
            "RemoteAccess",   // Routing and Remote Access
            "SensorDataService", // Sensor Data
            "SensrSvc",       // Sensor Monitoring
            "SensorService",  // Sensor
            "LanmanServer",   // Server
            "shpamsvc",       // Shared PC Account Manager
            "SCardSvr",       // Smart Card
            "ScDeviceEnum",   // Smart Card Device Enumeration
            "SCPolicySvc",    // Smart Card Removal Policy
            "OneSyncSvc*",    // Sync Host (pattern matching)
            "SysMain",        // SysMain
            "TabletInputService", // Touch Keyboard and Handwriting Panel
            "UserDataSvc*",   // User Data Access (pattern matching)
            "UnistoreSvc*",   // User Data Storage (pattern matching)
            "UevAgentService", // User Experience Virtualization
            "SDRSVC",         // Windows Backup
            "FrameServer",    // Windows Camera Frame Server
            "wcncsvc",        // Windows Connect Now
            "Wecsvc",         // Windows Event Collector
            "wisvc",          // Windows Insider
            "MixedRealityOpenXRSvc", // Windows Mixed Reality OpenXR
            "icssvc",         // Windows Mobile Hotspot
            "spectrum",       // Windows Perception
            "perceptionsimulation", // Windows Perception Simulation
            "WpnUserService*", // Windows Push Notifications User (pattern matching)
            "PushToInstall",  // Windows Push To Install
            "W32Time",        // Windows Time
            "WFDSConMgrSvc",  // Wi-Fi Direct Services Connection Manager
            "WSearch",  // Windows Search
            "LanmanWorkstation", // Workstation
            "AppVClient",     // Microsoft App-V Client
            "cloudidsvc",     // Microsoft Cloud Identity
            "diagnosticshub.standardcollector.service", // Microsoft (R) Diagnostics Hub Standard Collector
            "wlidsvc"         // Microsoft Account Sign-in Assistant
        };

        public async Task DisableMultipleServices(string[] additionalServices = null)
        {
            string[] servicesToDisable = DefaultServicesToDisable;

            if (additionalServices != null && additionalServices.Length > 0)
            {
                servicesToDisable = servicesToDisable.Concat(additionalServices).ToArray();
            }

            foreach (string service in servicesToDisable.Distinct())
            {
                // Check if this is a pattern (has asterisk)
                if (service.EndsWith("*"))
                {
                    string pattern = service.TrimEnd('*');
                    DisableServicesByRegKeyPattern(pattern);
                }
                else
                {
                    await DisableService(service);
                }
            }
        }

        private async Task DisableService(string serviceName)
        {
            try
            {
                // Try to disable the exact service name
                if (ServiceExists(serviceName))
                {
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped &&
                            sc.Status != ServiceControllerStatus.StopPending)
                        {
                            await StopServiceAsync(sc);
                        }
                    }
                }

                // Disable via registry regardless if service exists in SC
                string registryPath = $@"SYSTEM\CurrentControlSet\Services\{serviceName}";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("Start", 4, RegistryValueKind.DWord);
                    }
                }
            }
            catch
            {
                // Ignore errors and continue with next service
            }
        }

        private void DisableServicesByRegKeyPattern(string serviceNamePattern)
        {
            try
            {
                string servicesPath = @"SYSTEM\CurrentControlSet\Services";
                using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(servicesPath, true))
                {
                    if (servicesKey != null)
                    {
                        foreach (string serviceName in servicesKey.GetSubKeyNames())
                        {
                            if (serviceName.StartsWith(serviceNamePattern, StringComparison.OrdinalIgnoreCase))
                            {
                                using (RegistryKey serviceKey = servicesKey.OpenSubKey(serviceName, true))
                                {
                                    if (serviceKey != null)
                                    {
                                        serviceKey.SetValue("Start", 4, RegistryValueKind.DWord);

                                        // Try to stop the service if it exists
                                        if (ServiceExists(serviceName))
                                        {
                                            try
                                            {
                                                using (ServiceController sc = new ServiceController(serviceName))
                                                {
                                                    if (sc.Status != ServiceControllerStatus.Stopped)
                                                    {
                                                        sc.Stop();
                                                    }
                                                }
                                            }
                                            catch { /* Ignore stop errors */ }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore errors and continue
            }
        }

        public bool ServiceExists(string serviceName)
        {
            try
            {
                return ServiceController.GetServices()
                    .Any(s => s.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        public async Task StopServiceAsync(ServiceController sc)
        {
            await Task.Run(() =>
            {
                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(5));
                }
                catch
                {
                    // Ignore errors when stopping service
                }
            });
        }
    }
}