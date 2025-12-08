@echo off
setlocal EnableDelayedExpansion
title HST WINDOWS UTILITY - SCRIPT VERSION v1.0.0
color 0B


>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (
    echo ============================================================
    echo          ERROR: ADMINISTRATOR PRIVILEGES REQUIRED
    echo                    RUN AS ADMINISTRATOR
    echo ============================================================
    echo.
    pause
    exit /b
)

set "nul=>NUL 2>&1"
:MAIN_MENU
cls
echo.


echo           _____                    _____                _____
echo          /\    \                  /\    \              /\    \
echo         /::\____\                /::\    \            /::\    \
echo        /:::/    /               /::::\    \           \:::\    \
echo       /:::/    /               /::::::\    \           \:::\    \
echo      /:::/    /               /:::/\:::\    \           \:::\    \
echo     /:::/____/               /:::/__\:::\    \           \:::\    \
echo    /::::\    \               \:::\   \:::\    \          /::::\    \
echo   /::::::\    \   _____    ___\:::\   \:::\    \        /::::::\    \
echo  /:::/\:::\    \ /\    \  /\   \:::\   \:::\    \      /:::/\:::\    \
echo /:::/  \:::\    /::\____\/::\   \:::\   \:::\____\    /:::/  \:::\____\
echo \::/    \:::\  /:::/    /\:::\   \:::\   \::/    /   /:::/    \::/    /
echo  \/____/ \:::\/:::/    /  \:::\   \:::\   \/____/   /:::/    / \/____/
echo           \::::::/    /    \:::\   \:::\    \      /:::/    /
echo            \::::/    /      \:::\   \:::\____\    /:::/    /
echo            /:::/    /        \:::\  /:::/    /    \::/    /
echo           /:::/    /          \:::\/:::/    /      \/____/
echo          /:::/    /            \::::::/    /
echo         /:::/    /              \::::/    /
echo         \::/    /                \::/    /
echo          \/____/                  \/____/
echo.
echo.
echo.
echo.
echo         HST WINDOWS UTILITY - SCRIPT VERSION v1.0.0
echo ============================================================
echo.
echo  [0] EXIT
echo.
echo  [1] CREATE RESTORE POINT            DEBLOAT
echo  [2] FULL OPTIMIZATION               [10] REMOVE MS APPS
echo                                      [11] REMOVE XBOX APPS
echo  REGISTRY OPTIMIZATIONS              [12] REMOVE STORE APPS
echo  [3] OPTIMIZE REGISTRY               [13] REMOVE EDGE
echo  [4] LOWER VISUAL EFFECTS            [14] REMOVE ONEDRIVE
echo  [5] ENABLE DARK MODE
echo                                      CLEANUP
echo  SYSTEM MANAGEMENT                   [15] CLEAN TEMP FILES
echo  [6] OPTIMIZE SCHEDULER              [16] CLEAN CACHE FILES
echo  [7] DISABLE WINDOWS UPDATES         [17] CLEAN EVENT LOGS
echo  [8] OPTIMIZE SERVICES               [18] CLEAN POWER PLANS
echo  [9] ADD CUSTOM POWERPLAN            [19] CLEAN RECYCLE BIN
echo                                      [20] FULL CLEANUP
echo.
echo ============================================================
set /p choice="YOUR CHOICE: "

if "%choice%"=="0" goto EXIT
if "%choice%"=="1" goto CREATE_RESTORE_POINT
if "%choice%"=="2" goto FULL_OPTIMIZATION
if "%choice%"=="3" goto OPTIMIZE_REGISTRY
if "%choice%"=="4" goto LOWER_VISUALS
if "%choice%"=="5" goto DARK_MODE
if "%choice%"=="6" goto OPTIMIZE_TASKSCHEDULER
if "%choice%"=="7" goto DISABLE_UPDATES
if "%choice%"=="8" goto OPTIMIZE_SERVICES_MENU
if "%choice%"=="9" goto ADD_POWER_PLAN
if "%choice%"=="10" goto REMOVE_MS_APPS
if "%choice%"=="11" goto REMOVE_XBOX_APPS
if "%choice%"=="12" goto REMOVE_STORE_APPS
if "%choice%"=="13" goto REMOVE_EDGE
if "%choice%"=="14" goto REMOVE_ONEDRIVE
if "%choice%"=="15" goto CLEAN_TEMP
if "%choice%"=="16" goto CLEAN_CACHE
if "%choice%"=="17" goto CLEAN_EVENTLOGS
if "%choice%"=="18" goto CLEAN_POWERPLANS
if "%choice%"=="19" goto CLEAN_RECYCLE
if "%choice%"=="20" goto FULL_CLEANUP

echo INVALID CHOICE
pause
goto MAIN_MENU


:CREATE_RESTORE_POINT
cls
echo ============================================================
echo               CREATING SYSTEM RESTORE POINT
echo ============================================================
echo.

reg add "HKLM\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v "SystemRestorePointCreationFrequency" /t REG_DWORD /d 0 /f %nul%
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Enable-ComputerRestore -Drive $env:SystemDrive; Checkpoint-Computer -Description 'HST-WINDOWS-OPTIMIZER' -RestorePointType 'MODIFY_SETTINGS'"

if %errorLevel% equ 0 (
    echo.
    echo ============================================================
    echo                           DONE
    echo ============================================================
    echo.
) else (
    echo.
    echo WARNING: ERROR CREATING RESTORE POINT
)
echo.
pause
goto MAIN_MENU


:CREATE_RESTORE_POINT_SILENT
reg add "HKLM\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v "SystemRestorePointCreationFrequency" /t REG_DWORD /d 0 /f %nul%
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Enable-ComputerRestore -Drive $env:SystemDrive; Checkpoint-Computer -Description 'HST-WINDOWS-OPTIMIZER' -RestorePointType 'MODIFY_SETTINGS'" %nul%
exit /b


:FULL_OPTIMIZATION
cls
echo ============================================================
echo                RUNNING FULL OPTIMIZATION
echo ============================================================
echo.

echo [1/7] CREATING RESTORE POINT
call :CREATE_RESTORE_POINT_SILENT

echo [2/7] OPTIMIZING REGISTRY
call :OPTIMIZE_REGISTRY_SILENT
call :LOWER_VISUALS_SILENT

echo [3/7] OPTIMIZING TASK SCHEDULER
call :OPTIMIZE_TASKSCHEDULER_SILENT

echo [4/7] DISABLING WINDOWS UPDATES
call :DISABLE_UPDATES_SILENT

echo [5/7] OPTIMIZING SERVICES
call :DISABLE_SERVICES_ALL

echo [6/7] ADDING AND ACTIVATING CUSTOM POWER PLAN
call :ADD_POWER_PLAN_SILENT

