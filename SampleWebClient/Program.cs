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

                byte[] echo = new byte[1024];
                s.Receive(echo);  // receive the echo message
                Console.WriteLine(Encoding.ASCII.GetString(echo));
            }
            catch (Exception e)
            {

            }
        }
    }
}
