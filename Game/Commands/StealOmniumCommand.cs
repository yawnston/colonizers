using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class StealOmniumCommand : IRequest<GameState>
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }
        public Colonist Target { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return $@"Steal Omnium: 
    Target: {Target.ToString()}";
        }
    }
}
