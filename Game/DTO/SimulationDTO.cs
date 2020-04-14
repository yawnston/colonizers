using Game.Commands;
using Game.Exceptions;
using System;

namespace Game.DTO
{
    /// <summary>
    /// Object sent by an AI when it wants to simulate a game step
    /// </summary>
    public sealed class SimulationDTO
    {
        public BoardState BoardState { get; set; }

        public string Action { get; set; }

        public IGameAction ToGameAction()
        {
            string actionName, actionTarget;
            int firstSpaceIndex = Action.IndexOf(' ');
            if (firstSpaceIndex > -1)
            {
                actionName = Action.Substring(0, firstSpaceIndex);
                actionTarget = Action.Substring(firstSpaceIndex).Trim();
            }
            else
            {
                actionName = Action;
                actionTarget = null;
            }

            GameConstants.Action action = (GameConstants.Action)Enum.Parse(typeof(GameConstants.Action), actionName);

            switch (action)
            {
                case GameConstants.Action.BuildModule:
                    return new BuildModuleCommand { BoardState = BoardState, Module = actionTarget };
                case GameConstants.Action.BuildNothing:
                    return new BuildNothingCommand { BoardState = BoardState };
                case GameConstants.Action.ColonistPick:
                    return new ColonistPickCommand { BoardState = BoardState, Colonist = actionTarget };
                case GameConstants.Action.DoNothing:
                    return new DoNothingCommand { BoardState = BoardState };
                case GameConstants.Action.DrawModules:
                    return new DrawModulesCommand { BoardState = BoardState };
                case GameConstants.Action.KeepModule:
                    return new KeepModuleCommand { BoardState = BoardState, Module = actionTarget };
                case GameConstants.Action.StealOmnium:
                    return new StealOmniumCommand { BoardState = BoardState, Target = actionTarget };
                case GameConstants.Action.SwapHands:
                    return new SwapHandsCommand { BoardState = BoardState, Target = actionTarget };
                case GameConstants.Action.TakeOmnium:
                    return new TakeOmniumCommand { BoardState = BoardState };
                default:
                    throw new InvalidPlayerResponseException($"Invalid player response: {Action}");
            }
        }
    }
}