echo [7/7] RUNNING FULL CLEANUP
call :FULL_CLEANUP_SILENT

echo.
echo ============================================================
echo                   OPTIMIZATION COMPLETE
echo                 RESTART YOUR COMPUTER NOW
echo ============================================================
echo.
pause
goto MAIN_MENU


:OPTIMIZE_REGISTRY
cls
echo ============================================================
echo                    OPTIMIZING REGISTRY
echo ============================================================
echo.
call :OPTIMIZE_REGISTRY_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:OPTIMIZE_REGISTRY_SILENT
echo   - PERFORMANCE SETTINGS
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d 38 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Power" /v "HiberbootEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Power" /v "SleepStudyDisabled" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "HibernateEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance" /v "MaintenanceDisabled" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v "PowerThrottlingOff" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d 1 /f %nul%

echo   - GAME MODE AND DVR
reg add "HKCU\Software\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\GameBar" /v "AutoGameModeEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\GameBar" /v "AllowGameBarControllerButton" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\GameBar" /v "UseNexusForGameBarEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_FSEBehaviorMode" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_FSEBehavior" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_HonorUserFSEBehaviorMode" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_DXGIHonorFSEWindowsCompatible" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_EFSEFeatureFlags" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_DSEBehavior" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\System\GameConfigStore" /v "GameDVR_DXGI_AGILITY_FACTOR" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR" /v "AllowGameDVR" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR" /v "AppCaptureEnabled" /t REG_DWORD /d 0 /f %nul%

echo   - MENU DELAY
reg add "HKCU\Control Panel\Desktop" /v "MenuShowDelay" /t REG_DWORD /d 0 /f %nul%

echo   - ACCESSIBILITY SHORTCUTS
reg add "HKCU\Control Panel\Accessibility\HighContrast" /v "Flags" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Accessibility\ToggleKeys" /v "Flags" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Accessibility\StickyKeys" /v "Flags" /t REG_SZ /d "0" /f %nul%

echo   - NETWORK AND LATENCY
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "NoLazyMode" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "AlwaysOn" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "NetworkThrottlingIndex" /t REG_DWORD /d 4294967295 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "SystemResponsiveness" /t REG_DWORD /d 10 /f %nul%

echo   - GAMING TASK PRIORITY
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Affinity" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Background Only" /t REG_SZ /d "False" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "GPU Priority" /t REG_DWORD /d 8 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Priority" /t REG_DWORD /d 6 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "SFIO Priority" /t REG_SZ /d "High" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Latency Sensitive" /t REG_SZ /d "True" /f %nul%

echo   - POWER LATENCY
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "ExitLatency" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "ExitLatencyCheckEnabled" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "Latency" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "LatencyToleranceDefault" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "LatencyToleranceFSVP" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "LatencyTolerancePerfOverride" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "LatencyToleranceScreenOffIR" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "RtlCapabilityCheckLatency" /t REG_DWORD /d 1 /f %nul%

echo   - GPU LATENCY
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultD3TransitionLatencyActivelyUsed" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultD3TransitionLatencyIdleLongTime" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultD3TransitionLatencyIdleMonitorOff" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultD3TransitionLatencyIdleNoContext" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultD3TransitionLatencyIdleShortTime" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultD3TransitionLatencyIdleVeryLongTime" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceIdle0" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceIdle0MonitorOff" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceIdle1" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceIdle1MonitorOff" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceMemory" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceNoContext" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceNoContextMonitorOff" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceOther" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultLatencyToleranceTimerPeriod" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultMemoryRefreshLatencyToleranceActivelyUsed" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultMemoryRefreshLatencyToleranceMonitorOff" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "DefaultMemoryRefreshLatencyToleranceNoContext" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "Latency" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "MaxIAverageGraphicsLatencyInOneBucket" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "MiracastPerfTrackGraphicsLatency" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "MonitorLatencyTolerance" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "MonitorRefreshLatencyTolerance" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v "TransitionLatency" /t REG_DWORD /d 1 /f %nul%

echo   - AMD GPU ULPS
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000" /v "EnableUlps" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000" /v "EnableUlps_NA" /t REG_SZ /d "0" /f %nul%

echo   - NTFS OPTIMIZATIONS
reg add "HKLM\SYSTEM\CurrentControlSet\Control\FileSystem" /v "NtfsDisableLastAccessUpdate" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\FileSystem" /v "NtfsDisable8dot3NameCreation" /t REG_DWORD /d 1 /f %nul%

echo   - DISTRIBUTE TIMERS
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DistributeTimers" /t REG_DWORD /d 1 /f %nul%

echo   - BACKGROUND PROCESS PRIORITY
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\wuauclt.exe\PerfOptions" /v "CpuPriorityClass" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\wuauclt.exe\PerfOptions" /v "IoPriority" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\SearchIndexer.exe\PerfOptions" /v "CpuPriorityClass" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\SearchIndexer.exe\PerfOptions" /v "IoPriority" /t REG_DWORD /d 0 /f %nul%

echo   - KEYBOARD AND MOUSE
reg add "HKCU\Control Panel\Keyboard" /v "KeyboardDelay" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Keyboard" /v "KeyboardSpeed" /t REG_SZ /d "31" /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v "MouseDataQueueSize" /t REG_DWORD /d 16 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v "KeyboardDataQueueSize" /t REG_DWORD /d 16 /f %nul%
reg add "HKCU\Control Panel\Mouse" /v "MouseSpeed" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Mouse" /v "MouseThreshold1" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Mouse" /v "MouseThreshold2" /t REG_SZ /d "0" /f %nul%

echo   - CONTENT DELIVERY MANAGER
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-338393Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-338388Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-314559Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-280815Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-202914Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-353694Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-353696Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-338387Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-353698Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-338389Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-310093Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContent-314563Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "RotatingLockScreenOverlayEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "RotatingLockScreenEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "ContentDeliveryAllowed" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "OemPreInstalledAppsEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "PreInstalledAppsEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "PreInstalledAppsEverEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SilentInstalledAppsEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SoftLandingEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SubscribedContentEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "FeatureManagementEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "SystemPaneSuggestionsEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager" /v "RemediationRequired" /t REG_DWORD /d 0 /f %nul%

echo   - PRIVACY SETTINGS
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy" /v "TailoredExperiencesWithDiagnosticDataEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds" /v "EnableFeeds" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Explorer" /v "ShowOrHideMostUsedApps" /t REG_DWORD /d 2 /f %nul%

echo   - WINDOWS SPOTLIGHT
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "ConfigureWindowsSpotlight" /t REG_DWORD /d 2 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "IncludeEnterpriseSpotlight" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableWindowsSpotlightFeatures" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableWindowsSpotlightWindowsWelcomeExperience" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableWindowsSpotlightOnActionCenter" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableWindowsSpotlightOnSettings" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableThirdPartySuggestions" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableTailoredExperiencesWithDiagnosticData" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableThirdPartySuggestions" /t REG_DWORD /d 2 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableWindowsConsumerFeatures" /t REG_DWORD /d 0 /f %nul%

