using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SetServicesApp
{
    public class SetServices
    {
        public static List<string> DefaultServicesToDisable = new List<string>
        {
            "tzautoupdate", "BthAvctpSvc", "BDESVC", "wbengine", "autotimesvc",
            "ClipSVC", "cbdhsvc*", "DiagTrack", "ConsentUxUserSvc*", "PimIndexMaintenanceSvc*",
            "CDPUserSvc*", "DsSvc", "DusmSvc*", "DoSvc", "DmEnrollmentSvc", "diagsvc",
            "DialogBlockingService", "DisplayEnhancementService", "fhsvc", "BcastDVRUserService*",
            "lfsvc", "iphlpsvc", "LxpSvc*", "MicrosoftEdgeElevationService", "edgeupdate",
            "edgeupdatem", "MsKeyboardFilter", "NgcSvc", "NgcCtnrSvc", "InstallService",
            "uhssvc", "SmsRouter", "NetTcpPortSharing", "Netlogon", "NcbService", "CscService",
            "defragsvc", "WpcMonSvc", "SEMgrSvc", "PhoneSvc", "Spooler",
            "PrintDeviceConfigurationService", "PrintNotify", "PrintWorkflowUserSvc*",
            "wercplsupport", "PcaSvc", "QWAVE", "RmSvc", "TroubleshootingSvc",
            "RasAuto", "RasMan", "SessionEnv", "UmRdpService", "RemoteRegistry",
            "RemoteAccess", "SensorDataService", "SensrSvc", "SensorService", "LanmanServer",
            "shpamsvc", "SCardSvr", "ScDeviceEnum", "SCPolicySvc", "OneSyncSvc*", "SysMain",
            "TabletInputService", "UserDataSvc*", "UnistoreSvc*", "UevAgentService",
            "SDRSVC", "FrameServer", "wcncsvc", "Wecsvc", "wisvc", "MixedRealityOpenXRSvc",
            "icssvc", "spectrum", "perceptionsimulation", "WpnUserService*", "PushToInstall",
            "W32Time", "WFDSConMgrSvc", "WSearch", "LanmanWorkstation", "AppVClient",
            "cloudidsvc", "diagnosticshub.standardcollector.service", "wlidsvc"
        };

        public async Task DisableConfiguredServices()
        {
            foreach (string service in DefaultServicesToDisable.Distinct())
            {
                try
                {
                    if (service.EndsWith("*"))
                    {
                        await DisableServicesByRegKeyPattern(service.TrimEnd('*'));
                    }
                    else
                    {
                        await DisableService(service);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public async Task DisableMultipleServices(string[] additionalServices)
        {
            var allServices = DefaultServicesToDisable.Concat(additionalServices).Distinct();

            foreach (string service in allServices)
            {
                try
                {
                    if (service.EndsWith("*"))
                    {
                        await DisableServicesByRegKeyPattern(service.TrimEnd('*'));
                    }
                    else
                    {
                        await DisableService(service);
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }

        private async Task<bool> DisableService(string serviceName)
        {
            try
            {
                if (!ServiceExists(serviceName)) return false;

                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
                    {
                        await StopServiceAsync(sc);
                    }
                }

                string registryPath = $@"SYSTEM\CurrentControlSet\Services\{serviceName}";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("Start", 4, RegistryValueKind.DWord);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task DisableServicesByRegKeyPattern(string serviceNamePattern)
        {
            try
            {
                string servicesPath = @"SYSTEM\CurrentControlSet\Services";
                using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(servicesPath, true))
                {
                    if (servicesKey == null) return;

                    var matchingServices = servicesKey.GetSubKeyNames()
                        .Where(name => name.StartsWith(serviceNamePattern, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (string serviceName in matchingServices)
                    {
                        try
                        {
                            if (ServiceExists(serviceName))
                            {
                                using (ServiceController sc = new ServiceController(serviceName))
                                {
                                    if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
                                    {
                                        await StopServiceAsync(sc);
                                    }
                                }
                            }

                            using (RegistryKey serviceKey = servicesKey.OpenSubKey(serviceName, true))
                            {
                                serviceKey?.SetValue("Start", 4, RegistryValueKind.DWord);
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
            catch
            {
                
            }
        }

        public bool ServiceExists(string serviceName)
        {
            try
            {
                return ServiceController.GetServices().Any(s => s.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
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
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
                catch
                {

                }
            });
        }
    }
}