using Game;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class GetClientActionResponseState
    {
        public Socket workSocket = null;
        public GameState gameState;
        public const int BufferSize = 4;
        public byte[] buffer = new byte[BufferSize];
    }

    public class GameOverPayloadState
    {
        public Socket workSocket = null;
        public byte[] endStateBytes;
    }

    public class GetClientActionPayloadState
    {
        public Socket workSocket = null;
        public GameState gameState;
        public byte[] gameStateBytes;
    }

    public class GetClientActionGameState
    {
        public Socket workSocket = null;
        public GameState gameState;
    }
}
