using Game.Players;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Desktop.Services
{
    public class PlayerService : IDisposable
    {
        private readonly ILogger<PlayerService> logger;

        public List<IPlayer> Players { get; set; }

        public PlayerService(ILogger<PlayerService> logger)
        {
            this.logger = logger;
        }

        public void InitPlayers()
        {
            if (Players != null)
            {
                logger.LogWarning("Initializing players while other players exist, disposing previous players.");
                DisposePlayers();
            }

            Players = new List<IPlayer>
            {
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player1Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\HeuristicIntelligence\HeuristicIntelligence.py", "Player2Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player3Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player4Pipe"),
                //new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\ISMCTSIntelligence\ISMCTSIntelligence.py", "Player4Pipe"),
            };
        }

        public void DisposePlayers()
        {
            if (Players != null)
            {
                foreach (var p in Players)
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
