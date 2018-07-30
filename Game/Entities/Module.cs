using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public class Module
    {
        public enum Color { Green, Blue, Red, None }

        public Color Type { get; set; }

        public int BuildCost { get; set; }

        public int VictoryValue { get; set; }

        public override string ToString()
        {
            return $"Cost: {BuildCost}, Value: {VictoryValue}, Type: {Type.ToString()}";
        }
    }
}
