using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public abstract class Module
    {
        public enum Color { Green, Blue, Red }

        public Color Type { get; set; }

        public int BuildCost { get; set; }

        public int VictoryValue { get; set; }
    }
}