echo   - MISC SETTINGS
reg add "HKLM\SOFTWARE\Policies\Microsoft\Dsh" /v "AllowNewsAndInterests" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation" /v "DisableStartupSound" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Remote Assistance" /v "fAllowToGetHelp" /t REG_DWORD /d 0 /f %nul%

echo   - FILE EXPLORER
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer" /v "ShowFrequent" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer" /v "ShowRecent" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer" /v "TelemetrySalt" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer" /v "NoRecentDocsHistory" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v "NoRecentDocsHistory" /t REG_DWORD /d 1 /f %nul%

echo   - MAPS AND DRIVERS
reg add "HKLM\SYSTEM\Maps" /v "AutoUpdateEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching" /v "SearchOrderConfig" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching" /v "DontSearchWindowsUpdate" /t REG_DWORD /d 1 /f %nul%

echo   - TELEMETRY
reg add "HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowDeviceNameInTelemetry" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowCommercialDataPipeline" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "LimitEnhancedDiagnosticDataWindowsAnalytics" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "DoNotShowFeedbackNotifications" /t REG_DWORD /d 1 /f %nul%

echo   - SEARCH SETTINGS
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search" /v "HistoryViewEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search" /v "DeviceHistoryEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search" /v "BingSearchEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search" /v "SearchboxTaskbarMode" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\SearchSettings" /v "IsDynamicSearchBoxEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\SearchSettings" /v "IsDeviceSearchHistoryEnabled" /t REG_DWORD /d 0 /f %nul%

echo   - NOTIFICATIONS
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\PushNotifications" /v "ToastEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Policies\Microsoft\Windows\Explorer" /v "DisableNotificationCenter" /t REG_DWORD /d 1 /f %nul%

echo   - BACKGROUND APPS
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications" /v "GlobalUserDisabled" /t REG_DWORD /d 1 /f %nul%

echo   - AUTO-ENDTASK (WIN11)
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings" /v "TaskbarEndTask" /t REG_DWORD /d 1 /f %nul%

echo   - PERSONALIZATION PRIVACY
reg add "HKCU\Software\Microsoft\Personalization\Settings" /v "AcceptedPrivacyPolicy" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Personalization\Settings" /v "RestrictImplicitInkCollection" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\Software\Microsoft\Personalization\Settings" /v "RestrictImplicitTextCollection" /t REG_DWORD /d 1 /f %nul%

echo   - DIAGNOSTICS AND ACTIVITY
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack\EventTranscriptKey" /v "EnableEventTranscript" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Siuf\Rules" /v "NumberOfSIUFInPeriod" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v "PublishUserActivities" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v "UploadUserActivities" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v "EnableActivityFeed" /t REG_DWORD /d 0 /f %nul%

echo   - MISC PRIVACY
reg add "HKCU\Control Panel\Accessibility" /v "DynamicScrollbars" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\International\User Profile" /v "HttpAcceptLanguageOptOut" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy" /v "Start_TrackProgs" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\SystemSettings\AccountNotifications" /v "EnableAccountNotifications" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy" /v "AppSuggestions" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard" /v "Disabled" /t REG_DWORD /d 1 /f %nul%

echo   - CAPABILITY ACCESS MANAGER
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appointments" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\broadFileSystemAccess" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\bluetoothSync" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\chat" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\contacts" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\documentsLibrary" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\downloadsFolder" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\email" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\graphicsCaptureProgrammatic" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\graphicsCaptureWithoutBorder" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone" /v "Value" /t REG_SZ /d "Allow" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\musicLibrary" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\phoneCall" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\phoneCallHistory" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\picturesLibrary" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\radios" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userDataTasks" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener" /v "Value" /t REG_SZ /d "Deny" /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\videosLibrary" /v "Value" /t REG_SZ /d "Deny" /f %nul%

echo   - SPEECH AND VOICE
reg add "HKCU\SOFTWARE\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy" /v "HasAccepted" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps" /v "AgentActivationEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps" /v "AgentActivationLastUsed" /t REG_DWORD /d 0 /f %nul%

echo   - SYNC SETTINGS
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync" /v "SyncPolicy" /t REG_DWORD /d 5 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Accessibility" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\AppSync" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Personalization" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\BrowserSettings" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Credentials" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Language" /v "Enabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Windows" /v "Enabled" /t REG_DWORD /d 0 /f %nul%

echo   - ERROR REPORTING AND STORAGE
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting" /v "Disabled" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting" /v "DoReport" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageSense" /v "AllowStorageSenseGlobal" /t REG_DWORD /d 0 /f %nul%

echo   - BROWSER OPTIMIZATION
reg add "HKLM\SOFTWARE\Policies\Microsoft\Edge" /v "StartupBoostEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Edge" /v "HardwareAccelerationModeEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Edge" /v "BackgroundModeEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\MicrosoftEdgeElevationService" /v "Start" /t REG_DWORD /d 4 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\edgeupdate" /v "Start" /t REG_DWORD /d 4 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\edgeupdatem" /v "Start" /t REG_DWORD /d 4 /f %nul%

echo   - BROWSER OPTIMIZATION
reg add "HKLM\SOFTWARE\Policies\Google\Chrome" /v "StartupBoostEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Google\Chrome" /v "HardwareAccelerationModeEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Google\Chrome" /v "BackgroundModeEnabled" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\GoogleChromeElevationService" /v "Start" /t REG_DWORD /d 4 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\gupdate" /v "Start" /t REG_DWORD /d 4 /f %nul%
reg add "HKLM\SYSTEM\CurrentControlSet\Services\gupdatem" /v "Start" /t REG_DWORD /d 4 /f %nul%

echo   - STORE AND TASKBAR
reg add "HKLM\SOFTWARE\Policies\Microsoft\WindowsStore" /v "AutoDownload" /t REG_DWORD /d 2 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v "TaskbarMn" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v "ShowTaskViewButton" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v "HideSCAMeetNow" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings" /v "ShowLockOption" /t REG_DWORD /d 0 /f %nul%
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings" /v "ShowSleepOption" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v "EnableTransparency" /t REG_DWORD /d 0 /f %nul%

echo   - SOUND SCHEME
reg add "HKCU\AppEvents\Schemes" /v "" /t REG_SZ /d ".None" /f %nul%
exit /b


