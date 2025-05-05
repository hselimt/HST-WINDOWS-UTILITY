using Microsoft.Win32;
using SetServicesApp;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

public class DisableWindowsUpdatesApp
{
    private readonly SetServices _setServices;

    public DisableWindowsUpdatesApp()
    {
        _setServices = new SetServices();
    }

    public DisableWindowsUpdatesApp(SetServices setServices)
    {
        _setServices = setServices ?? throw new ArgumentNullException(nameof(setServices));
    }

    private bool[] CheckServices(string[] serviceNames)
    {
        bool[] results = new bool[serviceNames.Length];
        for (int i = 0; i < serviceNames.Length; i++)
        {
            results[i] = _setServices.ServiceExists(serviceNames[i]);
        }
        return results;
    }

    public async Task<bool> DisableWindowsUpdates(TextBox tbStatus)
    {
        try
        {
            tbStatus.Text += "Starting Windows Update disabling process...\r\n";
            tbStatus.Refresh();

            string[] WUservicesToDisable = new string[]
            {
                "wuauserv",       // Windows Update
                "UsoSvc",         // Update Orchestrator
                "BITS"            // Background Intelligent Transfer
            };

            bool[] serviceExists = CheckServices(WUservicesToDisable);

            for (int i = 0; i < WUservicesToDisable.Length; i++)
            {
                string serviceName = WUservicesToDisable[i];
                if (!serviceExists[i])
                {
                    tbStatus.Text += $"Service {serviceName} does not exist, skipping.\r\n";
                    tbStatus.Refresh();
                    continue;
                }

                try
                {
                    using (var sc = new ServiceController(serviceName))
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped)
                        {
                            tbStatus.Text += $"Stopping service: {serviceName}\r\n";
                            tbStatus.Refresh();
                            await _setServices.StopServiceAsync(sc);
                            tbStatus.Text += $"Service {serviceName} stopped successfully.\r\n";
                            tbStatus.Refresh();
                        }
                        else
                        {
                            tbStatus.Text += $"Service {serviceName} is already stopped.\r\n";
                            tbStatus.Refresh();
                        }
                    }

                    tbStatus.Text += $"Modifying registry for {serviceName}...\r\n";
                    tbStatus.Refresh();
                    string registryPath = $@"SYSTEM\CurrentControlSet\Services\{serviceName}";
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true))
                    {
                        if (key != null)
                        {
                            key.SetValue("Start", 4, RegistryValueKind.DWord);
                            tbStatus.Text += $"Registry modified for {serviceName}.\r\n";
                            tbStatus.Refresh();
                        }
                        else
                        {
                            tbStatus.Text += $"Could not open registry key for {serviceName}. Make sure you're running as administrator.\r\n";
                            tbStatus.Refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    tbStatus.Text += $"Error handling service {serviceName}: {ex.Message}\r\n";
                    tbStatus.Refresh();
                    return false;
                }
            }

            try
            {
                tbStatus.Text += "Modifying Windows Update settings...\r\n";
                tbStatus.Refresh();
                string wuRegistryPath = @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";
                using (RegistryKey wuKey = Registry.LocalMachine.OpenSubKey(wuRegistryPath, writable: true) ??
                                          Registry.LocalMachine.CreateSubKey(wuRegistryPath))
                {
                    wuKey.SetValue("NoAutoUpdate", 1, RegistryValueKind.DWord);
                    wuKey.SetValue("AUOptions", 1, RegistryValueKind.DWord);
                    tbStatus.Text += "Windows Update registry settings updated.\r\n";
                    tbStatus.Refresh();
                }

                tbStatus.Text += "Modifying Delivery Optimization settings...\r\n";
                tbStatus.Refresh();
                string doRegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config";
                using (RegistryKey doKey = Registry.LocalMachine.OpenSubKey(doRegistryPath, writable: true) ??
                                          Registry.LocalMachine.CreateSubKey(doRegistryPath))
                {
                    doKey.SetValue("DODownloadMode", 0, RegistryValueKind.DWord);
                    tbStatus.Text += "Delivery Optimization settings updated.\r\n";
                    tbStatus.Refresh();
                }

                tbStatus.Text += "Windows Updates successfully disabled.\r\n";
                tbStatus.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                tbStatus.Text += $"Error modifying registry settings: {ex.Message}\r\n";
                tbStatus.Refresh();
                return false;
            }
        }
        catch (Exception ex)
        {
            tbStatus.Text += $"General error: {ex.Message}\r\n";
            tbStatus.Refresh();
            return false;
        }
    }
}