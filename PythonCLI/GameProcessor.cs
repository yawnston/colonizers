﻿using Game;
using Game.Entities;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Game.Serialization;

namespace PythonCLI
{
    class GameProcessor
    {
        private readonly Resolver resolver;

        public GameProcessor(Resolver resolver)
        {
            this.resolver = resolver;
        }

        public void Run(GameState beginningState, IList<CompiledCode> scripts, ScriptEngine engine)
        {
            GameState gameState = beginningState;
            PlayerInfo currentPlayer;
            ScriptScope scope;

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