:LOWER_VISUALS
cls
echo ============================================================
echo                  LOWERING VISUAL EFFECTS
echo ============================================================
echo.
call :LOWER_VISUALS_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:LOWER_VISUALS_SILENT
echo   - VISUAL EFFECTS
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v "VisualFXSetting" /t REG_DWORD /d 3 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "UserPreferencesMask" /t REG_BINARY /d 9012038010000000 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "MinAnimate" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "DragFullWindows" /t REG_SZ /d "0" /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "FontSmoothing" /t REG_SZ /d "2" /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "WindowAnimation" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "MenuAnimation" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "TaskbarAnimations" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "IconAnimation" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "ScrollAnimation" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\Desktop" /v "ScrollSmoothness" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v "TaskbarAnimations" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v "IconsOnly" /t REG_DWORD /d 1 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v "ListviewAlphaSelect" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v "ListviewShadow" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\DWM" /v "EnableAeroPeek" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Software\Microsoft\Windows\DWM" /v "AlwaysHibernateThumbnails" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\Control Panel\Desktop\WindowMetrics" /v "MinAnimate" /t REG_SZ /d "0" /f %nul%
exit /b


:DARK_MODE
cls
echo ============================================================
echo                    ENABLING DARK MODE
echo ============================================================
echo.
echo   - SETTING DARK MODE
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v "AppsUseLightTheme" /t REG_DWORD /d 0 /f %nul%
reg add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v "SystemUsesLightTheme" /t REG_DWORD /d 0 /f %nul%
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:OPTIMIZE_TASKSCHEDULER
cls
echo ============================================================
echo                 OPTIMIZING TASK SCHEDULER
echo ============================================================
echo.
call :OPTIMIZE_TASKSCHEDULER_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:OPTIMIZE_TASKSCHEDULER_SILENT
echo   - DISABLING GOOGLE TASKS
schtasks /Change /TN "\GoogleSystem\GoogleUpdater" /Disable %nul%
schtasks /Change /TN "\GoogleSystem\GoogleUpdaterInternalService" /Disable %nul%

echo   - DISABLING APPLICATION DATA TASKS
schtasks /Change /TN "\Microsoft\Windows\ApplicationData\appuriverifierdaily" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\ApplicationData\appuriverifierinstall" /Disable %nul%

echo   - DISABLING APPLICATION EXPERIENCE TASKS
schtasks /Change /TN "\Microsoft\Windows\Application Experience\PcaPatchDbTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Application Experience\ProgramDataUpdater" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Application Experience\StartupAppTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Application Experience\MareBackup" /Disable %nul%

echo   - DISABLING BACKUP AND SYNC TASKS
schtasks /Change /TN "\Microsoft\Windows\AppListBackup\Backup" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Autochk\Proxy" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Bluetooth\UninstallDeviceTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\CloudExperienceHost\CreateObjectTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\CloudRestore\Backup" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\CloudRestore\Restore" /Disable %nul%

echo   - DISABLING CEIP TASKS
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\Consolidator" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\UsbCeip" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\BthSQM" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\KernelCeipTask" /Disable %nul%

echo   - DISABLING DEFRAG AND DIAGNOSTICS
schtasks /Change /TN "\Microsoft\Windows\Defrag\ScheduledDefrag" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Diagnosis\RecommendedTroubleshootingScanner" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Diagnosis\Scheduled" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\DiskCleanup\SilentCleanup" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticResolver" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\DiskFootprint\Diagnostics" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\DiskFootprint\StorageSense" /Disable %nul%

echo   - DISABLING FILE HISTORY
schtasks /Change /TN "\Microsoft\Windows\FileHistory\File History (maintenance mode)" /Disable %nul%

echo   - DISABLING MISC TASKS
schtasks /Change /TN "\Microsoft\Windows\International\Synchronize Language Settings" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Maintenance\WinSAT" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Maps\MapsToastTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Maps\MapsUpdateTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\MemoryDiagnostic\RunFullMemoryDiagnostic" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\MemoryDiagnostic\ProcessMemoryDiagnosticEvents" /Disable %nul%

echo   - DISABLING OFFLINE FILES
schtasks /Change /TN "\Microsoft\Windows\Offline Files\Background Synchronization" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Offline Files\Logon Synchronization" /Disable %nul%

echo   - DISABLING POWER AND PRINTING
schtasks /Change /TN "\Microsoft\Windows\Power Efficiency Diagnostics\AnalyzeSystem" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Printing\EduPrintProv" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Printing\PrinterCleanupTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Printing\PrintJobCleanupTask" /Disable %nul%

echo   - DISABLING RETAIL AND REGISTRY
schtasks /Change /TN "\Microsoft\Windows\RetailDemo\CleanupOfflineContent" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Registry\RegIdleBackup" /Disable %nul%

echo   - DISABLING FAMILY SAFETY
schtasks /Change /TN "\Microsoft\Windows\Shell\FamilySafetyMonitor" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Shell\FamilySafetyRefreshTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Shell\FamilySafetyUpload" /Disable %nul%

echo   - DISABLING SPEECH AND TIME
schtasks /Change /TN "\Microsoft\Windows\Speech\SpeechModelDownloadTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Time Synchronization\ForceSynchronizeTime" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Time Synchronization\SynchronizeTime" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Time Zone\SynchronizeTimeZone" /Disable %nul%

echo   - DISABLING WINDOWS DEFENDER
schtasks /Change /TN "\Microsoft\Windows\Windows Defender\Windows Defender Cache Maintenance" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Windows Defender\Windows Defender Cleanup" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Windows Defender\Windows Defender Scheduled Scan" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Windows Defender\Windows Defender Verification" /Disable %nul%

echo   - DISABLING ERROR REPORTING AND UPDATES
schtasks /Change /TN "\Microsoft\Windows\Windows Error Reporting\QueueReporting" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Windows Media Sharing\UpdateLibrary" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\WindowsUpdate\Scheduled Start" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Wininet\CacheTask" /Disable %nul%

echo   - DISABLING WORK FOLDERS
schtasks /Change /TN "\Microsoft\Windows\Work Folders\Work Folders Logon Synchronization" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Work Folders\Work Folders Maintenance Work" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\Workplace Join\Automatic-Device-Join" /Disable %nul%

echo   - DISABLING WS TASKS
schtasks /Change /TN "\Microsoft\Windows\WS\WSRefreshBannedAppsListTask" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\WS\WSTask" /Disable %nul%

echo   - DISABLING UPDATE ORCHESTRATOR
schtasks /Change /TN "\Microsoft\Windows\UpdateOrchestrator\Schedule Scan" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\UpdateOrchestrator\Schedule Maintenance Work" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\UpdateOrchestrator\UpdateAssistant" /Disable %nul%
schtasks /Change /TN "\Microsoft\Windows\WaaSMedic\PerformRemediation" /Disable %nul%

exit /b


