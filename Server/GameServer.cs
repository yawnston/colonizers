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

                RunGame(gameState, clSock);

                clSock.Shutdown(SocketShutdown.Both); // close sockets
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // log the exception to the console
            }
            finally
            {
                clSock.Close();
            }
        }

        private void RunGame(GameState beginningState, Socket clSock)
        {
            GameState gameState = beginningState;
            PlayerInfo currentPlayer;

            while (true)
            {
                if (gameState.GameOver) break;

                currentPlayer = gameState.BoardState.Players[gameState.BoardState.PlayerTurn - 1];

                int result = GetClientAction(GameStateJsonSerializer.Serialize(gameState), clSock); // Call the python script to let it choose what to do
                if (result < 0 || result >= gameState.Actions.Count) throw new InvalidOperationException("Player script returned out-of-bounds response");
                gameState = resolver.Resolve(gameState.Actions[result]).Result;
            }
            
            SendGameOverMessage(GameStateJsonSerializer.SerializeGameOver(gameState), clSock);
        }

        private int GetClientAction(string gameStateJson, Socket clSock)
        {
            var bytes = Encoding.UTF8.GetBytes(gameStateJson);
            var jsonLength = bytes.Length;
            var lengthBytes = BitConverter.GetBytes(jsonLength);
            
            int count = 0;
            while (count < lengthBytes.Length)
            {
                count += clSock.Send(
                    lengthBytes,
                    count,
                    lengthBytes.Length - count,
                    SocketFlags.None);
            }

            int sent = 0;
            while (sent < bytes.Length)
            {
                sent += clSock.Send(
                    bytes,
                    sent,
                    bytes.Length - sent,
                    SocketFlags.None);
            }

            var responseBytes = new Byte[4];
            clSock.Receive(responseBytes);
            return BitConverter.ToInt32(responseBytes);
        }

        private void SendGameOverMessage(string gameStateJson, Socket clSock)
        {
            var bytes = Encoding.UTF8.GetBytes(gameStateJson);
            var jsonLength = bytes.Length;
            var lengthBytes = BitConverter.GetBytes(jsonLength);

            int count = 0;
            while (count < lengthBytes.Length)
            {
                count += clSock.Send(
                    lengthBytes,
                    count,
                    lengthBytes.Length - count,
                    SocketFlags.None);
            }

            int sent = 0;
            while (sent < bytes.Length)
            {
                sent += clSock.Send(
                    bytes,
                    sent,
                    bytes.Length - sent,
                    SocketFlags.None);
            }
        }
    }
}

