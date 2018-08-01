using System;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using Game.ActionGetters;
using Game;
using Newtonsoft.Json;
using Game.Entities;
using System.Linq;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddMediatR(typeof(Resolver).GetTypeInfo().Assembly)
            .AddScoped<IMediator, Mediator>()
            .AddScoped<IColonistPickGetter, ColonistPickGetter>()
            .AddScoped<IDrawGetter, DrawGetter>()
            .AddScoped<IDiscardGetter, DiscardGetter>()
            .AddScoped<IPowerGetter, PowerGetter>()
            .AddScoped<IBuildGetter, BuildGetter>()
            .AddScoped<IGameEndGetter, GameEndGetter>()
            .BuildServiceProvider();

            Console.WriteLine("Colonizers v1.0");
            Console.WriteLine("Created by Daniel Crha");

            var mediator = serviceProvider.GetService<IMediator>();
            var resolver = new Resolver(mediator);

            GameState gameState; IRequest<GameState> action; PlayerInfo currentPlayer;
            var boardState = BoardFactory.Standard();
            gameState = GameFactory.NewGame(boardState, serviceProvider);

            while (true)
            {
                currentPlayer = gameState.BoardState.Players[gameState.BoardState.PlayerTurn - 1];
                Console.WriteLine($"+++ PLAYER {currentPlayer.ID}");
                Console.WriteLine($"Your Omnium: {currentPlayer.Omnium}");
                if (gameState.BoardState.GamePhase != BoardState.Phase.ColonistPick) Console.WriteLine($"Your Colonist: {currentPlayer.Colonist}");
                Console.WriteLine("Your Hand:");
                foreach(var m in currentPlayer.Hand)
                {
                    Console.WriteLine($"{m.ToString()}");
                }
                Console.WriteLine("Your Colony:");
                foreach (var m in currentPlayer.Colony)
                {
                    Console.WriteLine($"{m.ToString()}");
                }

                Console.WriteLine($"OTHER PLAYERS:");
                foreach(var p in (from pl in gameState.BoardState.Players where pl != currentPlayer select pl))
                {
                    Console.WriteLine($"*** PLAYER {p.ID}");
                    Console.WriteLine($"Omnium: {p.Omnium}");
                    Console.WriteLine($"Hand: {p.Hand.Count} modules");
                    Console.WriteLine("Colony:");
                    foreach (var m in p.Colony)
                    {
                        Console.WriteLine($"{m.ToString()}");
                    }
                }

                Console.WriteLine("-------------------------------------");

                int i = 0;
                foreach(var a in gameState.Actions)
                {
                    Console.WriteLine($"({i++}) {a.ToString()}");
                }
                bool parseSuccessful; int response;
                while (true) // loop until the user inputs a valid response
                {
                    parseSuccessful = int.TryParse(Console.ReadLine(), out response);
                    if (!parseSuccessful || response < 0 || response >= gameState.Actions.Count)
                    {
                        Console.WriteLine("Invalid action. Please enter a valid action number (starting at 0 from the top).");
                    }
                    else break;
                }
                action = gameState.Actions[response];
                gameState = resolver.Resolve(action).Result;
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                if (gameState.GameOver) break;
            }

            Console.WriteLine("Game over");
            Console.WriteLine("Results:");
            Console.WriteLine();
            foreach(var p in gameState.GameEndInfo.Players.OrderByDescending(pi => pi.VictoryPoints))
            {
                Console.WriteLine($"Player {p.Player.ID}: {p.VictoryPoints} points");
            }
        }
    }
}
