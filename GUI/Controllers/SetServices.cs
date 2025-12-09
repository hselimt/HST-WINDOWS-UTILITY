using HST.Controllers;
using HST.Controllers.RemovalTools;
using System.Diagnostics;
using Microsoft.Win32;

#pragma warning disable CA1416
#pragma warning disable CS8600
#pragma warning disable CS8618

namespace HST.Controllers.SetService
{
    public class ServiceInfo
    {
        public string service { get; set; }
        public string name { get; set; }
    }

    public class SetServices
    {
        private readonly RemovalHelpers _removalTools;

        public SetServices(RemovalHelpers removalTools)
        {
            _removalTools = removalTools ?? throw new ArgumentNullException(nameof(removalTools));
        }

        public async Task DisableConfiguredServices(List<string> servicesToDisable)
        {
            if (!servicesToDisable.Any()) return;

            var wServices = servicesToDisable.Where(s => s.Contains("*")).ToList();
            var services = servicesToDisable.Where(s => !s.Contains("*")).ToList();

            string disableServicesScript = "$ErrorActionPreference = 'SilentlyContinue'\n";

            if (services.Any())
            {
                string serviceList = string.Join("', '", services);
                disableServicesScript += $@"
                $services = @('{serviceList}')
                foreach ($svc in $services) {{
                    Stop-Service -Name $svc -Force
                    Set-Service -Name $svc -StartupType Disabled

                    # Restrict permissions to make it unrevertable
                    sc.exe sdset $svc 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                }}
                ";
            }

            foreach (string pattern in wServices)
            {
                string cleanPattern = pattern.Replace("*", "");
                disableServicesScript += $@"
                    Get-Service | Where-Object {{ $_.Name -like '{cleanPattern}*' }} | ForEach-Object {{
                        Stop-Service -Name $_.Name -Force
                        Set-Service -Name $_.Name -StartupType Disabled

                        # Restrict permissions to make it unrevertable
                        sc.exe sdset $_.Name 'D:(D;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCLCSWRPWPDTLOCRRC;;;BA)'
                    }}
                ";
            }

            await _removalTools.RunCommandAsync("powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"{disableServicesScript.Replace("\"", "\"\"")}\"");
        }

        public async Task DisableConfiguredServicesRevert()
        {
            var services = new Dictionary<string, int>
            {
                { "tzautoupdate", 3 }, { "BthAvctpSvc", 3 }, { "BDESVC", 3 }, { "wbengine", 3 },
                { "autotimesvc", 3 }, { "ClipSVC", 3 }, { "DiagTrack", 2 }, { "DsSvc", 3 },
                { "DoSvc", 2 }, { "DmEnrollmentSvc", 3 }, { "dmwappushservice", 3 }, { "diagsvc", 3 },
                { "DPS", 2 }, { "DialogBlockingService", 4 }, { "DisplayEnhancementService", 3 }, { "fhsvc", 3 },
                { "lfsvc", 3 }, { "iphlpsvc", 2 }, { "MapsBroker", 2 }, { "MicrosoftEdgeElevationService", 3 },
                { "edgeupdate", 2 }, { "edgeupdatem", 3 }, { "MsKeyboardFilter", 4 }, { "NgcSvc", 3 },
                { "NgcCtnrSvc", 3 }, { "InstallService", 3 }, { "uhssvc", 3 }, { "SmsRouter", 3 },
                { "NetTcpPortSharing", 4 }, { "Netlogon", 3 }, { "NcbService", 3 }, { "CscService", 3 },
                { "defragsvc", 3 }, { "WpcMonSvc", 3 }, { "SEMgrSvc", 3 }, { "PhoneSvc", 3 },
                { "Spooler", 2 }, { "PrintDeviceConfigurationService", 3 }, { "PrintNotify", 3 }, { "wercplsupport", 3 },
                { "PcaSvc", 2 }, { "QWAVE", 3 }, { "RmSvc", 3 }, { "TroubleshootingSvc", 3 },
                { "RasAuto", 3 }, { "RasMan", 3 }, { "SessionEnv", 3 }, { "UmRdpService", 3 },
                { "RemoteRegistry", 4 }, { "RemoteAccess", 4 }, { "RetailDemo", 3 }, { "SensorDataService", 3 },
                { "SensrSvc", 3 }, { "SensorService", 3 }, { "LanmanServer", 2 }, { "shpamsvc", 4 },
                { "SCardSvr", 3 }, { "ScDeviceEnum", 3 }, { "SCPolicySvc", 3 }, { "SysMain", 2 },
                { "TabletInputService", 3 }, { "TapiSrv", 3 }, { "UevAgentService", 4 }, { "SDRSVC", 3 },
                { "FrameServer", 3 }, { "wcncsvc", 3 }, { "Wecsvc", 3 }, { "wisvc", 3 },
                { "MixedRealityOpenXRSvc", 3 }, { "icssvc", 3 }, { "spectrum", 3 }, { "perceptionsimulation", 3 },
                { "PushToInstall", 3 }, { "W32Time", 3 }, { "WFDSConMgrSvc", 3 }, { "WSearch", 2 },
                { "LanmanWorkstation", 2 }, { "AppVClient", 4 }, { "cloudidsvc", 3 }, { "diagnosticshub.standardcollector.service", 3 },
                { "WbioSrvc", 3 }, { "WdiSystemHost", 3 }, { "WdiServiceHost", 3 }, { "wlidsvc", 3 },
                { "WerSvc", 3 }, { "workfolderssvc", 3 }, { "BTAGService", 3 }, { "bthserv", 3 },
                { "HvHost", 3 }, { "vmickvpexchange", 3 }, { "vmicguestinterface", 3 }, { "vmicshutdown", 3 },
                { "vmicheartbeat", 3 }, { "vmcompute", 3 }, { "vmicvmsession", 3 }, { "vmicrdv", 3 },
                { "vmictimesync", 3 }, { "vmms", 3 }, { "vmicvss", 3 }, { "XboxGipSvc", 3 },
                { "XblAuthManager", 3 }, { "XblGameSave", 3 }, { "XboxNetApiSvc", 3 }
            };

            foreach (var service in services)
            {
                try
                {
                    Registry.LocalMachine.DeleteSubKeyTree(
                        $@"SYSTEM\CurrentControlSet\Services\{service.Key}\Security", false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Security key for {service.Key}: {ex.Message}");
                }

                try
                {
                    Registry.SetValue(
                        $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{service.Key}",
                        "Start",
                        service.Value,
                        RegistryValueKind.DWord);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting Start value for {service.Key}: {ex.Message}");
                }
            }
        }
    }
}