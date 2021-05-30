namespace Piratico
{
    public class ShipParams
    {
        private const double Modifier = 1.3;

        private int maxHealth = 100;

        public ShipParams(int crewAmount = 10, int gold = 100, int consumables = 100, int maxDamage = 25)
        {
            Strength = 100;
            MaxCrewAmount = CrewAmount = crewAmount;
            Gold = gold;
            Consumables = consumables;
            MaxDamage = maxDamage;
        }

        public int ConsumablesPerLevel { get; private set; } = 120;
        public bool IsCollected { get; private set; }
        public int Strength { get; private set; }
        public int CrewAmount { get; private set; }
        public int MaxCrewAmount { get; private set; }
        public int MaxDamage { get; private set; }
        public int Gold { get; private set; }
        public int Consumables { get; private set; }

        public bool AbleToUpgrade()
        {
            return ConsumablesPerLevel < Consumables;
        }

        public void Upgrade()
        {
            if (!AbleToUpgrade()) return;
            maxHealth = (int) (Modifier * maxHealth);
            MaxCrewAmount = (int) (Modifier * MaxCrewAmount);
            MaxDamage = (int) (Modifier * MaxDamage);
            Strength = 100;
            Consumables -= ConsumablesPerLevel;
            ConsumablesPerLevel = (int) (Modifier * ConsumablesPerLevel);
        }

        public void CalculateDamageEffects(int damage)
        {
            var modifier = 1 - damage / 200.0;
            Strength = (int) (100 * (Strength / 100.0 * maxHealth - damage) / maxHealth);
            CrewAmount = (int) (CrewAmount * modifier);
            if (CrewAmount == 0) CrewAmount = 1;
            Consumables = (int) (Consumables * modifier);
            Gold = (int) (Gold * modifier);
        }

        public void AddCollectedResources(int crewAmount, int consumables, int gold)
        {
            CrewAmount += crewAmount;
            if (CrewAmount > MaxCrewAmount) CrewAmount = MaxCrewAmount;
            Consumables += consumables;
            Gold += gold;
        }

        public void MarkCollected()
        {
            IsCollected = true;
        }
    }
}