:DISABLE_UPDATES
cls
echo ============================================================
echo                 DISABLING WINDOWS UPDATES
echo ============================================================
echo.
call :DISABLE_UPDATES_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:DISABLE_UPDATES_SILENT
echo   - STOPPING UPDATE SERVICES
net stop wuauserv %nul%
net stop UsoSvc %nul%
net stop WaaSMedicSvc %nul%
net stop BITS %nul%

echo   - DISABLING UPDATE SERVICES
sc config wuauserv start= disabled %nul%
sc config UsoSvc start= disabled %nul%
sc config WaaSMedicSvc start= disabled %nul%
sc config BITS start= disabled %nul%

echo   - SETTING SERVICE PERMISSIONS
sc sdset wuauserv D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA) %nul%
sc sdset UsoSvc D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA) %nul%
sc sdset WaaSMedicSvc D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA) %nul%
sc sdset BITS D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA) %nul%

echo   - SETTING UPDATE POLICIES
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU" /v "NoAutoUpdate" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU" /v "AUOptions" /t REG_DWORD /d 1 /f %nul%
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config" /v "DODownloadMode" /t REG_DWORD /d 0 /f %nul%
exit /b


:OPTIMIZE_SERVICES_MENU
cls
echo ============================================================
echo                    OPTIMIZE SERVICES
echo ============================================================
echo.
echo SELECT SERVICES TO DISABLE
echo  [1] RECOMMENDED (80 SERVICES)
echo  [2] BLUETOOTH (4 SERVICES)
echo  [3] HYPER-V (11 SERVICES)
echo  [4] XBOX (4 SERVICES)
echo  [5] ALL OF THE ABOVE (99 SERVICES)
echo  [0] BACK TO MENU
echo.
set /p svc_choice="YOUR CHOICE -> "

if "%svc_choice%"=="0" goto MAIN_MENU
if "%svc_choice%"=="1" call :DISABLE_SERVICES_ALL_RECOMMENDED
if "%svc_choice%"=="2" call :DISABLE_BLUETOOTH_SERVICES
if "%svc_choice%"=="3" call :DISABLE_HYPERV_SERVICES
if "%svc_choice%"=="4" call :DISABLE_XBOX_SERVICES
if "%svc_choice%"=="5" call :DISABLE_SERVICES_ALL

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:DISABLE_SERVICES_ALL
call :DISABLE_SERVICES_ALL_RECOMMENDED
call :DISABLE_BLUETOOTH_SERVICES
call :DISABLE_HYPERV_SERVICES
call :DISABLE_XBOX_SERVICES
exit /b


:DISABLE_SERVICES_ALL_RECOMMENDED
echo   - DISABLING SERVICES (BATCH 1/8)
sc stop tzautoupdate %nul% & sc config tzautoupdate start= disabled %nul%
sc stop BthAvctpSvc %nul% & sc config BthAvctpSvc start= disabled %nul%
sc stop BDESVC %nul% & sc config BDESVC start= disabled %nul%
sc stop wbengine %nul% & sc config wbengine start= disabled %nul%
sc stop autotimesvc %nul% & sc config autotimesvc start= disabled %nul%
sc stop ClipSVC %nul% & sc config ClipSVC start= disabled %nul%
sc stop DiagTrack %nul% & sc config DiagTrack start= disabled %nul%
sc stop DsSvc %nul% & sc config DsSvc start= disabled %nul%
sc stop DoSvc %nul% & sc config DoSvc start= disabled %nul%
sc stop DmEnrollmentSvc %nul% & sc config DmEnrollmentSvc start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 2/8)
sc stop dmwappushservice %nul% & sc config dmwappushservice start= disabled %nul%
sc stop diagsvc %nul% & sc config diagsvc start= disabled %nul%
sc stop DPS %nul% & sc config DPS start= disabled %nul%
sc stop DialogBlockingService %nul% & sc config DialogBlockingService start= disabled %nul%
sc stop DisplayEnhancementService %nul% & sc config DisplayEnhancementService start= disabled %nul%
sc stop fhsvc %nul% & sc config fhsvc start= disabled %nul%
sc stop lfsvc %nul% & sc config lfsvc start= disabled %nul%
sc stop iphlpsvc %nul% & sc config iphlpsvc start= disabled %nul%
sc stop MapsBroker %nul% & sc config MapsBroker start= disabled %nul%
sc stop MicrosoftEdgeElevationService %nul% & sc config MicrosoftEdgeElevationService start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 3/8)
sc stop edgeupdate %nul% & sc config edgeupdate start= disabled %nul%
sc stop edgeupdatem %nul% & sc config edgeupdatem start= disabled %nul%
sc stop MsKeyboardFilter %nul% & sc config MsKeyboardFilter start= disabled %nul%
sc stop NgcSvc %nul% & sc config NgcSvc start= disabled %nul%
sc stop NgcCtnrSvc %nul% & sc config NgcCtnrSvc start= disabled %nul%
sc stop InstallService %nul% & sc config InstallService start= disabled %nul%
sc stop uhssvc %nul% & sc config uhssvc start= disabled %nul%
sc stop SmsRouter %nul% & sc config SmsRouter start= disabled %nul%
sc stop NetTcpPortSharing %nul% & sc config NetTcpPortSharing start= disabled %nul%
sc stop Netlogon %nul% & sc config Netlogon start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 4/8)
sc stop NcbService %nul% & sc config NcbService start= disabled %nul%
sc stop CscService %nul% & sc config CscService start= disabled %nul%
sc stop defragsvc %nul% & sc config defragsvc start= disabled %nul%
sc stop WpcMonSvc %nul% & sc config WpcMonSvc start= disabled %nul%
sc stop SEMgrSvc %nul% & sc config SEMgrSvc start= disabled %nul%
sc stop PhoneSvc %nul% & sc config PhoneSvc start= disabled %nul%
sc stop Spooler %nul% & sc config Spooler start= disabled %nul%
sc stop PrintDeviceConfigurationService %nul% & sc config PrintDeviceConfigurationService start= disabled %nul%
sc stop PrintNotify %nul% & sc config PrintNotify start= disabled %nul%
sc stop wercplsupport %nul% & sc config wercplsupport start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 5/8)
sc stop PcaSvc %nul% & sc config PcaSvc start= disabled %nul%
sc stop QWAVE %nul% & sc config QWAVE start= disabled %nul%
sc stop RmSvc %nul% & sc config RmSvc start= disabled %nul%
sc stop TroubleshootingSvc %nul% & sc config TroubleshootingSvc start= disabled %nul%
sc stop RasAuto %nul% & sc config RasAuto start= disabled %nul%
sc stop RasMan %nul% & sc config RasMan start= disabled %nul%
sc stop SessionEnv %nul% & sc config SessionEnv start= disabled %nul%
sc stop UmRdpService %nul% & sc config UmRdpService start= disabled %nul%
sc stop RemoteRegistry %nul% & sc config RemoteRegistry start= disabled %nul%
sc stop RemoteAccess %nul% & sc config RemoteAccess start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 6/8)
sc stop RetailDemo %nul% & sc config RetailDemo start= disabled %nul%
sc stop SensorDataService %nul% & sc config SensorDataService start= disabled %nul%
sc stop SensrSvc %nul% & sc config SensrSvc start= disabled %nul%
sc stop SensorService %nul% & sc config SensorService start= disabled %nul%
sc stop LanmanServer %nul% & sc config LanmanServer start= disabled %nul%
sc stop shpamsvc %nul% & sc config shpamsvc start= disabled %nul%
sc stop SCardSvr %nul% & sc config SCardSvr start= disabled %nul%
sc stop ScDeviceEnum %nul% & sc config ScDeviceEnum start= disabled %nul%
sc stop SCPolicySvc %nul% & sc config SCPolicySvc start= disabled %nul%
sc stop SysMain %nul% & sc config SysMain start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 7/8)
sc stop TabletInputService %nul% & sc config TabletInputService start= disabled %nul%
sc stop TapiSrv %nul% & sc config TapiSrv start= disabled %nul%
sc stop UevAgentService %nul% & sc config UevAgentService start= disabled %nul%
sc stop SDRSVC %nul% & sc config SDRSVC start= disabled %nul%
sc stop FrameServer %nul% & sc config FrameServer start= disabled %nul%
sc stop wcncsvc %nul% & sc config wcncsvc start= disabled %nul%
sc stop Wecsvc %nul% & sc config Wecsvc start= disabled %nul%
sc stop wisvc %nul% & sc config wisvc start= disabled %nul%
sc stop MixedRealityOpenXRSvc %nul% & sc config MixedRealityOpenXRSvc start= disabled %nul%
sc stop icssvc %nul% & sc config icssvc start= disabled %nul%


