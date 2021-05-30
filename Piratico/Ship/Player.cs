using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Piratico
{
    public class Player : Ship
    {
        public static readonly Point PlayerStartPosition =
            new(MapCell.MapSize.Width / 2, MapCell.MapSize.Height / 2);

        private readonly Timer playerStepChecker = new() {Interval = 50};

        public Player(Size spriteSize, PictureBox parentPictureBox, Game game) :
            base(spriteSize, PlayerStartPosition, parentPictureBox, game)
        {
            ShipParams = new ShipParams();
            StateSprites = new Image[]
                {Resources.PlayerShip, Resources.PlayerShip_66, Resources.PlayerShip_33, Resources.PlayerShip_0};
            CheckCurrentShipState();
            playerStepChecker.Tick += (_, _) =>
            {
                if (!StepEnded) return;
                Thread.Sleep(100);
                Game.LetEnemiesDoTheirMove();
                StepEnded = false;
            };
            playerStepChecker.Start();
        }

        public bool IsMoving { get; set; }

        private bool StepEnded { get; set; }

        public void StartMovement(Action playerMoves)
        {
            if (Game.PlayerDoingSomething) return;
            IsMoving = true;
            var timer = new Timer {Interval = 300};
            timer.Tick += (_, _) =>
            {
                playerMoves();
                if (IsMoving) return;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        public (int strength, int crewAmount, int consumables, int gold, int upgradeCost) GetShipParams()
        {
            return (ShipParams.Strength, ShipParams.CrewAmount, ShipParams.Consumables, ShipParams.Gold,
                ShipParams.ConsumablesPerLevel);
        }

        public void UpgradeStats()
        {
            if (!ShipParams.AbleToUpgrade()) return;
            ShipParams.Upgrade();
            CheckCurrentShipState();
            Game.UpdatePlayerResourcesUI();
            EndStep();
        }

        public void EndStep()
        {
            Game.ExitScoutModeManually();
            StepEnded = true;
        }
    }
}