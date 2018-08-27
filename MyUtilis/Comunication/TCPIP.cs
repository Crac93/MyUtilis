using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Comunication
{
    public class TCPIP
    {

        public static string SendMessage(string IP, int Port, string Message)
        {
            string Result = null;
            TcpClient client;
            byte[] data;
            NetworkStream stream;
            string responseData;
            Int32 bytes;


            Console.WriteLine(Utilis.Debug() + "SendMessage()");
            Console.WriteLine(Utilis.Debug() + "IP: " + IP);
            Console.WriteLine(Utilis.Debug() + "Port: " + Port);
            Console.WriteLine(Utilis.Debug() + "Message: " + Message);
            try
            {
                client = new TcpClient(IP, Port);
                data = new byte[256];
                data = System.Text.Encoding.ASCII.GetBytes(Message);

                stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                Console.WriteLine(Utilis.Debug() + "Result: Message Sent");

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
                Console.WriteLine(Utilis.Debug() + "Error (ArgumentNullException): " + e.ToString());
                Result = Utilis.Debug() + "Error (ArgumentNullException): " + e.ToString();
            }

            catch (SocketException e)
            {
                Console.WriteLine(Utilis.Debug() + "Error (SocketException): " + e.ToString());
                Result = Utilis.Debug() + "Error (SocketException): " + e.ToString();
            }
            return Result;
        }



        public static void ListenTCP(string ipAddress, Int32 port)
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
                    Console.Write(Utilis.Debug() + "Waiting for a connection... ");
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine(Utilis.Debug() + "Connected!");
                    data = null;
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int i;
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine(Utilis.Debug() + "Received: {0}", data);
                        // Process the data sent by the client.
                        data = data.ToUpper();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine(Utilis.Debug() + "Sent: {0}", data);
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
                server.Stop();
            }

        }
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
