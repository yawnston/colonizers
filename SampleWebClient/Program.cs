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
                s.ReceiveTimeout = 5000;
                s.SendTimeout = 5000;
                s.Connect(new IPEndPoint(IPAddress.Loopback, 4141));

                int jsonStringByteLength;
                byte[] lengthBytes = new byte[4];

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
                    var gameState = JObject.Parse(jsonString);
                    Console.WriteLine(gameState.ToString(Newtonsoft.Json.Formatting.Indented));
                    var actionCount = ((JArray)gameState["Actions"]).Count;

                    int response; bool parseSuccessful;
                    while (true) // loop until the user inputs a valid response
                    {
                        parseSuccessful = int.TryParse(Console.ReadLine(), out response);
                        if (!parseSuccessful || response < 0 || response >= actionCount)
                        {
                            Console.WriteLine("Invalid action. Please enter a valid action number (starting at 0 from the top).");
                        }
                        else break;
                    }
                    var responseBytes = BitConverter.GetBytes(response);
                    s.Send(responseBytes);
                }
                
                // TODO: end game
            }
            catch (SocketException)
            {
                Console.WriteLine("Error: Timeout exceeded while waiting for server");
            }
        }
    }
}
