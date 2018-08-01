using Game;
using Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class GameServer
    {
        private readonly Socket s;
        private readonly Resolver

        public bool StartUp(IPAddress ip, int port)
        {
            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                    ProtocolType.Tcp);
                s.Bind(new IPEndPoint(ip, port));
                s.Listen(10);  // maximal 10 clients in queue
            }
            catch (Exception e)
            {

            }
            for (; ; )
            {
                Communicate(s.Accept());  // waits for connecting clients
            }

        }

        // returns all the received data back to the client
        private void Communicate(Socket clSock)
        {
            try
            {
                var mediator = serviceProvider.GetService<IMediator>();
                var resolver = new Resolver(mediator);

                var processor = new GameProcessor(resolver);
                var boardState = BoardFactory.Standard();
                var gameState = GameFactory.NewGame(boardState, serviceProvider);

                clSock.Shutdown(SocketShutdown.Both);  // close sockets
                clSock.Close();
            }
            catch (Exception e)
            {

            }
        }

        public void RunGame(GameState beginningState)
        {
            GameState gameState = beginningState;
            PlayerInfo currentPlayer;

            while (true)
            {
                if (gameState.GameOver) break;

                currentPlayer = gameState.BoardState.Players[gameState.BoardState.PlayerTurn - 1];

                scope = engine.CreateScope();
                scripts[currentPlayer.ID].Execute(scope);
                Func<string, int> processState = scope.GetVariable<Func<string, int>>("processState");

                int result = processState(GameStateJsonSerializer.Serialize(gameState)); // Call the python script to let it choose what to do
                if (result < 0 || result >= gameState.Actions.Count) throw new InvalidOperationException("Player script returned out-of-bounds response");
                gameState = resolver.Resolve(gameState.Actions[result]).Result;
            }

            Console.WriteLine("Results:");
            Console.WriteLine();
            foreach (var p in gameState.GameEndInfo.Players.OrderByDescending(pi => pi.VictoryPoints))
            {
                Console.WriteLine($"Player {p.Player.ID}: {p.VictoryPoints} points");
            }
        }
    }
}
}
