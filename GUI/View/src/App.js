import React, { useState, useEffect } from "react";
import {
    Monitor,
    Settings,
    Trash2,
    Cpu,
    HardDrive,
    MemoryStick,
    Shield,
    Calendar,
    DownloadCloud,
    Eye,
    Moon,
    Battery,
    Check,
    Sparkles,
    Activity,
    X,
    Github,
    Wifi,
    WifiOff,
    CheckCircle,
    Loader,
    Server,
    Watch,
    Zap,
    RotateCcw,
    HelpCircle,
    User,
    ChevronDown,
    ChevronRight,
    AlertTriangle,
    ArrowLeft,
    Info,
    XCircle,
} from "lucide-react";

// ============================================================================
// HELP PAGE COMPONENT - DETAILED VERSION
// ============================================================================
const HelpPage = ({ onBack }) => {
    const [expandedSections, setExpandedSections] = useState({
        orderOfOperations: true,
        buttons: false,
        services: false,
        debloat: false,
        cleanup: false,
        revert: false,
        warnings: false,
        faq: false,
    });

    const toggleSection = (section) => {
        setExpandedSections((prev) => ({ ...prev, [section]: !prev[section] }));
    };

    const Section = ({ id, title, icon: Icon, children, color = "#8b5cf6" }) => (
        <div
            style={{
                background: "#0a0a0a",
                border: "1px solid #1a1a1a",
                borderRadius: "12px",
                marginBottom: "12px",
                overflow: "hidden",
            }}
        >
            <button
                onClick={() => toggleSection(id)}
                style={{
                    width: "100%",
                    padding: "16px 20px",
                    background: "transparent",
                    border: "none",
                    display: "flex",
                    alignItems: "center",
                    gap: "12px",
                    cursor: "pointer",
                    transition: "background 0.2s ease",
                }}
                onMouseEnter={(e) => (e.currentTarget.style.background = "#111")}
                onMouseLeave={(e) => (e.currentTarget.style.background = "transparent")}
            >
                <Icon style={{ width: "20px", height: "20px", color }} />
                <span
                    style={{
                        color: "#f3f4f6",
                        fontSize: "14px",
                        fontWeight: "700",
                        textTransform: "uppercase",
                        letterSpacing: "0.5px",
                        flex: 1,
                        textAlign: "left",
                    }}
                >
                    {title}
                </span>
                {expandedSections[id] ? (
                    <ChevronDown
                        style={{ width: "18px", height: "18px", color: "#666" }}
                    />
                ) : (
                    <ChevronRight
                        style={{ width: "18px", height: "18px", color: "#666" }}
                    />
                )}
            </button>
            {expandedSections[id] && (
                <div style={{ padding: "0 20px 20px 20px" }}>{children}</div>
            )}
        </div>
    );

    const HelpItem = ({
        title,
        description,
        icon: Icon,
        color = "#8b5cf6",
        warning = null,
        details = null,
    }) => (
        <div
            style={{
                background: "#000",
                border: "1px solid #1a1a1a",
                borderRadius: "8px",
                padding: "14px",
                marginBottom: "10px",
            }}
        >
            <div
                style={{
                    display: "flex",
                    alignItems: "center",
                    gap: "10px",
                    marginBottom: "8px",
                }}
            >
                {Icon && <Icon style={{ width: "16px", height: "16px", color }} />}
                <span style={{ color: "#e5e7eb", fontSize: "13px", fontWeight: "700" }}>
                    {title}
                </span>
            </div>
            <p
                style={{
                    color: "#9ca3af",
                    fontSize: "12px",
                    lineHeight: "1.6",
                    margin: 0,
                }}
            >
                {description}
            </p>

            {details && (
                <div
                    style={{
                        marginTop: "12px",
                        padding: "12px",
                        background: "#0a0a0a",
                        borderRadius: "6px",
                        border: "1px solid #1a1a1a",
                    }}
                >
                    <p
                        style={{
                            color: "#8b5cf6",
                            fontSize: "11px",
                            fontWeight: "600",
                            marginBottom: "8px",
                            textTransform: "uppercase",
                        }}
                    >
                        {details.title}
                    </p>
                    <div style={{ display: "flex", flexWrap: "wrap", gap: "6px" }}>
                        {details.items.map((item, i) => (
                            <span
                                key={i}
                                style={{
                                    padding: "4px 8px",
                                    background: "#111",
                                    border: "1px solid #222",
                                    borderRadius: "4px",
                                    color: "#9ca3af",
                                    fontSize: "10px",
                                    fontFamily: "monospace",
                                }}
                            >
                                {item}
                            </span>
                        ))}
                    </div>
                </div>
            )}

            {warning && (
                <div
                    style={{
                        display: "flex",
                        alignItems: "flex-start",
                        gap: "8px",
                        marginTop: "10px",
                        padding: "10px",
                        background: "rgba(239, 68, 68, 0.1)",
                        border: "1px solid rgba(239, 68, 68, 0.3)",
                        borderRadius: "6px",
                    }}
                >
                    <AlertTriangle
                        style={{
                            width: "14px",
                            height: "14px",
                            color: "#ef4444",
                            flexShrink: 0,
                            marginTop: "1px",
                        }}
                    />
                    <span
                        style={{ color: "#fca5a5", fontSize: "11px", lineHeight: "1.5" }}
                    >
                        {warning}
                    </span>
                </div>
            )}
        </div>
    );

    const WarningBox = ({ children }) => (
        <div
            style={{
                display: "flex",
                alignItems: "flex-start",
                gap: "10px",
                padding: "14px",
                background: "rgba(239, 68, 68, 0.1)",
                border: "1px solid rgba(239, 68, 68, 0.3)",
                borderRadius: "8px",
                marginBottom: "12px",
            }}
        >
            <AlertTriangle
                style={{
                    width: "18px",
                    height: "18px",
                    color: "#ef4444",
                    flexShrink: 0,
                }}
            />
            <span style={{ color: "#fca5a5", fontSize: "12px", lineHeight: "1.6" }}>
                {children}
            </span>
        </div>
    );

    const InfoBox = ({ children }) => (
        <div
            style={{
                display: "flex",
                alignItems: "flex-start",
                gap: "10px",
                padding: "14px",
                background: "rgba(139, 92, 246, 0.1)",
                border: "1px solid rgba(139, 92, 246, 0.3)",
                borderRadius: "8px",
                marginBottom: "12px",
            }}
        >
            <Info
                style={{
                    width: "18px",
                    height: "18px",
                    color: "#8b5cf6",
                    flexShrink: 0,
                }}
            />
            <span style={{ color: "#c4b5fd", fontSize: "12px", lineHeight: "1.6" }}>
                {children}
            </span>
        </div>
    );

    // Service data for detailed lists
    const essentialServices = [
        "Auto Time Zone Updater (tzautoupdate)",
        "AVCTP service (BthAvctpSvc)",
        "BitLocker Drive Encryption (BDESVC)",
        "Block Level Backup Engine (wbengine)",
        "CaptureService (CaptureService)",
        "Cellular Time (autotimesvc)",
        "Client License Service (ClipSVC) (ClipSVC)",
        "Clipboard User Service (cbdhsvc)",
        "Connected Devices Platform Service (CDPSvc)",
        "Connected Devices Platform User Service (CDPUserSvc)",
        "Connected User Experiences and Telemetry (DiagTrack)",
        "ConsentUX User Service (ConsentUxUserSvc)",
        "Contact Data (PimIndexMaintenanceSvc)",
        "Data Sharing Service (DsSvc)",
        "Data Usage (DusmSvc)",
        "Delivery Optimization (DoSvc)",
        "Device Association Broker (DeviceAssociationBrokerSvc)",
        "Device Management Enrollment Service (DmEnrollmentSvc)",
        "Device Management WAP Push (dmwappushservice)",
        "Device Picker User Service (DevicePickerUserSvc)",
        "Devices Flow User Service (DevicesFlowUserSvc)",
        "Diagnostic Execution Service (diagsvc)",
        "Diagnostic Policy Service (DPS)",
        "Diagnostic Service Host (WdiServiceHost)",
        "Diagnostic System Host (WdiSystemHost)",
        "DialogBlockingService (DialogBlockingService)",
        "Display Enhancement Service (DisplayEnhancementService)",
        "Distributed Link Tracking Client (TrkWks)",
        "Downloaded Maps Manager (MapsBroker)",
        "File History Service (fhsvc)",
        "GameDVR and Broadcast User Service (BcastDVRUserService)",
        "Geolocation Service (lfsvc)",
        "Inventory and Compatibility Appraisal (InventorySvc)",
        "IP Helper (iphlpsvc)",
        "Language Experience Service (LxpSvc)",
        "Messaging Service (MessagingService)",
        "Microsoft Account Sign-in Assistant (wlidsvc)",
        "Microsoft App-V Client (AppVClient)",
        "Microsoft Cloud Identity Service (cloudidsvc)",
        "Microsoft Diagnostics Hub Standard Collector Service (diagnosticshub.standardcollector.service)",
        "Microsoft Edge Elevation Service (MicrosoftEdgeElevationService)",
        "Microsoft Edge Update Service (edgeupdate)",
        "Microsoft Edge Update Service (edgeupdatem) (edgeupdatem)",
        "Microsoft Keyboard Filter (MsKeyboardFilter)",
        "Microsoft Passport (NgcSvc)",
        "Microsoft Passport Container (NgcCtnrSvc)",
        "Microsoft Store Install Service (InstallService)",
        "Microsoft Update Health Service (uhssvc)",
        "Microsoft Windows SMS Router Service (SmsRouter)",
        "Net.Tcp Port Sharing Service (NetTcpPortSharing)",
        "Netlogon (Netlogon)",
        "Network Connection Broker (NcbService)",
        "Offline Files (CscService)",
        "Optimize drives (defragsvc)",
        "Parental Controls (WpcMonSvc)",
        "Payments and NFC/SE Manager (SEMgrSvc)",
        "Phone Service (PhoneSvc)",
        "Print Spooler (Spooler)",
        "PrintDeviceConfiguration Service (PrintDeviceConfigurationService)",
        "Printer Extensions and Notifications (PrintNotify)",
        "PrintWorkflow (PrintWorkflowUserSvc)",
        "Problem Reports Control Panel Support (wercplsupport)",
        "Program Compatibility Assistant (PcaSvc)",
        "Quality Windows Audio Video Experience (QWAVE)",
        "Radio Management Service (RmSvc)",
        "Recommended Troubleshooting Service (TroubleshootingSvc)",
        "Remote Access Auto Connection Manager (RasAuto)",
        "Remote Access Connection Manager (RasMan)",
        "Remote Desktop Configuration (SessionEnv)",
        "Remote Desktop Services UserMode Port Redirector (UmRdpService)",
        "Remote Registry (RemoteRegistry)",
        "Retail Demo Service (RetailDemo)",
        "Routing and Remote Access (RemoteAccess)",
        "Sensor Data Service (SensorDataService)",
        "Sensor Monitoring Service (SensrSvc)",
        "Sensor Service (SensorService)",
        "Server (LanmanServer)",
        "Shared PC Account Manager (shpamsvc)",
        "Smart Card (SCardSvr)",
        "Smart Card Device Enumeration Service (ScDeviceEnum)",
        "Smart Card Removal Policy (SCPolicySvc)",
        "Sync Host (OneSyncSvc)",
        "SysMain (Superfetch) (SysMain)",
        "Telephony (TapiSrv)",
        "Touch Keyboard and Handwriting (TabletInputService)",
        "User Data Access (UserDataSvc)",
        "User Data Storage (UnistoreSvc)",
        "User Experience Virtualization Service (UevAgentService)",
        "Wi-Fi Direct Services Connection Manager Service (WFDSConMgrSvc)",
        "Windows Backup (CloudBackupRestoreSvc)",
        "Windows Backup (SDRSVC)",
        "Windows Biometric Service (WbioSrvc)",
        "Windows Camera Frame Server (FrameServer)",
        "Windows Connect Now - Config Registrar (wcncsvc)",
        "Windows Error Reporting (WerSvc)",
        "Windows Event Collector (Wecsvc)",
        "Windows Health and Optimized Experiences (whesvc)",
        "Windows Insider Service (wisvc)",
        "Windows Mixed Reality OpenXR (MixedRealityOpenXRSvc)",
        "Windows Mobile Hotspot (icssvc)",
        "Windows Perception Service (spectrum)",
        "Windows Perception Simulation Service (perceptionsimulation)",
        "Windows Push Notifications (WpnService)",
        "Windows Push Notifications User Service (WpnUserService)",
        "Windows PushToInstall Service (PushToInstall)",
        "Windows Search (WSearch)",
        "Windows Time (W32Time)",
        "Work Folders (workfolderssvc)",
        "Workstation (LanmanWorkstation)",
    ];

    const bluetoothServices = [
        "Bluetooth Audio Gateway Service (BTAGService)",
        "Bluetooth Support Service (bthserv)",
        "Bluetooth User Support Service (BluetoothUserService)",
        "Device Association Broker (DeviceAssociationBrokerSvc)",
    ];

    const hypervServices = [
        "HV Host Service (HvHost)",
        "Hyper-V Data Exchange Service (vmickvpexchange)",
        "Hyper-V Guest Service Interface (vmicguestinterface)",
        "Hyper-V Guest Shutdown Service (vmicshutdown)",
        "Hyper-V Heartbeat Service (vmicheartbeat)",
        "Hyper-V Host Compute Service (vmcompute)",
        "Hyper-V PowerShell Direct Service (vmicvmsession)",
        "Hyper-V Remote Desktop Virtualization (vmicrdv)",
        "Hyper-V Time Synchronization (vmictimesync)",
        "Hyper-V Virtual Machine Management (vmms)",
        "Hyper-V Volume Shadow Copy Requestor (vmicvss)",
    ];

    const xboxServices = [
        "Xbox Accessory Management Service (XboxGipSvc)",
        "Xbox Live Auth Manager (XblAuthManager)",
        "Xbox Live Game Save (XblGameSave)",
        "Xbox Live Networking Service (XboxNetApiSvc)",
    ];

    const microsoftApps = [
        "3D Builder",
        "3D Viewer",
        "Alarms & Clock",
        "Calculator",
        "Calendar",
        "Camera",
        "Clipchamp",
        "Cortana",
        "Feedback Hub",
        "Get Help",
        "Groove Music",
        "Mail",
        "Maps",
        "Messaging",
        "Microsoft News",
        "Microsoft Solitaire Collection",
        "Microsoft To Do",
        "Mixed Reality Portal",
        "Money",
        "Movies & TV",
        "Office Hub",
        "OneNote",
        "Paint 3D",
        "People",
        "Phone Companion",
        "Photos",
        "Power Automate Desktop",
        "Print 3D",
        "Quick Assist",
        "Skype",
        "Sports",
        "Sticky Notes",
        "Tips",
        "Voice Recorder",
        "Weather",
        "Whiteboard",
        "Windows Alarms",
        "Your Phone",
        "Zune Music",
        "Zune Video",
    ];

    const registryTweaks = [
        "Disable Game DVR & Game Bar background recording",
        "Disable Windows Telemetry and data collection",
        "Disable Cortana and web search in Start Menu",
        "Disable Action Center animations",
        "Remove mouse acceleration (enhance pointer precision)",
        "Disable network throttling for gaming",
        "Optimize memory management (LargeSystemCache)",
        "Disable Nagle's Algorithm for lower latency",
        "Disable Windows Tips and suggestions",
        "Disable advertising ID and targeted ads",
        "Disable activity history and timeline",
        "Disable background apps",
        "Disable app launch tracking",
        "Disable Start Menu suggestions",
        "Disable Bing search in Start Menu",
        "Disable lock screen tips and ads",
        "Disable automatic Windows Store app updates",
        "Disable Windows Defender SmartScreen prompts",
        "Disable Aero Shake minimize gesture",
        "Disable sticky keys and filter keys prompts",
        "Set power throttling to disabled",
        "Optimize NTFS for performance (disable last access time)",
        "Disable prefetch and superfetch",
        "Set CPU priority for foreground applications",
        "Disable hibernation to free disk space",
        "Disable USB selective suspend",
        "Disable remote assistance",
        "Disable AutoPlay for all drives",
    ];

    const scheduledTasks = [
        "Microsoft Compatibility Appraiser",
        "ProgramDataUpdater",
        "Consolidator (CEIP)",
        "UsbCeip",
        "Microsoft-Windows-DiskDiagnosticDataCollector",
        "QueueReporting",
        "Proxy (autochk)",
        "MapsToastTask",
        "MapsUpdateTask",
        "FamilySafetyMonitor",
        "FamilySafetyRefreshTask",
        "XblGameSaveTask",
        "Scheduled Start (Edge)",
        "MicrosoftEdgeUpdateTaskMachine",
        "Office Background Task",
        "Office Feature Updates",
        "OfficeTelemetryAgentFallBack",
        "OfficeTelemetryAgentLogOn",
        "BackgroundUploadTask (SettingSync)",
        "BackupTask (SettingSync)",
        "Notifications (User Profile)",
        "CreateObjectTask",
        "LPRemove",
        "GatherNetworkInfo",
        "SpeechModelDownloadTask",
        "WindowsActionDialog",
        "AnalyzeSystem (WDI)",
        "SilentCleanup (Disk Cleanup)",
        "CleanupOfflineContent (RetailDemo)",
        "CleanupOnline (RetailDemo)",
        "StartupAppTask",
        "appuriverifierdaily",
        "appuriverifierinstall",
    ];

    return (
        <div
            style={{
                minHeight: "100vh",
                background: "linear-gradient(135deg, #000000 0%, #0a0a0a 100%)",
                fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
            }}
        >
            <div
                style={{ maxWidth: "800px", margin: "0 auto", padding: "25px 20px" }}
            >
                {/* Header */}
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "16px",
                        marginBottom: "24px",
                    }}
                >
                    <button
                        onClick={onBack}
                        style={{
                            width: "40px",
                            height: "40px",
                            background: "#0a0a0a",
                            border: "1px solid #1a1a1a",
                            borderRadius: "10px",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            cursor: "pointer",
                            transition: "all 0.2s ease",
                        }}
                        onMouseEnter={(e) => {
                            e.currentTarget.style.background = "#1a1a1a";
                            e.currentTarget.style.borderColor = "#2a2a2a";
                        }}
                        onMouseLeave={(e) => {
                            e.currentTarget.style.background = "#0a0a0a";
                            e.currentTarget.style.borderColor = "#1a1a1a";
                        }}
                    >
                        <ArrowLeft
                            style={{ width: "20px", height: "20px", color: "#888" }}
                        />
                    </button>
                    <div>
                        <h1
                            style={{
                                color: "#e5e7eb",
                                fontSize: "24px",
                                fontWeight: "700",
                                margin: 0,
                                letterSpacing: "0.5px",
                            }}
                        >
                            HELP & DOCUMENTATION
                        </h1>
                    </div>
                </div>

                {/* Order of Operations */}
                <Section
                    id="orderOfOperations"
                    title="Order of Operations"
                    icon={HelpCircle}
                >
                    <InfoBox>
                        Follow these steps in order for the best results. Skipping steps or
                        running out of order may cause issues or reduce effectiveness.
                    </InfoBox>
                    <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                        {[
                            {
                                step: "1",
                                text: "Update Windows to the latest version to ensure you have the newest drivers and security patches",
                                color: "#111111",
                            },
                            {
                                step: "2",
                                text: "Update your GPU drivers to the latest stable version from NVIDIA/AMD/Intel's official website",
                                color: "#111111",
                            },
                            {
                                step: "3",
                                text: "Restart your PC to clear any pending updates and ensure a clean state before optimization",
                                color: "#22c55e",
                            },
                            {
                                step: "4",
                                text: "Create a System Restore Point — this is your safety net if anything goes wrong",
                                color: "#22c55e",
                            },
                            {
                                step: "5",
                                text: "Run the optimization features you want (Registry, Services, Debloat, etc.)",
                                color: "#111111",
                            },
                            {
                                step: "6",
                                text: "Restart your PC to apply all changes — many tweaks only take effect after reboot",
                                color: "#111111",
                            },
                        ].map((item) => (
                            <div
                                key={item.step}
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: "12px",
                                    padding: "12px 14px",
                                    background: "#000",
                                    border: "1px solid #1a1a1a",
                                    borderRadius: "8px",
                                }}
                            >
                                <span
                                    style={{
                                        width: "28px",
                                        height: "28px",
                                        background: item.color,
                                        borderRadius: "6px",
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center",
                                        color: "#000",
                                        fontSize: "13px",
                                        fontWeight: "700",
                                        flexShrink: 0,
                                    }}
                                >
                                    {item.step}
                                </span>
                                <span
                                    style={{
                                        color: "#d1d5db",
                                        fontSize: "12px",
                                        lineHeight: "1.5",
                                    }}
                                >
                                    {item.text}
                                </span>
                            </div>
                        ))}
                    </div>
                </Section>

                {/* Action Buttons */}
                <Section id="buttons" title="Action Buttons" icon={Settings}>
                    <HelpItem
                        icon={Shield}
                        color="#3b82f6"
                        title="Create Restore Point"
                        description="Creates a Windows System Restore checkpoint that captures your current system state and settings. If any optimization causes problems, you can use System Restore to roll back your entire system to this exact point in time. This is your primary safety mechanism, always create one before making changes."
                    />

                    <HelpItem
                        icon={Settings}
                        color="#ec4899"
                        title="Optimize Registry"
                        description="Applies a comprehensive set of registry modifications targeting gaming performance, input latency, privacy, system responsiveness and minimalism. These tweaks modify Windows behavior at the deepest level, disabling features that consume resources or cause latency. Changes are reversible via the Revert panel."
                        warning="Creates hundreds of registry modifications. While safe, some may affect Windows behavior you rely on. Review the list below and create a restore point first."
                        details={{
                            title: "Registry Modifications Applied",
                            items: registryTweaks,
                        }}
                    />

                    <HelpItem
                        icon={Calendar}
                        color="#06b6d4"
                        title="Optimize Task Scheduler"
                        description="Disables scheduled tasks that run in the background collecting telemetry, performing diagnostics, updating Microsoft products, and running maintenance routines. These tasks consume resources at unpredictable times, potentially causing stutters during intensive activities. Changes are reversible via the Revert panel."
                        details={{
                            title: "Scheduled Tasks Disabled",
                            items: scheduledTasks,
                        }}
                    />

                    <HelpItem
                        icon={DownloadCloud}
                        color="#22c55e"
                        title="Disable Windows Updates"
                        description="Completely stops the Windows Update service and related components. This prevents automatic downloads, installations, and forced restarts. Useful for PCs where mid-session updates cause problems, or for systems where you want full control. Changes are reversible via the Revert panel."
                        warning="Your system will NOT receive security patches automatically. You are responsible for manually checking and installing updates. Re-enable via the Revert panel when you want to update, then disable again afterward."
                        details={{
                            title: "Services Stopped",
                            items: [
                                "Windows Update (wuauserv)",
                                "Update Orchestrator (UsoSvc)",
                                "Windows Update Medic (WaaSMedicSvc)",
                                "Background Intelligent Transfer (BITS)",
                            ],
                        }}
                    />

                    <HelpItem
                        icon={Eye}
                        color="#f59e0b"
                        title="Lower Visual Settings"
                        description="Disables Windows visual effects and animations that consume GPU and CPU resources. This includes window animations, transparency effects (Aero), smooth scrolling, font smoothing, taskbar animations, and other cosmetic features. The result is a snappier, more responsive interface with slightly lower visual quality. Changes are reversible via the Revert panel."
                        details={{
                            title: "Visual Effects Disabled",
                            items: [
                                "Window animations",
                                "Taskbar animations",
                                "Transparency/blur effects",
                                "Smooth scrolling",
                                "Menu fade effects",
                                "Tooltip fade",
                                "Window shadow",
                                "Aero Peek",
                                "Live thumbnails",
                                "Animated controls",
                            ],
                        }}
                    />

                    <HelpItem
                        icon={Moon}
                        color="#6b7280"
                        title="Set Dark Mode"
                        description="Enables the system-wide dark theme for Windows and all applications that support it. This affects the taskbar, Start menu, Settings app, File Explorer, and compatible third-party apps. Purely cosmetic with no performance impact — just easier on the eyes, especially at night."
                    />

                    <HelpItem
                        icon={Battery}
                        color="#a855f7"
                        title="Activate HST Power Plan"
                        description="Imports and activates a HST power plan specifically tuned for gaming and low-latency applications. The result is maximum performance with zero power-saving delays."
                        warning="HST power plan significantly increases electricity consumption and hardware stress. Not recommended for laptops or battery-powered devices."
                    />

                    <HelpItem
                        icon={Zap}
                        color="#ef4444"
                        title="Remove Startup Apps"
                        description="Removes common applications from the Windows startup sequence. These apps normally launch automatically when you log in, running in the background and consuming memory/CPU before you even need them. Removing them speeds up boot time and reduces background resource usage. You can still launch these apps manually when needed."
                    />
                </Section>

                {/* Services Panel */}
                <Section id="services" title="Services Panel" icon={Activity}>
                    <WarningBox>
                        Services are background processes that Windows and applications
                        depend on. Disabling the wrong service can break functionality. Each
                        option below lists exactly which services are affected. Read
                        carefully before enabling, and test your system after each change.
                        All changes are reversible via the Revert panel.
                    </WarningBox>
                    <HelpItem
                        color="#22c55e"
                        title="HST Essentials"
                        description="The main optimization preset that disables 100+ non-essential Windows services. This covers telemetry, diagnostics, remote access, location tracking, maps, phone integration, retail demo mode, sensors, smart card readers, biometrics, mixed reality, and more.
            This single option provides the bulk of service optimizations for most gaming PCs."
                        details={{
                            title: "Services Disabled",
                            items: essentialServices,
                        }}
                    />

                    <HelpItem
                        color="#22c55e"
                        title="Bluetooth"
                        description="Completely disables all Bluetooth functionality on your system.
            This stops the Bluetooth radio, prevents device discovery, and
            removes support for all Bluetooth peripherals. Only use this if
            you never use Bluetooth devices and want to eliminate the
            background services."
                        details={{
                            title: "Services Disabled",
                            items: bluetoothServices,
                        }}
                    />

                    <HelpItem
                        color="#22c55e"
                        title="Hyper-V"
                        description="Disables Microsoft's hypervisor and all virtualization services.
            Hyper-V runs a thin virtualization layer even when you're not
            using VMs, which can affect gaming performance on some systems.
            Disabling it removes this layer entirely."
                        details={{
                            title: "Services Disabled",
                            items: hypervServices,
                        }}
                    />

                    <HelpItem
                        color="#22c55e"
                        title="Xbox"
                        description="Disables Xbox-related services that handle Xbox Live integration,
            game saves, accessories, and networking. These services run in the
            background even if you don't actively use Xbox features, consuming
            resources."
                        details={{
                            title: "Services Disabled",
                            items: xboxServices,
                        }}
                    />
                </Section>

                {/* Debloat Panel */}
                <Section id="debloat" title="Debloat Panel" icon={Trash2}>
                    <WarningBox>
                        Debloating PERMANENTLY removes applications from your system.
                        Removed apps must be reinstalled from the Microsoft Store (if
                        available) or cannot be recovered at all.
                    </WarningBox>

                    <div
                        style={{
                            background: "#000",
                            border: "1px solid #1a1a1a",
                            borderRadius: "8px",
                            padding: "14px",
                            marginBottom: "10px",
                        }}
                    >
                        <span
                            style={{
                                color: "#e5e7eb",
                                fontSize: "13px",
                                fontWeight: "700",
                                display: "block",
                                marginBottom: "8px",
                            }}
                        >
                            Microsoft Apps
                        </span>
                        <p
                            style={{
                                color: "#9ca3af",
                                fontSize: "12px",
                                lineHeight: "1.6",
                                margin: "0 0 12px 0",
                            }}
                        >
                            Removes the bloatware apps that come pre-installed with Windows.
                            Most can be reinstalled from the Microsoft Store.
                        </p>
                        <div
                            style={{
                                marginTop: "12px",
                                padding: "12px",
                                background: "#0a0a0a",
                                borderRadius: "6px",
                                border: "1px solid #1a1a1a",
                            }}
                        >
                            <p
                                style={{
                                    color: "#8b5cf6",
                                    fontSize: "11px",
                                    fontWeight: "600",
                                    marginBottom: "10px",
                                    textTransform: "uppercase",
                                }}
                            >
                                Apps Removed
                            </p>
                            <div style={{ display: "flex", flexWrap: "wrap", gap: "6px" }}>
                                {microsoftApps.map((app, i) => (
                                    <span
                                        key={i}
                                        style={{
                                            padding: "4px 8px",
                                            background: "#111",
                                            border: "1px solid #222",
                                            borderRadius: "4px",
                                            color: "#9ca3af",
                                            fontSize: "10px",
                                        }}
                                    >
                                        {app}
                                    </span>
                                ))}
                            </div>
                        </div>
                    </div>

                    <HelpItem
                        title="Microsoft Edge"
                        description="Removes the Microsoft Edge browser entirely from your system and disables Edge's ability to reinstall itself through Windows Update. You'll need an alternative browser (Chrome, Firefox, etc.) installed before removing Edge."
                        details={{
                            title: "Components Removed",
                            items: [
                                "Microsoft Edge browser",
                                "Edge Update services",
                                "Edge scheduled tasks",
                                "Edge Registry keys",
                                "Edge shortcuts",
                            ],
                        }}
                    />

                    <HelpItem
                        title="OneDrive"
                        description="Removes Microsoft OneDrive from your system including the sync client, scheduled tasks, and startup entries. OneDrive will no longer sync with the cloud. Make sure to turn off file sync before removing OneDrive"
                        details={{
                            title: "Components Removed",
                            items: [
                                "OneDrive sync client",
                                "OneDrive shell integration",
                                "OneDrive startup entry",
                                "OneDrive scheduled tasks",
                                "File Explorer sidebar entry",
                                "OneDrive context menus",
                            ],
                        }}
                    />

                    <HelpItem
                        title="Xbox Apps"
                        description="Removes the Xbox app ecosystem from Windows. This is a more thorough removal than just disabling Xbox services."
                        details={{
                            title: "Apps Removed",
                            items: [
                                "Xbox App",
                                "Xbox Game Bar",
                                "Xbox Identity Provider",
                                "Xbox Speech to Text Overlay",
                                "Xbox TCUI",
                                "Xbox Gaming Overlay",
                                "Game Bar Presence Writer",
                            ],
                        }}
                    />

                    <HelpItem
                        title="Microsoft Store"
                        description="Removes the Microsoft Store app and its supporting components. This prevents installation of UWP (Universal Windows Platform) apps and removes access to the Store's app catalog. Windows will no longer be able to install or update Store apps automatically."
                        details={{
                            title: "Components Removed",
                            items: [
                                "Microsoft Store app",
                                "Store Purchase App",
                                "Store Experience Host",
                            ],
                        }}
                    />
                </Section>

                {/* Cleanup Panel */}
                <Section id="cleanup" title="Cleanup Panel" icon={Sparkles}>
                    <InfoBox>
                        Cleanup operations are safe and non-destructive. They remove
                        temporary files, caches, and logs that Windows regenerates as
                        needed. Running cleanup periodically can free up significant disk
                        space without affecting system functionality.
                    </InfoBox>

                    <HelpItem
                        title="Temporary Files"
                        description="Clears temporary files from multiple locations including the Windows temp folder (C:\Windows\Temp), user temp folder (%TEMP%), prefetch cache (used for app launch optimization), thumbnail cache, Windows Update cleanup files, and other temporary data. These files accumulate over time and can consume gigabytes of disk space."
                        details={{
                            title: "Locations Cleaned",
                            items: [
                                "C:\\Windows\\Temp",
                                "%USERPROFILE%\\AppData\\Local\\Temp",
                                "C:\\Windows\\Prefetch",
                                "Thumbnail cache",
                                "Windows Update cache",
                                "Windows Installer cache",
                                "Delivery Optimization files",
                                "DirectX Shader cache",
                            ],
                        }}
                    />
                    <HelpItem
                        title="Recycle Bin"
                        description="Clears Recycle Bin"
                    />

                    <HelpItem
                        title="Browser Cache"
                        description="Clears cached data from all major web browsers including downloaded files, images, scripts, cookies, and site data. This can free up significant space especially if you browse media-heavy sites. You may need to re-login to websites after clearing, and some sites may load slightly slower on first visit."
                        details={{
                            title: "Browsers Cleaned",
                            items: ["Only Google Chrome for now..."],
                        }}
                    />

                    <HelpItem
                        title="Event Logs"
                        description="Clears all Windows Event Logs including Application, Security, System, and Setup logs. Event logs record system events, errors, and security audits. Clearing them frees disk space and gives you a clean slate for troubleshooting. This does not affect system functionality."
                        details={{
                            title: "Logs Cleared",
                            items: [
                                "Application Log",
                                "Security Log",
                                "System Log",
                                "Setup Log",
                                "Forwarded Events",
                                "Custom application logs",
                            ],
                        }}
                    />

                    <HelpItem
                        title="Default Power Plans"
                        description="Removes the default Windows power plans (Balanced, Power Saver, High Performance) leaving only the HST Power Plan or your currently active custom plan. Make sure you've activated the HST power plan before deleting power plans"
                        details={{
                            title: "Plans Removed",
                            items: [
                                "Balanced",
                                "Power Saver",
                                "High Performance",
                                "Ultimate Performance (if present)",
                            ],
                        }}
                    />
                </Section>

                {/* Revert Panel */}
                <Section id="revert" title="Revert Panel" icon={RotateCcw}>
                    <InfoBox>
                        The Revert panel restores Windows defaults — it does NOT restore
                        your personal configuration from before using HST Utility. To fully
                        undo changes, use Windows System Restore to roll back to your
                        restore point. The Revert panel is useful for selectively
                        re-enabling specific features without a full system restore.
                    </InfoBox>

                    <HelpItem
                        title="Services"
                        description="Restores ALL services to their Windows default startup configuration. This includes services disabled by HST Essentials, Bluetooth, Hyper-V, Xbox, and Windows Update options. Each service is set back to its original startup type (Automatic, Manual, or Disabled as Windows intended). A restart is recommended after reverting services."
                    />

                    <HelpItem
                        title="Task Scheduler"
                        description="Re-enables all scheduled tasks that were disabled by the Task Scheduler optimization. This includes telemetry tasks, Office tasks, Edge update tasks, and all other disabled items. Tasks will resume their normal schedules after the next reboot or when their trigger conditions are met."
                    />

                    <HelpItem
                        title="Windows Update"
                        description="Re-enables Windows Update services and restores automatic update functionality. After reverting, Windows will check for updates on its normal schedule, download them automatically, and may prompt you to restart for installation. Use this when you want to install updates, then run 'Disable Windows Updates' again afterward."
                    />

                    <HelpItem
                        title="Registry"
                        description="Removes or resets registry modifications made by the 'Optimize Registry' and 'Lower Visual Settings' feature. This restores Windows default behavior for all settings. Some changes may require a restart to fully take effect."
                    />
                </Section>

                {/* Warnings */}
                <Section
                    id="warnings"
                    title="Warnings & Risks"
                    icon={AlertTriangle}
                    color="#ef4444"
                >
                    <h4
                        style={{
                            color: "#e5e7eb",
                            fontSize: "13px",
                            fontWeight: "700",
                            marginBottom: "12px",
                        }}
                    >
                        What Can Break
                    </h4>
                    <div style={{ display: "grid", gap: "8px", marginBottom: "16px" }}>
                        {[
                            {
                                feature: "Printing",
                                cause: "HST Essentials disables Print Spooler service",
                                fix: "Revert → Services, or enable 'Print Spooler' in services.msc",
                            },
                            {
                                feature: "Bluetooth devices",
                                cause: "Bluetooth option disables all BT services",
                                fix: "Revert → Services, or enable Bluetooth services in services.msc",
                            },
                            {
                                feature: "Remote Desktop",
                                cause: "HST Essentials disables RDP services",
                                fix: "Revert → Services, or enable Remote Desktop services",
                            },
                            {
                                feature: "WSL2 / Docker",
                                cause: "Hyper-V option disables virtualization",
                                fix: "Revert → Services, then re-enable Hyper-V in Windows Features",
                            },
                            {
                                feature: "Xbox Game Pass",
                                cause: "Xbox services disabled or apps removed",
                                fix: "Revert → Services, reinstall Xbox apps from Store",
                            },
                            {
                                feature: "Windows Search",
                                cause: "HST Essentials disables WSearch indexing",
                                fix: "Revert → Services, or enable 'Windows Search' in services.msc",
                            },
                            {
                                feature: "Store apps",
                                cause: "Microsoft Store removed via Debloat",
                                fix: "System Restore, or PowerShell: Get-AppxPackage *Store* | Add-AppxPackage",
                            },
                            {
                                feature: "OneDrive sync",
                                cause: "OneDrive removed via Debloat",
                                fix: "Download OneDrive installer from microsoft.com/onedrive",
                            },
                        ].map((item) => (
                            <div
                                key={item.feature}
                                style={{
                                    padding: "12px 14px",
                                    background: "#000",
                                    border: "1px solid #1a1a1a",
                                    borderRadius: "8px",
                                }}
                            >
                                <div
                                    style={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "10px",
                                        marginBottom: "6px",
                                    }}
                                >
                                    <XCircle
                                        style={{ width: "16px", height: "16px", color: "#ef4444" }}
                                    />
                                    <span
                                        style={{
                                            color: "#e5e7eb",
                                            fontSize: "12px",
                                            fontWeight: "600",
                                        }}
                                    >
                                        {item.feature}
                                    </span>
                                </div>
                                <p
                                    style={{
                                        color: "#6b7280",
                                        fontSize: "11px",
                                        margin: "0 0 6px 26px",
                                    }}
                                >
                                    {item.cause}
                                </p>
                                <p
                                    style={{
                                        color: "#22c55e",
                                        fontSize: "11px",
                                        margin: "0 0 0 26px",
                                    }}
                                >
                                    Fix: {item.fix}
                                </p>
                            </div>
                        ))}
                    </div>

                    <h4
                        style={{
                            color: "#e5e7eb",
                            fontSize: "13px",
                            fontWeight: "700",
                            marginBottom: "12px",
                        }}
                    >
                        Recovery Methods
                    </h4>
                    <div style={{ display: "grid", gap: "8px" }}>
                        {[
                            {
                                method: "System Restore",
                                desc: "Roll back entire system to your restore point. Best for major issues.",
                                cmd: "'Create a restore point' →  Hold Shift + click Restart → Troubleshoot → Use system Restore",
                            },
                            {
                                method: "HST Revert Panel",
                                desc: "Selectively restore services, tasks, or registry to Windows defaults.",
                                cmd: "Use the Revert panel in HST WINDOWS UTILITY",
                            },
                            {
                                method: "Safe Mode",
                                desc: "Boot into Safe Mode if system won't start normally.",
                                cmd: "Hold Shift + click Restart → Troubleshoot → Startup Settings",
                            },
                            {
                                method: "SFC Scan",
                                desc: "Repair corrupted system files.",
                                cmd: "Run as Admin: sfc /scannow",
                            },
                            {
                                method: "DISM Repair",
                                desc: "Fix Windows image corruption.",
                                cmd: "Run as Admin: DISM /Online /Cleanup-Image /RestoreHealth",
                            },
                        ].map((item, i) => (
                            <div
                                key={i}
                                style={{
                                    padding: "12px 14px",
                                    background: "#000",
                                    border: "1px solid #1a1a1a",
                                    borderRadius: "8px",
                                }}
                            >
                                <div
                                    style={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "10px",
                                        marginBottom: "6px",
                                    }}
                                >
                                    <CheckCircle
                                        style={{ width: "16px", height: "16px", color: "#22c55e" }}
                                    />
                                    <span
                                        style={{
                                            color: "#e5e7eb",
                                            fontSize: "12px",
                                            fontWeight: "600",
                                        }}
                                    >
                                        {item.method}
                                    </span>
                                </div>
                                <p
                                    style={{
                                        color: "#9ca3af",
                                        fontSize: "11px",
                                        margin: "0 0 4px 26px",
                                    }}
                                >
                                    {item.desc}
                                </p>
                                <code
                                    style={{
                                        display: "block",
                                        color: "#8b5cf6",
                                        fontSize: "10px",
                                        margin: "0 0 0 26px",
                                        fontFamily: "monospace",
                                    }}
                                >
                                    {item.cmd}
                                </code>
                            </div>
                        ))}
                    </div>
                </Section>

                {/* Footer */}
                <div
                    style={{
                        textAlign: "center",
                        padding: "20px 0",
                        borderTop: "1px solid #1a1a1a",
                        marginTop: "20px",
                    }}
                >
                    <p style={{ color: "#6b7280", fontSize: "11px", margin: 0 }}>
                        HST WINDOWS UTILITY • Use at your own risk
                    </p>
                </div>
            </div>
        </div>
    );
};

