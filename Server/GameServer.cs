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

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            
            var boardState = BoardFactory.Standard();
            var gameState = GameFactory.NewGame(boardState, serviceProvider);

            ProcessRound(gameState, handler);

            /*
            try
            {
                

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
            */
        }

        private void ProcessRound(GameState gameState, Socket socket)
        {
            if (gameState.GameOver)
            {
                SendGameOverMessage(GameStateJsonSerializer.SerializeGameOver(gameState), socket);
            }
            else
            {
                
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

                int result = GetClientAction(GameStateJsonSerializer.Serialize(gameState), clSock); // Ask the client for input
                if (result < 0 || result >= gameState.Actions.Count) throw new InvalidOperationException("Player script returned out-of-bounds response");
                gameState = resolver.Resolve(gameState.Actions[result]).Result;
            }
            
            SendGameOverMessage(GameStateJsonSerializer.SerializeGameOver(gameState), clSock);
        }

        #region gameOver
        private void SendGameOverMessage(string gameStateJson, Socket socket)
        {
            var bytes = Encoding.UTF8.GetBytes(gameStateJson);
            var jsonLength = bytes.Length;
            var lengthBytes = BitConverter.GetBytes(jsonLength);
            var state = new GameOverPayloadStateObject();
            state.workSocket = socket;
            state.endStateBytes = bytes;

            socket.BeginSend(lengthBytes, 0, jsonLength, 0,
                new AsyncCallback(SendGameOverPayloadCallback), state);
        }

        private void SendGameOverPayloadCallback(IAsyncResult ar)
        {
            var state = (GameOverPayloadStateObject)ar.AsyncState;

            // TODO: send again if not all is sent?
            state.workSocket.EndSend(ar);

            state.workSocket.BeginSend(state.endStateBytes, 0, state.endStateBytes.Length, 0,
                new AsyncCallback(GameOverCallback), state.workSocket);
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
            }
        }
        #endregion

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
    }
    
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Persistent game state
        public GameState gameState;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class GameOverPayloadStateObject
    {
        public Socket workSocket = null;
        public byte[] endStateBytes;
    }
}

