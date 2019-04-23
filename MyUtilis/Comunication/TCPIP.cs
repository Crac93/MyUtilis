using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Comunication
{
    /// <summary>
    /// Class to comunicate TCP/IP
    /// </summary>
    public class TCPIP
    {
        /// <summary>
        /// Send a message via TCP/IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SendMessage(string ip, int port, string message)
        {
            string Result = null;
            TcpClient client;
            byte[] data;
            NetworkStream stream;
            string responseData;
            Int32 bytes;

            try
            {
                client = new TcpClient(ip, port);
                data = new byte[256];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                data = new byte[256];
                responseData = string.Empty;

                bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Result = responseData;

                stream.Close();
                client.Close();

            }

            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }

            catch (SocketException e)
            {
                throw new Exception(e.Message);
            }
            return Result;
        }


        /// <summary>
        /// Listening TCP
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public static void ListenTcp(string ipAddress, Int32 port)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.             
                IPAddress localAddr = IPAddress.Parse(ipAddress);
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                // Start listening for client requests.
                server.Start();
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                string data = null;
                // Enter the listening loop.
                while (true)
                {
                  
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    data = null;
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int i;
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        // Process the data sent by the client.
                        data = data.ToUpper();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                    }
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server?.Stop();
            }

        }
        /// <summary>
        /// Send a file via TCP
        /// </summary>
        /// <param name="pathFile"></param>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        public static void SendFileTCP(string pathFile, string hostname, int port)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(hostname);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            // Create a TCP socket.
            Socket client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint.
            client.Connect(ipEndPoint);
            // Send file fileName to the remote host with preBuffer and postBuffer data.
            // There is a text file test.txt located in the root directory.
            string fileName = pathFile;

            // Create the preBuffer data.
            string string1 = String.Format("This is text data that precedes the file.{0}", Environment.NewLine);
            byte[] preBuf = Encoding.ASCII.GetBytes(string1);

            // Create the postBuffer data.
            string string2 = String.Format("This is text data that will follow the file.{0}", Environment.NewLine);
            byte[] postBuf = Encoding.ASCII.GetBytes(string2);

            //Send file fileName with buffers and default flags to the remote device.
            Console.WriteLine("Sending {0} with buffers to the host.{1}", fileName, Environment.NewLine);
            client.SendFile(fileName, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);

            // Release the socket.
            client.Shutdown(SocketShutdown.Both);
            client.Close();

        }

    }
}
