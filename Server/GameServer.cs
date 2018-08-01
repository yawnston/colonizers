using Game;
using Game.Entities;
using Game.Serialization;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class GameServer
    {
        private Socket s;
        private readonly Resolver resolver;
        private readonly IMediator mediator;
        private readonly IServiceProvider serviceProvider;

        public GameServer(IMediator mediator, IServiceProvider serviceProvider)
        {
            this.mediator = mediator;
            this.serviceProvider = serviceProvider;
            resolver = new Resolver(mediator);
        }

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

        // create a new game for the connecting client
        private void Communicate(Socket clSock)
        {
            try
            {
                var boardState = BoardFactory.Standard();
                var gameState = GameFactory.NewGame(boardState, serviceProvider);

                RunGame(gameState);

                clSock.Shutdown(SocketShutdown.Both);  // close sockets
                clSock.Close();
            }
            catch (Exception e)
            {
                // TODO
            }
        }

        private void RunGame(GameState beginningState)
        {
            GameState gameState = beginningState;
            PlayerInfo currentPlayer;

            while (true)
            {
                if (gameState.GameOver) break;

                currentPlayer = gameState.BoardState.Players[gameState.BoardState.PlayerTurn - 1];

                int result = GetClientAction(GameStateJsonSerializer.Serialize(gameState)); // Call the python script to let it choose what to do
                if (result < 0 || result >= gameState.Actions.Count) throw new InvalidOperationException("Player script returned out-of-bounds response");
                gameState = resolver.Resolve(gameState.Actions[result]).Result;
            }

            SendGameOverMessage(GameStateJsonSerializer.SerializeGameOver(gameState));
        }

        private int GetClientAction(string gameStateJson)
        {
            var bytes = Encoding.UTF8.GetBytes(gameStateJson);
            var jsonLength = bytes.Length;
            s.Send(BitConverter.GetBytes(jsonLength)); // send the amount of bytes the json string takes
            int sent = 0;
            while (sent < bytes.Length) // if you are brave you can do it all in the condition.
            {
                sent += s.Send(
                    bytes,
                    sent,
                    bytes.Length - sent, // This can be anything as long as it doesn't overflow the buffer, bytes.
                    SocketFlags.None);
            }

            var responseBytes = new Byte[4];
            s.Receive(responseBytes);
            return BitConverter.ToInt32(responseBytes);
        }

        private void SendGameOverMessage(string gameStateJson)
        {

        }
    }
}