echo   - DISABLING SERVICES (BATCH 8/8)
sc stop spectrum %nul% & sc config spectrum start= disabled %nul%
sc stop perceptionsimulation %nul% & sc config perceptionsimulation start= disabled %nul%
sc stop PushToInstall %nul% & sc config PushToInstall start= disabled %nul%
sc stop W32Time %nul% & sc config W32Time start= disabled %nul%
sc stop WFDSConMgrSvc %nul% & sc config WFDSConMgrSvc start= disabled %nul%
sc stop WSearch %nul% & sc config WSearch start= disabled %nul%
sc stop LanmanWorkstation %nul% & sc config LanmanWorkstation start= disabled %nul%
sc stop AppVClient %nul% & sc config AppVClient start= disabled %nul%
sc stop cloudidsvc %nul% & sc config cloudidsvc start= disabled %nul%
sc stop diagnosticshub.standardcollector.service %nul% & sc config diagnosticshub.standardcollector.service start= disabled %nul%
sc stop WbioSrvc %nul% & sc config WbioSrvc start= disabled %nul%
sc stop WdiSystemHost %nul% & sc config WdiSystemHost start= disabled %nul%
sc stop WdiServiceHost %nul% & sc config WdiServiceHost start= disabled %nul%
sc stop wlidsvc %nul% & sc config wlidsvc start= disabled %nul%
sc stop WerSvc %nul% & sc config WerSvc start= disabled %nul%
sc stop workfolderssvc %nul% & sc config workfolderssvc start= disabled %nul%
exit /b


:DISABLE_BLUETOOTH_SERVICES
echo   - DISABLING BLUETOOTH SERVICES
sc stop BTAGService %nul% & sc config BTAGService start= disabled %nul%
sc stop bthserv %nul% & sc config bthserv start= disabled %nul%
exit /b


:DISABLE_HYPERV_SERVICES
echo   - DISABLING HYPER-V SERVICES
sc stop HvHost %nul% & sc config HvHost start= disabled %nul%
sc stop vmickvpexchange %nul% & sc config vmickvpexchange start= disabled %nul%
sc stop vmicguestinterface %nul% & sc config vmicguestinterface start= disabled %nul%
sc stop vmicshutdown %nul% & sc config vmicshutdown start= disabled %nul%
sc stop vmicheartbeat %nul% & sc config vmicheartbeat start= disabled %nul%
sc stop vmcompute %nul% & sc config vmcompute start= disabled %nul%
sc stop vmicvmsession %nul% & sc config vmicvmsession start= disabled %nul%
sc stop vmicrdv %nul% & sc config vmicrdv start= disabled %nul%
sc stop vmictimesync %nul% & sc config vmictimesync start= disabled %nul%
sc stop vmms %nul% & sc config vmms start= disabled %nul%
sc stop vmicvss %nul% & sc config vmicvss start= disabled %nul%
exit /b


:DISABLE_XBOX_SERVICES
echo   - DISABLING XBOX SERVICES
sc stop XboxGipSvc %nul% & sc config XboxGipSvc start= disabled %nul%
sc stop XblAuthManager %nul% & sc config XblAuthManager start= disabled %nul%
sc stop XblGameSave %nul% & sc config XblGameSave start= disabled %nul%
sc stop XboxNetApiSvc %nul% & sc config XboxNetApiSvc start= disabled %nul%
exit /b


:ADD_POWER_PLAN_SILENT
if not exist "%~dp0HST.pow" (
    echo    - ERROR: HST.POW FILE NOT FOUND
    exit /b 1
)

echo    - IMPORTING AND ACTIVATING POWER PLAN...
set "HST_GUID=f0f0f0f0-a1a1-b2b2-c3c3-123456789abc"
powercfg -import "%~dp0HST.pow" %HST_GUID% %nul%
powercfg -setactive %HST_GUID% %nul%
exit /b 0


:ADD_POWER_PLAN
cls
echo ============================================================
echo                 ADDING CUSTOM POWER PLAN
echo ============================================================
echo.

call :ADD_POWER_PLAN_SILENT

if %errorLevel% equ 1 (
    echo.
    echo ============================================================
    echo            ERROR: POWER PLAN FILE NOT FOUND
    echo ============================================================
) else (
    echo.
    echo ============================================================
    echo                            DONE
    echo ============================================================
)

echo.
pause
goto MAIN_MENU


:REMOVE_MS_APPS
cls
echo ============================================================
echo                 REMOVING MICROSOFT APPS
echo ============================================================
echo.
pause