// ============================================================================
// MAIN APP COMPONENT
// ============================================================================
export default function HSTWindowsUtility() {
    const [currentPage, setCurrentPage] = useState("main");
    const [apiStatus, setApiStatus] = useState("CHECKING");
    const [currentStatus, setCurrentStatus] = useState(
        "READY - SELECT AN OPERATION"
    );
    const [activeOperation, setActiveOperation] = useState(null);
    const [systemInfo, setSystemInfo] = useState({
        user: "LOADING...",
        time: "LOADING...",
        gpu: "LOADING...",
        cpu: "LOADING...",
        ram: "LOADING...",
        storage: "LOADING...",
    });

    const [serviceOptions, setServiceOptions] = useState({
        recommended: false,
        bluetooth: false,
        hyperv: false,
        xbox: false,
    });

    const [debloatOptions, setDebloatOptions] = useState({
        msApps: false,
        edge: false,
        onedrive: false,
        xboxApps: false,
        storeApps: false,
    });

    const [cleanupOptions, setCleanupOptions] = useState({
        temp: false,
        cache: false,
        eventLogs: false,
        powerPlans: false,
    });

    const [revertOptions, setRevertOptions] = useState({
        service: false,
        task: false,
        wUpdate: false,
        registry: false,
    });

    useEffect(() => {
        checkApiStatus();
        fetchSystemInfo();
    }, []);

    const checkApiStatus = async () => {
        try {
            const response = await fetch("http://localhost:5200/api/system/test");
            setApiStatus(response.ok ? "online" : "offline");
        } catch (error) {
            setApiStatus("offline");
        }
    };

    const fetchSystemInfo = async () => {
        try {
            const response = await fetch("http://localhost:5200/api/system/sysinfo");
            if (response.ok) {
                const data = await response.json();
                setSystemInfo(data);
            } else {
                setSystemInfo({
                    user: "ERROR",
                    time: "ERROR",
                    gpu: "ERROR",
                    cpu: "ERROR",
                    ram: "ERROR",
                    storage: "ERROR",
                });
            }
        } catch (error) {
            setSystemInfo({
                user: "OFFLINE",
                time: "OFFLINE",
                gpu: "OFFLINE",
                cpu: "OFFLINE",
                ram: "OFFLINE",
                storage: "OFFLINE",
            });
        }
    };

    const executeApiCall = async (endpoint, loadingMessage, body = null) => {
        if (apiStatus !== "online") {
            setCurrentStatus("API OFFLINE");
            setTimeout(() => setCurrentStatus("READY - SELECT AN OPERATION"), 3000);
            return;
        }

        setActiveOperation(endpoint);
        setCurrentStatus(loadingMessage);

        try {
            const options = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Accept: "application/json",
                },
            };
            if (body) options.body = JSON.stringify(body);

            const response = await fetch(
                `http://localhost:5200/api/system/${endpoint}`,
                options
            );
            const result = await response.json();
            setCurrentStatus(result.status);
        } catch (error) {
            console.error("API ERROR:", error);
            setCurrentStatus(`NETWORK ERROR: ${error.message}`);
        }

        setTimeout(() => {
            setCurrentStatus("READY - SELECT AN OPERATION");
            setActiveOperation(null);
        }, 4000);
    };

    const NeonButton = ({
        children,
        onClick,
        icon: Icon,
        gradient,
        textColor = "#ffffff",
        disabled = false,
    }) => (
        <button
            onClick={onClick}
            disabled={disabled || activeOperation !== null}
            style={{
                width: "100%",
                height: "38px",
                background:
                    gradient || "linear-gradient(135deg, #0a0a0a 0%, #000000 100%)",
                border: "none",
                borderRadius: "8px",
                color: textColor,
                fontSize: "11px",
                fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
                fontWeight: "700",
                cursor: disabled || activeOperation ? "not-allowed" : "pointer",
                transition: "all 0.2s ease",
                textTransform: "uppercase",
                letterSpacing: "0.8px",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                gap: "8px",
                position: "relative",
                opacity: disabled || activeOperation ? 0.5 : 1,
                boxShadow: "0 2px 10px rgba(0, 0, 0, 0.3)",
            }}
            onMouseEnter={(e) => {
                if (!disabled && !activeOperation) {
                    e.currentTarget.style.transform = "translateY(-1px)";
                    e.currentTarget.style.boxShadow = "0 4px 20px rgba(0, 0, 0, 0.5)";
                }
            }}
            onMouseLeave={(e) => {
                if (!disabled && !activeOperation) {
                    e.currentTarget.style.transform = "translateY(0)";
                    e.currentTarget.style.boxShadow = "0 2px 10px rgba(0, 0, 0, 0.3)";
                }
            }}
        >
            <Icon style={{ width: "15px", height: "15px", color: textColor }} />
            <span>{children}</span>
        </button>
    );

    const NeonCheckbox = ({ checked, onChange, label, color = "#ffffff" }) => (
        <label
            style={{
                display: "flex",
                alignItems: "center",
                gap: "6px",
                cursor: "pointer",
                padding: "4px 8px",
                borderRadius: "6px",
                transition: "all 0.2s ease",
                background: checked ? "rgba(255, 255, 255, 0.05)" : "transparent",
            }}
            onMouseEnter={(e) => {
                e.currentTarget.style.background = checked
                    ? "rgba(255, 255, 255, 0.08)"
                    : "rgba(255, 255, 255, 0.02)";
            }}
            onMouseLeave={(e) => {
                e.currentTarget.style.background = checked
                    ? "rgba(255, 255, 255, 0.05)"
                    : "transparent";
            }}
        >
            <div
                style={{
                    width: "16px",
                    height: "16px",
                    borderRadius: "4px",
                    border: `2px solid ${checked ? color : "#444444"}`,
                    background: checked ? color : "transparent",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    transition: "all 0.2s ease",
                }}
            >
                {checked && (
                    <Check
                        style={{
                            width: "10px",
                            height: "10px",
                            color: "black",
                            strokeWidth: 3,
                        }}
                    />
                )}
            </div>
            <input
                type="checkbox"
                checked={checked}
                onChange={onChange}
                style={{ display: "none" }}
            />
            <span
                style={{
                    color: "#cccccc",
                    fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
                    fontWeight: "600",
                    fontSize: "12px",
                }}
            >
                {label}
            </span>
        </label>
    );

    const OptionsPanel = ({
        title,
        options,
        setOptions,
        onExecute,
        color,
        gradient,
        icon: Icon,
    }) => {
        const optionLabels = {
            recommended: "HST Essentials",
            bluetooth: "Bluetooth",
            hyperv: "Hyper-V",
            xbox: "Xbox",
            msApps: "Microsoft Apps",
            edge: "Microsoft Edge",
            onedrive: "OneDrive",
            xboxApps: "Xbox Apps",
            storeApps: "Microsoft Store",
            temp: "Temporary Files",
            cache: "Browser Cache",
            eventLogs: "Event Logs",
            powerPlans: "Default Powerplans",
            service: "Services",
            task: "Task Scheduler",
            wUpdate: "Windows Update",
            registry: "Registry",
        };

        return (
            <div
                style={{
                    background: "#0a0a0a",
                    border: "1px solid #1a1a1a",
                    borderRadius: "12px",
                    padding: "14px",
                    boxShadow: "0 4px 12px rgba(0, 0, 0, 0.5)",
                    transition: "all 0.2s ease",
                    height: "230px",
                    display: "flex",
                    flexDirection: "column",
                }}
                onMouseEnter={(e) => {
                    e.currentTarget.style.borderColor = "#2a2a2a";
                    e.currentTarget.style.transform = "translateY(-1px)";
                }}
                onMouseLeave={(e) => {
                    e.currentTarget.style.borderColor = "#1a1a1a";
                    e.currentTarget.style.transform = "translateY(0)";
                }}
            >
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        gap: "8px",
                        marginBottom: "8px",
                    }}
                >
                    <Icon style={{ width: "16px", height: "16px", color }} />
                    <h3
                        style={{
                            color: "#f3f4f6",
                            fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
                            fontWeight: "700",
                            fontSize: "13px",
                            textTransform: "uppercase",
                            letterSpacing: "0.5px",
                            margin: 0,
                        }}
                    >
                        {title}
                    </h3>
                </div>

                <div style={{ marginBottom: "4px", flex: 1 }}>
                    {Object.entries(options).map(([key, value]) => (
                        <div key={key} style={{ marginBottom: "4px" }}>
                            <NeonCheckbox
                                checked={value}
                                onChange={(e) =>
                                    setOptions((prev) => ({ ...prev, [key]: e.target.checked }))
                                }
                                label={optionLabels[key] || key.toUpperCase()}
                                color={color}
                            />
                        </div>
                    ))}
                </div>

                <NeonButton
                    onClick={onExecute}
                    gradient={gradient}
                    icon={Icon}
                    textColor={color}
                >
                    Execute
                </NeonButton>
            </div>
        );
    };

    // Render Help Page
    if (currentPage === "help") {
        return <HelpPage onBack={() => setCurrentPage("main")} />;
    }

    // Render Main Page
    return (
        <div
            style={{
                minHeight: "100vh",
                background: "linear-gradient(135deg, #000000 0%, #0a0a0a 100%)",
                fontFamily: "'Segoe UI', system-ui, -apple-system, sans-serif",
                position: "relative",
                overflow: "hidden",
            }}
        >
            {/* Top Right Buttons */}
            <div
                style={{
                    position: "fixed",
                    top: "20px",
                    right: "20px",
                    zIndex: 1000,
                    display: "flex",
                    gap: "10px",
                }}
            >
                <button
                    onClick={() => setCurrentPage("help")}
                    style={{
                        width: "36px",
                        height: "36px",
                        background: "#0a0a0a",
                        border: "1px solid #1a1a1a",
                        borderRadius: "50%",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        cursor: "pointer",
                        transition: "all 0.2s ease",
                        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.5)",
                    }}
                    onMouseEnter={(e) => {
                        e.currentTarget.style.background = "#1a1a1a";
                        e.currentTarget.style.borderColor = "#8b5cf6";
                    }}
                    onMouseLeave={(e) => {
                        e.currentTarget.style.background = "#0a0a0a";
                        e.currentTarget.style.borderColor = "#1a1a1a";
                    }}
                >
                    <HelpCircle
                        style={{ width: "18px", height: "18px", color: "#8b5cf6" }}
                    />
                </button>

                <button
                    onClick={() => window.close()}
                    style={{
                        width: "36px",
                        height: "36px",
                        background: "#0a0a0a",
                        border: "1px solid #1a1a1a",
                        borderRadius: "50%",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        cursor: "pointer",
                        transition: "all 0.2s ease",
                        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.5)",
                    }}
                    onMouseEnter={(e) => {
                        e.currentTarget.style.background = "#1a1a1a";
                        e.currentTarget.style.borderColor = "#2a2a2a";
                    }}
                    onMouseLeave={(e) => {
                        e.currentTarget.style.background = "#0a0a0a";
                        e.currentTarget.style.borderColor = "#1a1a1a";
                    }}
                >
                    <X style={{ width: "18px", height: "18px", color: "#666666" }} />
                </button>
            </div>

            <div
                style={{
                    maxWidth: "1000px",
                    margin: "0 auto",
                    padding: "25px 20px",
                    position: "relative",
                    zIndex: 1,
                }}
            >
                {/* Header */}
                <div style={{ textAlign: "center", marginBottom: "25px" }}>
                    <p
                        style={{
                            color: "#e5e7eb",
                            fontSize: "33px",
                            fontWeight: "700",
                            marginBottom: "4px",
                            letterSpacing: "1px",
                            textTransform: "uppercase",
                        }}
                    >
                        HST Windows Utility
                    </p>
                    <p
                        style={{
                            color: "#9ca3af",
                            fontSize: "11px",
                            fontWeight: "600",
                            letterSpacing: "0.5px",
                            textTransform: "uppercase",
                        }}
                    >
                        Create Restore Point Before Modifications
                    </p>

                    {/* API Status Badge */}
                    <div
                        style={{
                            display: "inline-flex",
                            alignItems: "center",
                            gap: "6px",
                            padding: "6px 12px",
                            background:
                                apiStatus === "online"
                                    ? "rgba(139, 92, 246, 0.15)"
                                    : "rgba(239, 68, 68, 0.1)",
                            borderRadius: "16px",
                            border: `1px solid ${apiStatus === "online" ? "#8b5cf6" : "#ef4444"
                                }`,
                            marginTop: "16px",
                        }}
                    >
                        {apiStatus === "online" ? (
                            <Wifi
                                style={{ width: "14px", height: "14px", color: "#8b5cf6" }}
                            />
                        ) : (
                            <WifiOff
                                style={{ width: "14px", height: "14px", color: "#ef4444" }}
                            />
                        )}
                        <span
                            style={{
                                fontSize: "11px",
                                fontWeight: "700",
                                letterSpacing: "0.5px",
                                color: apiStatus === "online" ? "#8b5cf6" : "#ef4444",
                                textTransform: "uppercase",
                            }}
                        >
                            API {apiStatus}
                        </span>
                    </div>

                    {/* Status Bar */}
                    <div
                        style={{
                            background: "#0a0a0a",
                            border: "1px solid #1a1a1a",
                            borderRadius: "8px",
                            padding: "12px 16px",
                            marginTop: "16px",
                            boxShadow: "0 2px 8px rgba(0, 0, 0, 0.5)",
                        }}
                    >
                        <div
                            style={{
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                gap: "8px",
                            }}
                        >
                            {activeOperation ? (
                                <Loader
                                    style={{ width: "16px", height: "16px", color: "#8b5cf6" }}
                                    className="animate-spin"
                                />
                            ) : (
                                <CheckCircle
                                    style={{ width: "16px", height: "16px", color: "#8b5cf6" }}
                                />
                            )}
                            <span
                                style={{
                                    color: "#e5e7eb",
                                    fontWeight: "600",
                                    fontSize: "12px",
                                    letterSpacing: "0.3px",
                                }}
                            >
                                {currentStatus}
                            </span>
                        </div>
                    </div>
                </div>

                {/* Main Content Grid */}
                <div
                    style={{
                        display: "grid",
                        gridTemplateColumns: "1.7fr 240px",
                        gap: "16px",
                        marginBottom: "16px",
                    }}
                >
                    {/* Left - System Info Panel */}
                    <div
                        style={{
                            background: "#0a0a0a",
                            border: "1px solid #1a1a1a",
                            borderRadius: "12px",
                            padding: "18px",
                            boxShadow: "0 4px 12px rgba(0, 0, 0, 0.5)",
                            display: "flex",
                            flexDirection: "column",
                        }}
                    >
                        <div
                            style={{
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                gap: "8px",
                                marginBottom: "16px",
                            }}
                        >
                            <Server
                                style={{ width: "18px", height: "18px", color: "#8b5cf6" }}
                            />
                            <h2
                                style={{
                                    color: "#f3f4f6",
                                    fontSize: "14px",
                                    fontWeight: "700",
                                    textTransform: "uppercase",
                                    letterSpacing: "0.5px",
                                    margin: 0,
                                }}
                            >
                                System Information
                            </h2>
                        </div>

                        <div
                            style={{
                                display: "grid",
                                gridTemplateColumns: "repeat(2, 1fr)",
                                gap: "18px",
                                marginBottom: "16px",
                                flex: 1,
                                alignContent: "stretch",
                                gridAutoRows: "1fr",
                            }}
                        >
                            {[
                                {
                                    label: "USER",
                                    value: systemInfo.user,
                                    icon: User,
                                    color: "#8b5cf6",
                                },
                                {
                                    label: "TIME",
                                    value: systemInfo.time,
                                    icon: Watch,
                                    color: "#8b5cf6",
                                },
                                {
                                    label: "GPU",
                                    value: systemInfo.gpu,
                                    icon: Monitor,
                                    color: "#8b5cf6",
                                },
                                {
                                    label: "CPU",
                                    value: systemInfo.cpu,
                                    icon: Cpu,
                                    color: "#c084fc",
                                },
                                {
                                    label: "RAM",
                                    value: systemInfo.ram,
                                    icon: MemoryStick,
                                    color: "#c084fc",
                                },
                                {
                                    label: "Storage",
                                    value: systemInfo.storage,
                                    icon: HardDrive,
                                    color: "#c084fc",
                                },
                            ].map((item, i) => (
                                <div
                                    key={i}
                                    style={{
                                        background: "#000000",
                                        border: "1px solid #1a1a1a",
                                        borderRadius: "8px",
                                        padding: "10px",
                                        transition: "all 0.2s ease",
                                        cursor: "pointer",
                                        display: "flex",
                                        flexDirection: "column",
                                        justifyContent: "center",
                                        alignItems: "center",
                                        textAlign: "center",
                                    }}
                                    onMouseEnter={(e) => {
                                        e.currentTarget.style.borderColor = "#333333";
                                        e.currentTarget.style.background = "#0a0a0a";
                                    }}
                                    onMouseLeave={(e) => {
                                        e.currentTarget.style.borderColor = "#1a1a1a";
                                        e.currentTarget.style.background = "#000000";
                                    }}
                                >
                                    <div
                                        style={{
                                            display: "flex",
                                            alignItems: "center",
                                            gap: "6px",
                                            marginBottom: "4px",
                                        }}
                                    >
                                        <item.icon
                                            style={{
                                                width: "14px",
                                                height: "14px",
                                                color: item.color,
                                            }}
                                        />
                                        <div
                                            style={{
                                                color: "#9ca3af",
                                                fontSize: "10px",
                                                textTransform: "uppercase",
                                                letterSpacing: "0.5px",
                                                fontWeight: "700",
                                            }}
                                        >
                                            {item.label}
                                        </div>
                                    </div>
                                    <div
                                        style={{
                                            color: "#f3f4f6",
                                            fontWeight: "600",
                                            fontSize: "12px",
                                            whiteSpace: "nowrap",
                                            overflow: "hidden",
                                            textOverflow: "ellipsis",
                                            width: "100%",
                                        }}
                                    >
                                        {item.value}
                                    </div>
                                </div>
                            ))}
                        </div>

                        <div style={{ textAlign: "center" }}>
                            <a
                                href="https://github.com/hselimt"
                                target="_blank"
                                rel="noopener noreferrer"
                                style={{
                                    display: "inline-flex",
                                    alignItems: "center",
                                    gap: "6px",
                                    padding: "7px 14px",
                                    background: "#000000",
                                    border: "1px solid #1a1a1a",
                                    borderRadius: "6px",
                                    color: "#ffffff",
                                    textDecoration: "none",
                                    fontWeight: "700",
                                    fontSize: "11px",
                                    letterSpacing: "0.5px",
                                    transition: "all 0.2s ease",
                                }}
                                onMouseEnter={(e) => {
                                    e.currentTarget.style.borderColor = "#333333";
                                    e.currentTarget.style.color = "#cccccc";
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.style.borderColor = "#1a1a1a";
                                    e.currentTarget.style.color = "#ffffff";
                                }}
                            >
                                <Github style={{ width: "14px", height: "14px" }} />
                                GITHUB
                            </a>
                        </div>
                    </div>

                    {/* Right - 8 Operation Buttons */}
                    <div
                        style={{ display: "grid", gridTemplateColumns: "1fr", gap: "7px" }}
                    >
                        <NeonButton
                            onClick={() =>
                                executeApiCall("restore-point", "CREATING RESTORE POINT...")
                            }
                            icon={Shield}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#3b82f6"
                        >
                            CREATE RESTORE POINT
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall("optimize-registry", "OPTIMIZING REGISTRY...")
                            }
                            icon={Settings}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#ec4899"
                        >
                            OPTIMIZE REGISTRY
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall(
                                    "optimize-taskscheduler",
                                    "OPTIMIZING TASK SCHEDULER..."
                                )
                            }
                            icon={Calendar}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#06b6d4"
                        >
                            OPTIMIZE TASK SCHEDULER
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall(
                                    "disable-updates",
                                    "DISABLING WINDOWS UPDATES..."
                                )
                            }
                            icon={DownloadCloud}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#22c55e"
                        >
                            DISABLE WINDOWS UPDATES
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall("lower-visuals", "LOWERING VISUALS SETTINGS...")
                            }
                            icon={Eye}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#f59e0b"
                        >
                            LOWER VISUAL SETTINGS
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall("set-darkmode", "SETTING DARK MODE...")
                            }
                            icon={Moon}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#6b7280"
                        >
                            SET DARK MODE
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall(
                                    "set-powerplan",
                                    "ADDING AND ACTIVATING POWER PLAN..."
                                )
                            }
                            icon={Battery}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#a855f7"
                        >
                            ACTIVATE HST POWERPLAN
                        </NeonButton>
                        <NeonButton
                            onClick={() =>
                                executeApiCall(
                                    "remove-startup-apps",
                                    "REMOVING STARTUP APPS..."
                                )
                            }
                            icon={Zap}
                            gradient="linear-gradient(135deg, #0a0a0a 0%, #000000 100%)"
                            textColor="#ef4444"
                        >
                            REMOVE STARTUP APPS
                        </NeonButton>
                    </div>
                </div>

                {/* Bottom - 4 Cards */}
                <div
                    style={{
                        display: "grid",
                        gridTemplateColumns: "repeat(4, 1fr)",
                        gap: "16px",
                        marginBottom: "0px",
                    }}
                >
                    <OptionsPanel
                        title="Services"
                        options={serviceOptions}
                        setOptions={setServiceOptions}
                        onExecute={() => {
                            const selected = Object.entries(serviceOptions).filter(
                                ([_, checked]) => checked
                            );
                            if (selected.length === 0) {
                                setCurrentStatus("NO SERVICES SELECTED");
                                setTimeout(
                                    () => setCurrentStatus("READY - SELECT AN OPERATION"),
                                    2000
                                );
                            } else {
                                executeApiCall("optimize-services", "OPTIMIZING SERVICES...", {
                                    recommended: serviceOptions.recommended,
                                    bluetooth: serviceOptions.bluetooth,
                                    hyperv: serviceOptions.hyperv,
                                    xbox: serviceOptions.xbox,
                                });
                            }
                        }}
                        color="#ffffff"
                        gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
                        icon={Activity}
                    />

                    <OptionsPanel
                        title="Debloat"
                        options={debloatOptions}
                        setOptions={setDebloatOptions}
                        onExecute={() => {
                            const selected = Object.entries(debloatOptions).filter(
                                ([_, checked]) => checked
                            );
                            if (selected.length === 0) {
                                setCurrentStatus("NO ITEMS SELECTED");
                                setTimeout(
                                    () => setCurrentStatus("READY - SELECT AN OPERATION"),
                                    2000
                                );
                            } else {
                                executeApiCall("debloat-apps", "DEBLOATING WINDOWS...", {
                                    msApps: debloatOptions.msApps,
                                    edge: debloatOptions.edge,
                                    onedrive: debloatOptions.onedrive,
                                    xboxApps: debloatOptions.xboxApps,
                                    storeApps: debloatOptions.storeApps,
                                });
                            }
                        }}
                        color="#ffffff"
                        gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
                        icon={Trash2}
                    />

                    <OptionsPanel
                        title="Cleanup"
                        options={cleanupOptions}
                        setOptions={setCleanupOptions}
                        onExecute={() => {
                            const selected = Object.entries(cleanupOptions).filter(
                                ([_, checked]) => checked
                            );
                            if (selected.length === 0) {
                                setCurrentStatus("NO CLEANUP ITEMS SELECTED");
                                setTimeout(
                                    () => setCurrentStatus("READY - SELECT AN OPERATION"),
                                    2000
                                );
                            } else {
                                executeApiCall("cleanup", "CLEANING WINDOWS...", {
                                    temp: cleanupOptions.temp,
                                    cache: cleanupOptions.cache,
                                    eventLog: cleanupOptions.eventLogs,
                                    powerPlan: cleanupOptions.powerPlans,
                                });
                            }
                        }}
                        color="#ffffff"
                        gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
                        icon={Sparkles}
                    />

                    <OptionsPanel
                        title="Revert Back"
                        options={revertOptions}
                        setOptions={setRevertOptions}
                        onExecute={() => {
                            const selected = Object.entries(revertOptions).filter(
                                ([_, checked]) => checked
                            );
                            if (selected.length === 0) {
                                setCurrentStatus("NO OPTIONS SELECTED FOR REVERT");
                                setTimeout(
                                    () => setCurrentStatus("READY - SELECT AN OPERATION"),
                                    2000
                                );
                            } else {
                                executeApiCall(
                                    "revert-configurations",
                                    "REVERTING CONFIGURATIONS...",
                                    {
                                        service: revertOptions.service,
                                        task: revertOptions.task,
                                        wUpdate: revertOptions.wUpdate,
                                        registry: revertOptions.registry,
                                    }
                                );
                            }
                        }}
                        color="#ffffff"
                        gradient="linear-gradient(135deg, #1a1a1a 0%, #0a0a0a 100%)"
                        icon={RotateCcw}
                    />
                </div>
            </div>
        </div>
    );
}
