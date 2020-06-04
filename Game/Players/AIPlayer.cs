using Game.DTO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Game.Players
{
    public class AIPlayer : IPlayer
    {
        private readonly Process process;
        private readonly NamedPipeServerStream server;
        private readonly BinaryReader pipeReader;
        private readonly BinaryWriter pipeWriter;

        public string Name { get; set; }

        public AIPlayer(string scriptName, string pipeName, string playerName, string pythonExecutable)
        {
            Name = playerName;
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonExecutable,
                Arguments = $"{scriptName} {pipeName}",
                UseShellExecute = false
            };
            process = Process.Start(start);

            // Open the named pipe.
            server = new NamedPipeServerStream(pipeName);

            Console.WriteLine($"Waiting for connection on {pipeName}...");
            server.WaitForConnection();

            Console.WriteLine($"Connection received on {pipeName}.");
            pipeReader = new BinaryReader(server);
            pipeWriter = new BinaryWriter(server);
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
            return new string(pipeReader.ReadChars(len));
        }

        public async Task<int> GetMove(GameState gameState, Resolver resolver)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                WriteToPipe(JsonConvert.SerializeObject(gameState.CloneWithInformationSet(gameState.BoardState.PlayerTurn), serializerSettings));

                while (true)
                {
                    string result = ReadFromPipe();

                    // AI has chosen a move to make - we know because it's an int
                    if (int.TryParse(result, out int chosenMove))
                    {
                        return chosenMove;
                    }
                    // AI wants to determinize a state
                    else if (result == "determinize")
                    {
                        var determinizedState = gameState.CloneAndDeterminize();
                        WriteToPipe(JsonConvert.SerializeObject(determinizedState, serializerSettings));
                    }
                    // AI wants to simulate a move -> payload is a JSON SimulationDTO
                    else
                    {
                        var simulationDTO = JsonConvert.DeserializeObject<SimulationDTO>(result, serializerSettings);
                        var simulatedGameState = await resolver.Resolve(simulationDTO.ToGameAction()).ConfigureAwait(false);
                        simulatedGameState.BoardState.RevealColonistInformation(); // Keep information sets consistent in a determinized game
                        WriteToPipe(JsonConvert.SerializeObject(simulatedGameState, serializerSettings));
                    }
                }
            }
            catch (EndOfStreamException)
            {
                throw new InvalidOperationException("AI Process exited before returning a result");
            }
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
