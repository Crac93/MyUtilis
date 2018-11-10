using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyUtilis
{
    /// <summary>
    /// 
    /// </summary>
    public class Utilis
    {
        /// <summary>
        /// Get te curren user of machine.
        /// </summary>
        /// <returns></returns>
        public static string User { get { return System.Environment.UserName.ToString(); } }

        /// <summary>
        /// Get the currrent machine name.
        /// </summary>
        public static string MachineName { get { return System.Environment.MachineName.ToString(); } }

        /// <summary>
        /// Get the current domain.
        /// </summary>
        public static string Domain { get { return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 

        public static string NETframework { get { return System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion().ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string ServicePack
        {
            get
            {
                return System.Environment.OSVersion.ServicePack.ToString();
            }
        }

        /// <summary>
        /// Get OS Version
        /// </summary>
        public static string OSversion
        {
            get
            {
                return System.Environment.OSVersion.VersionString;
            }
        }

        /// <summary>
        /// Get OSplataform
        /// </summary>
        public static string OSplatform
        {
            get
            {
                return System.Environment.OSVersion.Platform.ToString();
            }
        }

        /// <summary>
        /// Get SystemType, example : 64 bit.
        /// </summary>
        public static string SystemType
        {
            get
            {
                return System.Environment.Is64BitOperatingSystem.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueID()
        {
            string format = "MMddyyHHmmss";

            return (DateTime.Now.ToString(format));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Seconds"></param>
        public static void Sleep(int Seconds)
        {
            int miliseconds
                = (Seconds * 1000);
            Thread.Sleep(miliseconds);
        }

        /// <summary>
        /// Get the mac adress of machine
        /// </summary>
        public static string MACAddress
        {
            get
            {
                string Result = null;

                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.OperationalStatus ==
                        OperationalStatus.Up && ni.GetPhysicalAddress().GetAddressBytes().Length != 0)
                    {
                        Result = ni.GetPhysicalAddress().ToString();
                    }
                }
                return Result;
            }
        }

        /// <summary>
        ///  Get the local IP
        /// </summary>
        public static string LocalIP
        {
            get
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                return localIP;
            }
        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDHCPServers()
        {
            string result = null;

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection addresses = adapterProperties.DhcpServerAddresses;
                if (addresses.Count > 0)
                {
                    foreach (IPAddress address in addresses)
                    {
                        result += address.ToString();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DefaulGateway()
        {
            string result = null;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            string intranetDomainName = ipProperties.DomainName;
            foreach (NetworkInterface networkCard in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (GatewayIPAddressInformation gatewayAddress in networkCard.GetIPProperties().GatewayAddresses)
                {
                    result = gatewayAddress.Address.ToString();
                }
            }
            return result;
        }

        public static bool DHCPEnabled()
        {
            bool IsDHCPEnabled = true;

            return IsDHCPEnabled;
        }

        /// <summary>
        /// Gets the local data and puts in a pad.
        /// </summary>
        public static void GetLocalHostData(string path)
        {

            string Line = System.Environment.NewLine;
            var Result = new Dictionary<string, string>();

            Result.Add("Macaddress:", MACAddress);
            Result.Add("SystemType:", SystemType);
            Result.Add("IpLocal:", LocalIP);
            Result.Add("OsVersion:", OSversion);
            //FilesManager.
          
            FilesManager.CreateTextFile(path, Result.ToString());
        }

        public static string Debug()
        {
            string format = "MM.dd.yy HH:mm:ss";
            return (" [" + DateTime.Now.ToString(format) + "] ");
        }


        /// <summary>
        /// Execumte a comanda in CMD
        /// </summary>
        /// <param name="Command"></param>
        public static void ExecuteCommandSync(string Command)
        {
            Console.WriteLine(Utilis.Debug() + "ExecuteCommandSync()");
            Console.WriteLine(Utilis.Debug() + "Command: " + Command);
            try
            {

                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + Command);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string Result = proc.StandardOutput.ReadToEnd();

                Console.WriteLine(Utilis.Debug() + "Result: " + Result);
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.ToString());
            }
        }


    }
}
