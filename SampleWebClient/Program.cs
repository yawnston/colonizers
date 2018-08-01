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
                

                while (true)
                {
                    s.Receive(lengthBytes); // receive payload length
                    jsonStringByteLength = BitConverter.ToInt32(lengthBytes);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
