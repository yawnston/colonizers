using Game;
using System;
using System.Collections.Generic;

namespace Experiments
{
    public sealed class ExperimentResult
    {
        public List<PlayerExperimentResult> Players { get; set; } = new List<PlayerExperimentResult>();

        public TimeSpan Duration { get; set; }
    }

    public sealed class PlayerExperimentResult
    {
        public PlayerEndInfo PlayerEndInfo { get; set; }

        public string Name { get; set; }
    }
}