echo   - REMOVING APPS (BATCH 1/6)
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Clipchamp.Clipchamp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Disney' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'LinkedInforWindows' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'LinkedIn' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.BingNews' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.BingSearch' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.BingWeather' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Copilot' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING APPS (BATCH 2/6)
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Edge.GameAssist' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.GamingApp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.GetHelp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Getstarted' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.MSPaint' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Microsoft3DViewer' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.MicrosoftOfficeHub' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.MicrosoftSolitaireCollection' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING APPS (BATCH 3/6)
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.MicrosoftStickyNotes' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.MixedReality.Portal' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Office.OneNote' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.OutlookforWindows' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Paint' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.People' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.PowerAutomateDesktop' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Windows.RemoteDesktop' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING APPS (BATCH 4/6)
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.ScreenSketch' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.SkypeApp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.SkyDrive.Desktop' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsTerminal' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Todos' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Wallet' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING APPS (BATCH 5/6)
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsAlarms' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsCalculator' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsCamera' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'MicrosoftCorporationII.MicrosoftFamily' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsFeedbackHub' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsMaps' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Windows.DevHome' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsSoundRecorder' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING APPS (BATCH 6/6)
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.YourPhone' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.ZuneMusic' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.ZuneVideo' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'MicrosoftCorporationII.QuickAssist' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'MicrosoftTeams' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'MSTeams' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'MicrosoftWindows.Client.WebExperience' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'SpotifyAB.SpotifyMusic' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsCommunicationsApps' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING PROVISIONED PACKAGES
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Get-AppxProvisionedPackage -Online | Where-Object { $_.DisplayName -like '*Clipchamp*' -or $_.DisplayName -like '*Disney*' -or $_.DisplayName -like '*LinkedIn*' -or $_.DisplayName -like '*BingNews*' -or $_.DisplayName -like '*BingWeather*' -or $_.DisplayName -like '*Copilot*' -or $_.DisplayName -like '*GamingApp*' -or $_.DisplayName -like '*GetHelp*' -or $_.DisplayName -like '*Getstarted*' -or $_.DisplayName -like '*MicrosoftSolitaire*' -or $_.DisplayName -like '*People*' -or $_.DisplayName -like '*WindowsFeedback*' -or $_.DisplayName -like '*WindowsMaps*' -or $_.DisplayName -like '*YourPhone*' -or $_.DisplayName -like '*ZuneMusic*' -or $_.DisplayName -like '*ZuneVideo*' -or $_.DisplayName -like '*Teams*' -or $_.DisplayName -like '*Spotify*' } | ForEach-Object { Remove-AppxProvisionedPackage -Online -PackageName $_.PackageName }" %nul%

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:REMOVE_XBOX_APPS
cls
echo ============================================================
echo                    REMOVING XBOX APPS
echo ============================================================
echo.

echo   - REMOVING XBOX APPS
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.GamingApp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Xbox.App' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.Xbox.TCUI' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.XboxApp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.XboxGameOverlay' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.XboxGamingOverlay' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.XboxSpeechToTextOverlay' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.XboxIdentityProvider' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING PROVISIONED XBOX PACKAGES
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Get-AppxProvisionedPackage -Online | Where-Object { $_.DisplayName -like '*Xbox*' } | ForEach-Object { Remove-AppxProvisionedPackage -Online -PackageName $_.PackageName }" %nul%

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:REMOVE_STORE_APPS
cls
echo ============================================================
echo                  REMOVING MICROSOFT STORE
echo ============================================================
echo.
set /p confirm="THIS WILL BREAK APP INSTALLATIONS. CONTINUE? (Y/N): "
if /i not "%confirm%"=="Y" goto MAIN_MENU

echo   - REMOVING STORE
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.WindowsStore' -AllUsers | Remove-AppxPackage -AllUsers" %nul%
powershell.exe -NoProfile -Command "Get-AppxPackage -Name 'Microsoft.StorePurchaseApp' -AllUsers | Remove-AppxPackage -AllUsers" %nul%

echo   - REMOVING PROVISIONED STORE PACKAGES
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Get-AppxProvisionedPackage -Online | Where-Object { $_.DisplayName -like '*WindowsStore*' -or $_.DisplayName -like '*StorePurchase*' } | ForEach-Object { Remove-AppxProvisionedPackage -Online -PackageName $_.PackageName }" %nul%

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:REMOVE_EDGE
cls
echo ============================================================
echo                  REMOVING MICROSOFT EDGE
echo ============================================================
echo.

echo   - KILLING EDGE PROCESSES
taskkill /f /im msedge.exe %nul%
taskkill /f /im msedgeupdate.exe %nul%
taskkill /f /im MicrosoftEdgeUpdate.exe %nul%
timeout /t 2 /nobreak >nul

echo   - REMOVING EDGE DIRECTORIES
rd /s /q "C:\Program Files (x86)\Microsoft\Edge" %nul%
rd /s /q "C:\Program Files (x86)\Microsoft\EdgeCore" %nul%
rd /s /q "C:\Program Files (x86)\Microsoft\EdgeUpdate" %nul%
rd /s /q "C:\Program Files\Microsoft\Edge" %nul%
rd /s /q "C:\Program Files\Microsoft\EdgeUpdate" %nul%
rd /s /q "C:\ProgramData\Microsoft\Edge" %nul%
rd /s /q "C:\ProgramData\Microsoft\EdgeUpdate" %nul%
rd /s /q "%APPDATA%\Microsoft\Internet Explorer" %nul%

echo   - REMOVING EDGE SHORTCUTS
del /f /q "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk" %nul%
del /f /q "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Microsoft Edge.lnk" %nul%
del /f /q "%PUBLIC%\Desktop\Microsoft Edge.lnk" %nul%
del /f /q "%USERPROFILE%\Desktop\Microsoft Edge.lnk" %nul%

echo   - REMOVING EDGE REGISTRY
reg delete "HKLM\Software\Microsoft\EdgeUpdate" /f %nul%
reg delete "HKLM\Software\Clients\StartMenuInternet\Microsoft Edge" /f %nul%
reg delete "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge" /f %nul%

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:REMOVE_ONEDRIVE
cls
echo ============================================================
echo                    REMOVING ONEDRIVE
echo ============================================================
echo.
set /p confirm="IF ONEDRIVE IS BACKING UP YOUR FILES, THEY WILL BE DELETED. CONTINUE? (Y/N): "
if /i not "%confirm%"=="Y" goto MAIN_MENU

echo   - KILLING ONEDRIVE PROCESS
taskkill /f /im OneDrive.exe %nul%
timeout /t 2 /nobreak >nul

echo   - UNINSTALLING ONEDRIVE
if exist "%SystemRoot%\SysWOW64\OneDriveSetup.exe" (
    start /wait "" "%SystemRoot%\SysWOW64\OneDriveSetup.exe" /uninstall
)
if exist "%SystemRoot%\System32\OneDriveSetup.exe" (
    start /wait "" "%SystemRoot%\System32\OneDriveSetup.exe" /uninstall
)
if exist "%SystemRoot%\Microsoft OneDrive\OneDriveSetup.exe" (
    start /wait "" "%SystemRoot%\Microsoft OneDrive\OneDriveSetup.exe" /uninstall
)
timeout /t 3 /nobreak >nul

