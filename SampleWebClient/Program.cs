using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SampleWebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // connect to the server
                Socket s = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                s.Connect(new IPEndPoint(IPAddress.Loopback, 4141));

                int jsonStringByteLength;
                byte[] lengthBytes = new byte[4];
                //s.Send(BitConverter.GetBytes(42)); // initialize communication with server

                while (true)
                {
                    int count = 0;
                    do // receive the json string payload
                    {
                        count += s.Receive(
                            lengthBytes,
                            count,
                            lengthBytes.Length - count,
                            SocketFlags.None);
                    } while (count < lengthBytes.Length);

                    jsonStringByteLength = BitConverter.ToInt32(lengthBytes);

                    count = 0;
                    var bytes = new byte[jsonStringByteLength];
                    do // receive the json string payload
                    {
                        count += s.Receive(
                            bytes,
                            count,
                            bytes.Length - count,
                            SocketFlags.None);
                    } while (count < bytes.Length);
                    var jsonString = Encoding.UTF8.GetString(bytes);

                    Console.WriteLine(JValue.Parse(jsonString).ToString(Newtonsoft.Json.Formatting.Indented));

                    int response = int.Parse(Console.ReadLine());
                    var responseBytes = BitConverter.GetBytes(response);
                    s.Send(responseBytes);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Error: Timeout exceeded while waiting for server");
            }
        }
    }
}
