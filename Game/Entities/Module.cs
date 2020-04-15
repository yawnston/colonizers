using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Entities
{
    public class Module
    {
        public enum Color { Green, Blue, Red, None }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Color Type { get; set; }

        public int BuildCost { get; set; }

        public int VictoryValue { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} Cost: {BuildCost}, Value: {VictoryValue}, Type: {Type.ToString()}";
        }

        /// <summary>
        /// Module which represents a module that a player doesn't know about
        /// </summary>
        public static Module Unknown { get; } = new Module
        {
            Name = "Unknown",
            BuildCost = -1,
            Type = Color.None,
            VictoryValue = -1
        };
    }
}
