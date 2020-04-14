using Game.DTO;
using Game.Serialization;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Players
{
    public class AIPlayer : IPlayer
    {
        private readonly Process process;
        private readonly NamedPipeServerStream server;
        private readonly BinaryReader pipeReader;
        private readonly BinaryWriter pipeWriter;

        public AIPlayer(string scriptName)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\danie\Anaconda3\python.exe",
                Arguments = $"{scriptName}",
                UseShellExecute = false
            };
            process = Process.Start(start);

            // Open the named pipe.
            server = new NamedPipeServerStream("NPtest");

            Console.WriteLine("Waiting for connection...");
            server.WaitForConnection();

            Console.WriteLine("Connected.");
            pipeReader = new BinaryReader(server);
            pipeWriter = new BinaryWriter(server);
        }

        void run_server()
        {
            while (true)
            {
                try
                {
                    var wr = "testAA";  // Just for fun

                    var buf = Encoding.ASCII.GetBytes(wr);     // Get ASCII byte array     
                    pipeWriter.Write((uint)buf.Length);                // Write string length
                    pipeWriter.Write(buf);                              // Write string
                    Console.WriteLine("Wrote: \"{0}\"", wr);

                    var len = (int)pipeReader.ReadUInt32();            // Read string length
                    var str = new string(pipeReader.ReadChars(len));    // Read string

                    Console.WriteLine("Read: \"{0}\"", str);

                    Thread.Sleep(5000);
                }
                catch (EndOfStreamException)
                {
                    break;                    // When client disconnects
                }
            }

            Console.WriteLine("Client disconnected.");
            server.Close();
            server.Dispose();
        }

        private void WriteToPipe(string stringToWrite)
        {
            var buf = Encoding.ASCII.GetBytes(stringToWrite);
            pipeWriter.Write((uint)buf.Length);
            pipeWriter.Write(buf);
        }

        private string ReadFromPipe()
        {
            var len = (int)pipeReader.ReadUInt32();
            var str = new string(pipeReader.ReadChars(len));
            //Console.WriteLine($"C# Read: {str}");
            return str;
        }

        public async Task<int> GetMove(GameState gameState, Resolver resolver)
        {
            WriteToPipe(GameStateJsonSerializer.Serialize(gameState));

            while (true)
            {
                string result = ReadFromPipe();
                // TODO: handle end of pipe?

                // AI has chosen a move to make - TODO: parse better?
                if (int.TryParse(result, out int chosenMove))
                {
                    return chosenMove;
                }

                // AI needs to simulate a move - it sends a game state and an action
                // This action is the only one left in the Actions list
                var simulationDTO = JsonConvert.DeserializeObject<SimulationDTO>(result);
                var simulatedGameState = await resolver.Resolve(simulationDTO.ToGameAction()).ConfigureAwait(false);
                await process.StandardInput.WriteLineAsync("TODO").ConfigureAwait(false);
                await process.StandardInput.FlushAsync().ConfigureAwait(false);

                // TODO: add determinization of information sets
            }

            throw new InvalidOperationException("AI Process exited before returning a result");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    process.Dispose();
                    server.Close();
                    server.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
