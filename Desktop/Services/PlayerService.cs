using Game.Players;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Desktop.Services
{
    public class PlayerService : IDisposable
    {
        private readonly ILogger<PlayerService> logger;
        private readonly PythonExecutableService pythonExecutableService;

        public List<IPlayer> Players { get; set; }

        public PlayerService(ILogger<PlayerService> logger,
            PythonExecutableService pythonExecutableService)
        {
            this.logger = logger;
            this.pythonExecutableService = pythonExecutableService;
        }

        public void InitPlayers(string[] playerNames)
        {
            if (Players != null)
            {
                logger.LogWarning("Initializing players while other players exist, disposing previous players.");
                DisposePlayers();
            }

            string[] pipeNames = new[] { "Player1Pipe", "Player2Pipe", "Player3Pipe", "Player4Pipe" };

            Players = new List<IPlayer>();
            for (int i = 0; i < pipeNames.Length; i++)
            {
                Players.Add(CreatePlayer(playerNames[i], pipeNames[i]));
            }
        }

        private IPlayer CreatePlayer(string playerName, string pipeName)
        {
            if (playerName == "Human Player")
            {
                // Human players do not use pipes, therefore pipeName is not used here
                return new HumanPlayer { Name = "Human Player" };
            }

            // Else it is an AI player
            return CreateAIPlayer(playerName, pipeName);
        }

        private AIPlayer CreateAIPlayer(string playerName, string pipeName)
        {
            string scriptFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");
            string scriptPath;

            // Try looking for the AI as a top-level script
            string[] aiScript = Directory.GetFiles(scriptFolderPath, $"{playerName}.py");
            if (aiScript.Length != 0)
            {
                scriptPath = aiScript[0];
            }
            else
            {
                // Try looking for the AI as a folder
                // AI folders must have the name "<Name>Intelligence" and they must contain a script named "main.py"
                // This script is the AI's entrypoint which is called by the game engine
                string[] aiDir = Directory.GetDirectories(scriptFolderPath, playerName);
                if (aiDir.Length != 0)
                {
                    scriptPath = Path.Combine(aiDir[0], "main.py");
                    if (!File.Exists(scriptPath))
                    {
                        throw new InvalidOperationException($"AI folder {playerName} was found, but entrypoint \"main.py\" script was not found in the folder.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"The AI named {playerName} was not found.");
                }
            }

            // All AI names must follow the pattern "<Name>Intelligence"
            string name = playerName.Substring(0, playerName.LastIndexOf("Intelligence"));

            return new AIPlayer(scriptPath, pipeName, name, pythonExecutableService.GetPath());
        }

        public void DisposePlayers()
        {
            if (Players != null)
            {
                foreach (IPlayer p in Players)
                {
                    p.Dispose();
                }
            }

            Players = null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DisposePlayers();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PlayerService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
