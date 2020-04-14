namespace Game.Entities
{
    public class Module
    {
        public enum Color { Green, Blue, Red, None }

        public string Name { get; set; }

        public Color Type { get; set; }

        public int BuildCost { get; set; }

        public int VictoryValue { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} Cost: {BuildCost}, Value: {VictoryValue}, Type: {Type.ToString()}";
        }
    }
}