echo   - REMOVING ONEDRIVE DIRECTORIES
rd /s /q "%USERPROFILE%\OneDrive" %nul%
rd /s /q "%LOCALAPPDATA%\Microsoft\OneDrive" %nul%
rd /s /q "%LOCALAPPDATA%\OneDrive" %nul%
rd /s /q "%ProgramData%\Microsoft OneDrive" %nul%
rd /s /q "C:\Program Files\Microsoft\OneDrive" %nul%
rd /s /q "C:\Program Files (x86)\Microsoft\OneDrive" %nul%
rd /s /q "C:\Program Files\Microsoft OneDrive" %nul%
rd /s /q "C:\Program Files (x86)\Microsoft OneDrive" %nul%

echo   - REMOVING ONEDRIVE SHORTCUTS
del /f /q "%USERPROFILE%\Desktop\OneDrive.lnk" %nul%
del /f /q "%APPDATA%\Microsoft\Windows\Start Menu\Programs\OneDrive.lnk" %nul%
del /f /q "%ProgramData%\Microsoft\Windows\Start Menu\Programs\OneDrive.lnk" %nul%

echo   - REMOVING ONEDRIVE REGISTRY
reg delete "HKCR\OneDrive" /f %nul%
reg delete "HKCU\Software\Microsoft\OneDrive" /f %nul%
reg delete "HKLM\Software\Microsoft\OneDrive" /f %nul%
reg delete "HKCR\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}" /f %nul%
reg delete "HKCR\Wow6432Node\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}" /f %nul%
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\OneDrive" /v DisableFileSyncNGSC /t REG_DWORD /d 1 /f %nul%

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:FULL_CLEANUP_SILENT
echo [1/5] CLEANING TEMP FILES
call :CLEAN_TEMP_SILENT
echo [2/5] CLEANING CACHE FILES
call :CLEAN_CACHE_SILENT
echo [3/5] CLEANING EVENT LOGS
call :CLEAN_EVENTLOGS_SILENT
echo [4/5] CLEANING DEFAULT POWER PLANS
powercfg -delete 381b4222-f694-41f0-9685-ff5bb260df2e %nul%
powercfg -delete a1841308-3541-4fab-bc81-f71556f20b4a %nul%
powercfg -delete 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c %nul%
powercfg -delete e9a42b02-d5df-448d-aa00-03f14749eb61 %nul%
echo [5/5] CLEANING RECYCLE BIN
call :CLEAN_RECYCLE_SILENT
exit /b


:CLEAN_TEMP
cls
echo ============================================================
echo                    CLEANING TEMP FILES
echo ============================================================
echo.
call :CLEAN_TEMP_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:CLEAN_TEMP_SILENT
echo   - CLEANING USER TEMP
del /f /s /q "%TEMP%\*" %nul%
rd /s /q "%TEMP%" %nul%
mkdir "%TEMP%" %nul%

echo   - CLEANING WINDOWS TEMP
del /f /s /q "C:\Windows\Temp\*" %nul%
rd /s /q "C:\Windows\Temp" %nul%
mkdir "C:\Windows\Temp" %nul%

echo   - CLEANING PREFETCH
del /f /s /q "C:\Windows\Prefetch\*" %nul%

echo   - CLEANING LOCAL TEMP
del /f /s /q "%LOCALAPPDATA%\Temp\*" %nul%
rd /s /q "%LOCALAPPDATA%\Temp" %nul%
mkdir "%LOCALAPPDATA%\Temp" %nul%

exit /b


:CLEAN_CACHE
cls
echo ============================================================
echo                    CLEANING CACHE FILES
echo ============================================================
echo.
call :CLEAN_CACHE_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:CLEAN_CACHE_SILENT
echo   - CLEANING CHROME CACHE
del /f /s /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Cache\*" %nul%
rd /s /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Cache" %nul%
del /f /s /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Code Cache\*" %nul%
rd /s /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Code Cache" %nul%
del /f /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Network\Cookies" %nul%

echo   - CLEANING EDGE CACHE
del /f /s /q "%LOCALAPPDATA%\Microsoft\Edge\User Data\Default\Cache\*" %nul%
rd /s /q "%LOCALAPPDATA%\Microsoft\Edge\User Data\Default\Cache" %nul%

echo   - CLEANING WINDOWS UPDATE CACHE
net stop wuauserv %nul%
net stop bits %nul%
timeout /t 1 /nobreak >nul
del /f /s /q "C:\Windows\SoftwareDistribution\Download\*" %nul%
rd /s /q "C:\Windows\SoftwareDistribution\Download" %nul%
mkdir "C:\Windows\SoftwareDistribution\Download" %nul%

exit /b


:CLEAN_EVENTLOGS
cls
echo ============================================================
echo                    CLEANING EVENT LOGS
echo ============================================================
echo.
call :CLEAN_EVENTLOGS_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:CLEAN_EVENTLOGS_SILENT
echo   - CLEARING EVENT LOGS
wevtutil cl Application %nul%
wevtutil cl System %nul%
wevtutil cl Security %nul%
wevtutil cl Setup %nul%
wevtutil cl ForwardedEvents %nul%
exit /b


:CLEAN_POWERPLANS
cls
echo ============================================================
echo                CLEANING DEFAULT POWER PLANS
echo ============================================================
echo.
echo   - REMOVING DEFAULT POWER PLANS
powercfg -delete 381b4222-f694-41f0-9685-ff5bb260df2e %nul%
powercfg -delete a1841308-3541-4fab-bc81-f71556f20b4a %nul%
powercfg -delete 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c %nul%
powercfg -delete e9a42b02-d5df-448d-aa00-03f14749eb61 %nul%
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:CLEAN_RECYCLE
cls
echo ============================================================
echo                    CLEANING RECYCLE BIN
echo ============================================================
echo.
call :CLEAN_RECYCLE_SILENT
echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:CLEAN_RECYCLE_SILENT
echo   - EMPTYING RECYCLE BIN
powershell.exe -NoProfile -Command "Clear-RecycleBin -Force -ErrorAction SilentlyContinue" %nul%
exit /b


:FULL_CLEANUP
cls
echo ============================================================
echo                       FULL CLEANUP
echo ============================================================
echo.

call :FULL_CLEANUP_SILENT

echo.
echo ============================================================
echo                           DONE
echo ============================================================
echo.
pause
goto MAIN_MENU


:EXIT
cls
echo ============================================================
echo.
echo     RESTART YOUR COMPUTER FOR ALL CHANGES TO TAKE EFFECT
echo                     MADE BY HSELIMT
echo.
echo ============================================================
echo.
pause
exit /b 0