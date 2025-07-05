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
            tbStatus.Text = "DISABLING WINDOWS UPDATES";

            string[] WUservicesToDisable = new string[]
            {
                "WaaSMedicSvc",
                "wuauserv",
                "UsoSvc",
                "BITS"
            };

            bool[] serviceExists = CheckServices(WUservicesToDisable);

            for (int i = 0; i < WUservicesToDisable.Length; i++)
            {
                string serviceName = WUservicesToDisable[i];
                if (!serviceExists[i])
                {
                    tbStatus.Text = $"{serviceName} DOESN'T EXIST";
                    continue;
                }

                try
                {
                    using (var sc = new ServiceController(serviceName))
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped)
                        {
                            await _setServices.StopServiceAsync(sc);
                        }
                    }

                    string registryPath = $@"SYSTEM\CurrentControlSet\Services\{serviceName}";

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true)!)
                    {
                        if (key != null)
                        {
                            key.SetValue("Start", 4, RegistryValueKind.DWord);
                        }
                    }
                }
                catch (Exception)
                {
                    tbStatus.Text = $"ERROR HANDLING {serviceName} SERVICE";
                    return false;
                }
            }

            try
            {
                tbStatus.Text = "MODIFYING REGISTRY";

                string wuRegistryPath = @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";
                using (RegistryKey wuKey = Registry.LocalMachine.OpenSubKey(wuRegistryPath, writable: true) ??
                                          Registry.LocalMachine.CreateSubKey(wuRegistryPath))
                {
                    wuKey.SetValue("NoAutoUpdate", 1, RegistryValueKind.DWord);
                    wuKey.SetValue("AUOptions", 1, RegistryValueKind.DWord);
                }

                string doRegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config";
                using (RegistryKey doKey = Registry.LocalMachine.OpenSubKey(doRegistryPath, writable: true) ??
                                          Registry.LocalMachine.CreateSubKey(doRegistryPath))
                {
                    doKey.SetValue("DODownloadMode", 0, RegistryValueKind.DWord);
                }

                tbStatus.Text = "WINDOWS UPDATES DISABLED";
                return true;
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR MODIFYING REGISTRY {ex.Message}";
                return false;
            }
        }
        catch (Exception ex)
        {
            tbStatus.Text = $"ERROR {ex.Message}\r\n";
            return false;
        }
    }
}