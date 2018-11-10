using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis
{
    class Generalnformation
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
        /// Getting DCHP Servers
        /// </summary>
        /// <returns></returns>
        public static string GetDHCPServers
        {
            get
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DefaulGateway
        {
            get
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
    }
}
