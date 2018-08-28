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

    public class Utilis
    {

        public static string Debug()
        {
            string format = "MM.dd.yy HH:mm:ss";
            return (" [" + DateTime.Now.ToString(format) + "] ");
        }

        public static string GetUniqueID()
        {
            string format = "MMddyyHHmmss";

            return (DateTime.Now.ToString(format));
        }

        public static void Sleep(int Seconds)
        {
            int miliseconds
                = (Seconds * 1000);
            Thread.Sleep(miliseconds);
        }

        public static string Hostname = System.Environment.MachineName.ToString();

        public static string Domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName.ToString();

        public static string MACAddress()
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

        public static string GetLocalIP()
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

        public static string User = System.Environment.UserName.ToString();

        public static string NETframework = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion().ToString();

        public static string ServicePack = System.Environment.OSVersion.ServicePack.ToString();

        public static string OSversion = System.Environment.OSVersion.VersionString;

        public static string OSplatform = System.Environment.OSVersion.Platform.ToString();

        public static string SystemType = System.Environment.Is64BitOperatingSystem.ToString();


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

        public static string LocalHostData()
        {

            Console.WriteLine(Utilis.Debug() + "LocalHostData()");
            string Line = System.Environment.NewLine;
            string Result = null;

            try
            {
                Result =
                           Line +
                           " [ Hostname ] ------------> " + Hostname + Line +
                           " [ Username ] ------------> " + User + Line +
                           " [ MAC Address ] ---------> " + MACAddress() + Line +
                           " [ Default Gateway ] -----> " + DefaulGateway() + Line +
                           " [ Domain ] --------------> " + Domain + Line +
                           " [ I.P. address ] --------> " + GetLocalIP() + Line +
                           " [ DHCP Servers ] --------> " + GetDHCPServers() + Line +
                           " [ DHCP Enabled ] --------> " + DHCPEnabled() + Line +
                           " [ .NET framework ] ------> " + NETframework + Line +
                           " [ OS version ] ----------> " + OSversion + Line +
                           " [ OS platform ] ---------> " + OSplatform + Line +
                           " [ OS is 64 bit type ] ---> " + SystemType + Line +
                           Line;
            }
            catch { }
            Console.WriteLine(Utilis.Debug() + "Result: " + Result);
            return Result;
        }

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
