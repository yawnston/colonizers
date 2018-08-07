using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SampleWebClient
{
    class Program
    {
        const string API_CONFIG_SECTION = "server-info";
        const string API_CONFIG_NAME_IP = "ip";
        const string API_CONFIG_NAME_PORT = "port";

        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            LoadConfig();

            Socket s = null;
            try
            {
                // connect to the server
                s = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                s.ReceiveTimeout = 5000;
                s.SendTimeout = 5000;
                s.Connect(new IPEndPoint(
                    IPAddress.Parse(configuration.GetSection(API_CONFIG_SECTION)[API_CONFIG_NAME_IP]),
                    int.Parse(configuration.GetSection(API_CONFIG_SECTION)[API_CONFIG_NAME_PORT])));

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
                    if (gameState["GameOver"].Equals(new JValue(true))) // end the processing loop
                    {
                        Console.WriteLine(gameState["GameEndInfo"].ToString(Newtonsoft.Json.Formatting.Indented));
                        s.Shutdown(SocketShutdown.Both);
                        return;
                    }
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
            }
            catch (SocketException)
            {
                Console.WriteLine("Error: Timeout exceeded while waiting for server");
            }
            finally
            {
                s.Close();
            }
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }

    }
}
