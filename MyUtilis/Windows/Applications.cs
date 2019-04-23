using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Management;

namespace MyUtilis.Windows
{
    class Applications
    {

        public List<object> InstalledAppsW32()
        {
            var lstInstalled = new List<object>();
            var mos = new ManagementObjectSearcher(
                         "SELECT * FROM Win32_Product");
            foreach (var o in mos.Get())
            {
                var mo = (ManagementObject)o;
                lstInstalled.Add(mo["name"]);
              
            }

            return lstInstalled;
        }

        public List<object> InstalledAppUser()
        {
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var lst = new List<object>();
          
            using (var key = Registry.CurrentUser.OpenSubKey(registryKey))
            {
                foreach (var subKeyName in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(subKeyName))
                    {
                        if (subKey.GetValue("DisplayName") != null)
                        {
                            lst.Add(subKey.GetValue("DisplayName"));
                        }
                    }
                }
            }
            return lst;
        }

        public List<object> InformationSingleApp(string name)
        {
            var lst = new List<object>();
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Registry.CurrentUser.OpenSubKey(registryKey))
            {
                foreach (var subKeyName in key.GetSubKeyNames())
                {
                    using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                    {
                        var nameKey = subKey?.GetValue("DisplayName");
                        if (nameKey != null && nameKey.ToString() == name)
                        {
                            lst.Add(subKey.GetValue("DisplayName"));
                            lst.Add(subKey.GetValue("DisplayVersion"));
                            lst.Add(subKey.GetValue("Publisher"));
                            lst.Add(subKey.GetValue("UrlUpdateInfo"));
                        }
                    }
                }
            }
            return lst;
        }

        private static bool VerifyProcessRun(string processName)
        {
            var response = false;
            var processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == processName)
                    response = true;
            }

            return response;
        }

        public void ProcessAllwaysRun(string processName, string pathRun)
        {
            var vp = VerifyProcessRun(processName);
            if (!vp)
                Process.Start(pathRun);
        }



    }
}
