namespace Piratico
{
    public struct ShipParams
    {
        public int Strength;
        public int CrewAmount;
        public int Gold;
        public int Consumables;

        public ShipParams(int crewAmount = 10, int gold = 100, int consumables = 100)
        {
            Strength = 100;
            CrewAmount = crewAmount;
            Gold = gold;
            Consumables = consumables;
        }
    }
}