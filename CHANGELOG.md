## [1.8.0] - 2026-07-20

### Added
* **Clean Script** - `Clean-GUI.bat` removes build artifacts (bin, obj, dist, node_modules, wwwroot) while preserving the Powerplan folder

### Changed
* **Installer** - Replaced portable exe distribution with an NSIS installer (Start Menu entry, optional desktop shortcut, install wizard instead of silent one-click install)
* **Package Size** - Cut packaged app size from ~687MB to ~350MB by deleting the leftover intermediate `.NET` build directory after publish, pruning Electron's bundled locales to `en-US` only, and excluding compile-time reference assemblies (`ref/`) from the publish output
* **System Information Panel** - TIME field replaced with OS (name + build number); RAM now shows used/total plus module speed; Storage now detects the actual Windows system drive instead of assuming `C:`, and shows used/total instead of free space only
* **External Links** - Links (e.g. GitHub) now open in the default browser instead of navigating inside the app window

### Fixed
* **Blank Window on Some GPUs** - The app could launch to an empty window on machines where Chromium's GPU process crashed. Hardware acceleration is now disabled, resolving it
* **Silent Startup Failure** - A native error dialog now shows the failure reason when the backend fails to start, instead of the app silently vanishing with no explanation
* **Duplicate Instances** - Launching the app while it's already running now focuses the existing window instead of starting a second instance with its own backend
* **Orphaned Elevated Processes** - `ProcessRunner` now kills the entire process tree on a command timeout instead of abandoning it running in the background
* **Backend Readiness Check** - The Electron shell now verifies the backend is serving the actual app before loading it, instead of accepting any HTTP response
* **CLI Hardcoded Paths** - Cleanup and app-removal routines now resolve `%WINDIR%`, `%ProgramFiles%`, `%ProgramFiles(x86)%`, and `%ProgramData%` instead of assuming `C:\`

### Removed
* **Orphaned Files** - Deleted a stale `launchSettings.json` containing outdated build configuration

---

## [1.7.2] - 2026-03-30

### Fixed
* **Bluetooth Breaking Without Selection** - Moved BthAvctpSvc and DeviceAssociationBrokerSvc from recommended to bluetooth-only services
* **Fast Startup Not Actually Disabled** - Added HiberbootEnabled = 0 to registry optimization
* **Webcam Access Blocked** - Changed webcam CapabilityAccessManager from Deny to Allow
* **DisableWindowsConsumerFeatures** - Fixed value from 0 to 1
* **DisableThirdPartySuggestions (HKLM)** - Fixed value from 2 to 1
* **Windows Update Tasks Not Disabled** - Disable Updates now also disables update scheduled tasks
* **Temp Directory Not Recreated** - Added directory recreation after cleanup deletion

### Changed
* **Startup Apps** - Synced CLI and GUI startup app lists
* **Update Task Revert** - Now config-driven from ScheduledTasksConfig.json instead of hardcoded
* **CLI Services Menu** - Removed service counts
* **Help Page** - Updated service lists, added sleep/lock warning to troubleshooting

### Removed
* **Bare "Microsoft" Wildcard** - Removed wildcard entry from AppsConfig.json
* **wuTaskPaths Wildcards** - Replaced with specific task names (wuTasks) in ScheduledTasksConfig.json

---

## [1.7.1] - 2026-01-21

### Changed
* **AppsConfig.json** - Updated package list

### Fixed
* **Fast Startup Disabling** - Disabled Fast Startup (HiberbootEnabled = 0) to fix hardware initialization on cold boot
* **Unstable Settings** - Removed unstable power latency settings in OptimizeLatency
* **Revert Methods** - Updated Revert methods with current fixes

### Removed
* **Wildcard PowerShell Commands** - Replaced with direct sc commands for reliability

---

## [1.7.0] - 2025-12-27

### Added
* **Help Page** - Implemented detailed HelpPage component with full documentation of what this project does and how to use the features
* **Scrollbar** - Scrollbar that will be used on help page
* **ConfigLoader Utility Class** - Centralized configuration loading
* **Log Rotation** - Automatic cleanup on startup
* **Service Default Startup Types** - `defaultStartup` field for config-driven reverts
* **New Telemetry Services** - Added whesvc, TrkWks, InventorySvc, CDPSvc, WpnService
* **Template-based Per-User Services** - Proper handling of services like CDPUserSvc, OneSyncSvc

### Changed
* **HTTP Status Codes** - Proper REST standards across all endpoints
* **Service Revert System** - Reads from ServicesConfig.json instead of hardcoded values
* **CLI Service Optimization** - Removed wildcard approach, using template service names
* **Recommended Services Count** - Now 108 services (was 82)

### Fixed
* **Per-User Service Disabling** - Template services now disable correctly for all users
* **Code Duplication** - ConfigLoader eliminates duplicate JSON handling
* **Json Duplication** - Removed duplications from ServicesConfig

### Removed
* **Wildcard PowerShell Commands** - Replaced with direct sc commands for reliability

---

## [1.6.0] - 2025-12-20

### Added
- **Comprehensive Logging System** - All operations now log to HST-WINDOWS-UTILITY.log in %TEMP%
  - Service operations tracking
  - Error diagnostics and debugging
  - Operation success/failure reporting
  - PowerShell script execution logging

### Changed
- **Port Change** - Backend now runs on port 5200 (was 5000)
- **UI Status Messages** - More descriptive real-time operation status
- **UI Text Labels** - Improved clarity and user guidance
- **Error Handling** - Enhanced error messages with log file references
- **CLI Menu** - Reorganized options for better workflow
- **CLI Output** - Improved readability and status reporting

### Fixed
- **CLI Lower Visuals Crash** - Fixed crashes during visual effects optimization
- Consistent async method naming conventions
- Comprehensive code comments and documentation

---

## [1.5.0] - 2025-12-10

### Added
- **Restore Features** - Complete reversal options for all modifications
  - Restore Registry to Windows defaults
  - Restore Services to default startup types
  - Restore Scheduled Tasks
  - Restore Windows Update services and permissions
  - Restore All - One-click restoration
- **Remove Startup Apps** - Disable common apps from running at startup (Discord, Steam, Spotify, etc.)
- **Full Optimization** - Automated sequence running all optimizations
- **System Restore Point Creation** - Silent restore point creation for safety

### Changed
- **Complete Code Refactor** - Improved code organization and maintainability
- **UI Layout** - Compact design with adjusted element sizes for better usability
- **Menu Structure** - Reorganized options into logical groups
- **Error Handling** - Enhanced error messages and admin privilege checks
- **GUI** - New app icon and visual polish
- **CLI** - Added restore options section
- **Services Management** - Better organization (Recommended/Bluetooth/Hyper-V/Xbox)
- **Cleanup Operations** - Consolidated into Full Cleanup option

### Fixed
- Registry tweaks not applying correctly on some systems
- Power plan import failing when HST.pow not in same directory
- Edge removal leaving registry artifacts
- OneDrive uninstallation not complete on some systems

---

## [1.0.0] - 2025-09-05

### Added
- **GUI Version** - Electron-based desktop application with React frontend
- **CLI Version** - Batch script with full feature parity
- **Registry Optimization** - 100+ performance-focused registry tweaks
  - Gaming optimizations (GPU/CPU priority, latency reduction)
  - Network optimizations (throttling removal)
  - Telemetry and bloatware disabling
  - Power latency optimizations
- **Visual Effects** - Disable Windows animations and effects
- **Dark Mode** - System-wide dark theme toggle
- **Task Scheduler Optimization** - Disable 50+ unnecessary scheduled tasks
- **Windows Update Control** - Complete disable/enable of Windows updates
- **Services Management** - Disable unnecessary Windows services
  - 99+ recommended services
  - Bluetooth services (4)
  - Hyper-V virtualization (11)
  - Xbox gaming services (4)
- **Custom Power Plan** - HST high-performance power plan
- **Debloat Features**
  - Remove Microsoft Apps (40+ bloatware apps)
  - Remove Xbox Apps
  - Remove Microsoft Store
  - Remove Microsoft Edge
  - Remove OneDrive
- **Cleanup Features**
  - Clean Temp Files
  - Clean Cache Files (browsers, Windows Update)
  - Clean Event Logs
  - Clean Default Power Plans
  - Clean Recycle Bin
  - Full Cleanup (all cleanup operations)
