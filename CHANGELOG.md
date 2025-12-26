## [1.7.0] - 2024-12-26

### Added
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

### Removed
* **Wildcard PowerShell Commands** - Replaced with direct sc commands for reliability

---

## [1.6.0] - 2024-12-20

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

## [1.5.0] - 2024-12-10

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

## [1.0.0] - 2024-09-05

### Added
- **GUI Version** - Electron-based desktop application with React frontend
- **CLI Version** - Batch script with full feature parity
- **Registry Optimization** - 500+ performance-focused registry tweaks
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
