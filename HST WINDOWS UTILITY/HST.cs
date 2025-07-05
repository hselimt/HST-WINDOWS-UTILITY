using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using OptimizeTaskSchedulerApp;
using PackageRemoverApp;
using CleanupApp;
using RegistryOptimizationApp;
using PowerPlanApp;
using RemovalTools;
using SetServicesApp;
using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Management;
using System.Management.Automation;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;

namespace HST_WINDOWS_UTILITY
{
    public partial class HST : Form
    {
        private readonly RemovalHelpers _removalTools;
        private readonly SetServices _setServices;

        public HST()
        {
            InitializeComponent();
            _removalTools = new RemovalHelpers();
            _setServices = new SetServices();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadHardwareInfo();
        }
        public void LoadHardwareInfo()
        {
            string Username = $"{Environment.UserName}";

            if (!string.IsNullOrWhiteSpace(Username))
            {
                tbStatus.Text = $"WELCOME {Username} PLEASE CREATE RESTORE POINT TO REVERT CHANGES IF NEEDED";
            }
            else
            {
                tbStatus.Text = "WELCOME USER PLEASE CREATE RESTORE POINT TO REVERT CHANGES IF NEEDED";
            }

            try
            {
                var gpu = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>().First();
                tbGPU.Text = $"{gpu["Name"]}";
            }
            catch
            {
                tbGPU.Text = "ERROR";
            }

            try
            {
                var cpu = new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>().First();
                tbCPU.Text = $"{cpu["Name"]}";
            }
            catch
            {
                tbCPU.Text = "ERROR";
            }

            try
            {
                double ramGB = new ComputerInfo().TotalPhysicalMemory / (1024.0 * 1024 * 1024);
                tbRAM.Text = $"{ramGB:F2}GB RAM";
            }
            catch
            {
                tbRAM.Text = "ERROR";
            }

            try
            {
                var drive = new DriveInfo("C");
                double freeGB = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
                tbES.Text = $"{freeGB:F2}GB EMPTY SPACE";
            }
            catch
            {
                tbES.Text = "ERROR";
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void llGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://www.github.com/hselimt",
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception)
            {
                tbStatus.Text = ("COULDN'T VISIT LINK");
            }
        }
        private void btnCRP_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("systempropertiesprotection");
            }
            catch (Exception)
            {
                tbStatus.Text = ("COULDN'T OPEN SYSTEM RESTORE");
            }
        }
        private async void btnREG_Click(object sender, EventArgs e)
        {
            var registryOptimization = new RegistryOptimizationApp.RegistryOptimizer();
            tbStatus.Text = "OPTIMIZING REGISTRY";
            await Task.Delay(1000);

            try
            {
                await registryOptimization.OptimizeRegistryAsync();
                tbStatus.Text = "REGISTRY HAS BEEN OPTIMIZED";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnTS_Click(object sender, EventArgs e)
        {
            var optimizeTaskScheduler = new OptimizeTaskSchedulerApp.OptimizeTaskScheduler();
            tbStatus.Text = "OPTIMIZING TASK SCHEDULER";
            await Task.Delay(1000);

            try
            {
                await optimizeTaskScheduler.DisableAllScheduledTasksAsync();
                tbStatus.Text = "TASK SCHEDULER HAS BEEN OPTIMIZED";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnDU_Click(object sender, EventArgs e)
        {
            var disableWindowsUpdates = new DisableWindowsUpdatesApp();
            tbStatus.Text = "DISABLING WINDOWS UPDATES";
            await Task.Delay(1000);

            try
            {     
                await disableWindowsUpdates.DisableWindowsUpdates(tbStatus);
                tbStatus.Text = "DISABLED WINDOWS UPDATES";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnLV_Click(object sender, EventArgs e)
        {
            var registryOptimization = new RegistryOptimizationApp.RegistryOptimizer();
            tbStatus.Text = "LOWERING VISUALS";
            await Task.Delay(1000);

            try
            {
                await registryOptimization.LowerVisualsAsync();
                tbStatus.Text = "VISUALS ARE OPTIMIZED";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnDM_Click(object sender, EventArgs e)
        {
            var registryOptimization = new RegistryOptimizationApp.RegistryOptimizer();
            tbStatus.Text = "SWITCHING TO DARK MODE";
            await Task.Delay(1000);

            try
            {
                await registryOptimization.SwitchDarkModeAsync();
                tbStatus.Text = "DARK MODE HAS BEEN SET";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnAPP_Click(object sender, EventArgs e)
        {
            var removalHelpers = new RemovalTools.RemovalHelpers();
            var _powerPlan = new PowerPlan(removalHelpers);
            tbStatus.Text = "ADDING POWERPLAN";
            await Task.Delay(1000);

            try
            {
                await _powerPlan.AddAndActivatePP();
                tbStatus.Text = "ACTIVATED POWERPLAN";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
        private async void btnSS_Click(object sender, EventArgs e)
        {
            try
            {
                var setServices = new SetServices();
                var additionalServices = new List<string>();
                tbStatus.Text = "OPTIMIZING SERVICES";
                await Task.Delay(1000);

                if (clbSS.GetItemChecked(0))
                {
                    additionalServices.AddRange(new[] { "BTAGService", "bthserv", "BluetoothUserService" });
                }

                if (clbSS.GetItemChecked(1))
                {
                    additionalServices.AddRange(new[] { "HvHost", "vmickvpexchange", "vmicguestinterface" });
                }

                if (clbSS.GetItemChecked(2))
                {
                    additionalServices.AddRange(new[] { "XboxGipSvc", "XblAuthManager", "XblGameSave", "XboxNetApiSvc" });
                }

                if (additionalServices.Any())
                {
                    await setServices.DisableMultipleServices(additionalServices.ToArray());
                }

                else
                {
                    await setServices.DisableConfiguredServices();
                }

                tbStatus.Text = "SERVICES ARE OPTIMIZED";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }

        private async void btnDEB_Click(object sender, EventArgs e)
        {
            PackagesRemover packageRemover = new PackagesRemover(_removalTools);
            tbStatus.Text = "STARTING DEBLOAT";

            if (clbDEB.CheckedItems.Count == 0)
            {
                tbStatus.Text = "YOU HAVEN'T CHECKED ANY BOX";
                await Task.Delay(1000);
                tbStatus.Text = "";
                return;
            }

            for (int i = 0; i < clbDEB.Items.Count; i++)
            {
                if (clbDEB.GetItemChecked(i))
                {
                    switch (i)
                    {
                        case 0:
                            tbStatus.Text = "REMOVING MICROSOFT PACKAGES";
                            await packageRemover.RemoveMicrosoftAppsAsync();
                            await Task.Delay(100);
                            break;

                        case 1:
                            tbStatus.Text = "REMOVING EDGE";
                            await packageRemover.RemoveEdgeAsync();
                            await Task.Delay(100);
                            break;

                        case 2:
                            tbStatus.Text = "REMOVING ONEDRIVE";
                            await packageRemover.RemoveOneDriveAsync();
                            await Task.Delay(100);
                            break;

                        case 3:
                            tbStatus.Text = "REMOVING XBOX";
                            await packageRemover.RemoveXboxAppsAsync();
                            await Task.Delay(100);
                            break;

                        case 4:
                            tbStatus.Text = "REMOVING STORE";
                            await packageRemover.RemoveStoreAppsAsync();
                            await Task.Delay(100);
                            break;
                    }
                }
            }

            tbStatus.Text = "DEBLOATING HAS BEEN COMPLETED";
            await Task.Delay(1000);
            tbStatus.Text = "";
        }
        private async void btnCU_Click(object sender, EventArgs e)
        {
            var removalHelpers = new RemovalTools.RemovalHelpers();
            var cleanup = new CleanupApp.Cleanup(removalHelpers);

            bool cleanTemp = clbCU.GetItemChecked(0);
            bool cleanInternetCache = clbCU.GetItemChecked(1);
            bool cleanEventLogs = clbCU.GetItemChecked(2);
            bool cleanDefaultPowerPlans = clbCU.GetItemChecked(3);
            tbStatus.Text = "STARTING CLEANUP";
            await Task.Delay(1000);

            try
            {
                await cleanup.RunRemoval(cleanTemp, cleanInternetCache, cleanEventLogs, cleanDefaultPowerPlans);
                tbStatus.Text = "CLEANUP IS DONE";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"ERROR: {ex.Message}";
                await Task.Delay(1000);
                tbStatus.Text = "";
            }
        }
    }
}