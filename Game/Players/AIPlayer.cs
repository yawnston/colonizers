﻿using Game.DTO;
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

        public AIPlayer(string scriptName, string pipeName)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\danie\Anaconda3\python.exe",
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
                WriteToPipe(JsonConvert.SerializeObject(gameState.CloneWithInformationSet(gameState.BoardState.PlayerTurn)));

                while (true)
                {
                    string result = ReadFromPipe();

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
