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
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class GameServer
    {
        // thread signal  
        private static ManualResetEvent allDone = new ManualResetEvent(false);

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
                Console.WriteLine($"Starting up game server at {ip.ToString()}:{port}");
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                    ProtocolType.Tcp);
                s.Bind(new IPEndPoint(ip, port));
                s.Listen(100);  // max 100 clients in queue
            }
            catch (Exception e)
            {
                Console.WriteLine($"Startup failed: {e}");
                return false;
            }
            for (; ; )
            {
                allDone.Reset();

                s.BeginAccept(new AsyncCallback(Communicate), s);  // waits for connecting clients

                allDone.WaitOne();
            }
        }

        // create a new game for the connecting client
        private void Communicate(IAsyncResult ar)
        {
            allDone.Set();

            Console.WriteLine("Accepting new client");

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            Console.WriteLine($"Accept successful: {handler.RemoteEndPoint.ToString()}");

            var boardState = BoardFactory.Standard();
            var gameState = GameFactory.NewGame(boardState, serviceProvider);

            try
            {
                ProcessRound(gameState, handler);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                handler.Close();
            }
        }

        private void ProcessRound(GameState gameState, Socket socket)
        {
            if (gameState.GameOver)
            {
                SendGameOverMessage(GameStateJsonSerializer.SerializeGameOver(gameState), socket);
            }
            else
            {
                GetClientAction(gameState, socket);
            }
        }

        #region gameOver
        private void SendGameOverMessage(string gameStateJson, Socket socket)
        {
            var bytes = Encoding.UTF8.GetBytes(gameStateJson);
            var jsonLength = bytes.Length;
            var lengthBytes = BitConverter.GetBytes(jsonLength);
            var state = new GameOverPayloadState();
            state.workSocket = socket;
            state.endStateBytes = bytes;

            try
            {
                socket.BeginSend(lengthBytes, 0, lengthBytes.Length, 0,
                        new AsyncCallback(SendGameOverPayloadCallback), state);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                socket.Close();
            }
        }

        private void SendGameOverPayloadCallback(IAsyncResult ar)
        {
            var state = (GameOverPayloadState)ar.AsyncState;

            try
            {
                state.workSocket.EndSend(ar);

                state.workSocket.BeginSend(state.endStateBytes, 0, state.endStateBytes.Length, 0,
                    new AsyncCallback(GameOverCallback), state.workSocket);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                state.workSocket.Close();
            }
        }

        private void GameOverCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine($"Sent {bytesSent} bytes of game over info to client. Closing connection.");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ((Socket)ar.AsyncState).Close();
            }
        }
        #endregion

        #region roundProcessing
        private void GetClientAction(GameState gameState, Socket socket)
        {
            var gameStateJson = GameStateJsonSerializer.Serialize(gameState);
            var bytes = Encoding.UTF8.GetBytes(gameStateJson);
            var jsonLength = bytes.Length;
            var lengthBytes = BitConverter.GetBytes(jsonLength);

            var state = new GetClientActionPayloadState();
            state.workSocket = socket;
            state.gameState = gameState;
            state.gameStateBytes = bytes;

            try
            {
                socket.BeginSend(lengthBytes, 0, lengthBytes.Length, 0,
                        new AsyncCallback(SendGameStatePayloadCallback), state);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                socket.Close();
            }
        }

        private void SendGameStatePayloadCallback(IAsyncResult ar)
        {
            var state = (GetClientActionPayloadState)ar.AsyncState;
            
            var newState = new GetClientActionGameState();
            newState.workSocket = state.workSocket;
            newState.gameState = state.gameState;

            try
            {
                state.workSocket.EndSend(ar);
                state.workSocket.BeginSend(state.gameStateBytes, 0, state.gameStateBytes.Length, 0,
                    new AsyncCallback(GetClientResponseCallback), newState);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                state.workSocket.Close();
            }
        }

        private void GetClientResponseCallback(IAsyncResult ar)
        {
            var state = (GetClientActionGameState)ar.AsyncState;

            var newState = new GetClientActionResponseState();
            newState.workSocket = state.workSocket;
            newState.gameState = state.gameState;

            try
            {
                state.workSocket.BeginReceive(newState.buffer, 0, GetClientActionResponseState.BufferSize, 0,
                        new AsyncCallback(ProcessClientResponseCallback), newState);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                state.workSocket.Close();
            }
        }

        private void ProcessClientResponseCallback(IAsyncResult ar)
        {
            var state = (GetClientActionResponseState)ar.AsyncState;
            var response = BitConverter.ToInt32(state.buffer);
            if (response < 0 || response >= state.gameState.Actions.Count) throw new InvalidOperationException("Player script returned out-of-bounds response");
            var newState = resolver.Resolve(state.gameState.Actions[response]).Result;
            ProcessRound(newState, state.workSocket);
        }
        #endregion
    }
}