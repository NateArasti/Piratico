using System;
using System.Drawing;
using System.Windows.Forms;
using Thread = System.Threading.Thread;
using Timer = System.Windows.Forms.Timer;

namespace Piratico
{
    public class Player : Ship
    {
        public static readonly Point PlayerStartPosition =
            new Point(MapCell.MapSize.Width / 2, MapCell.MapSize.Height / 2);

        public bool IsMoving;
        public bool StepEnded;
        private readonly Timer playerStepChecker = new Timer {Interval = 50};

        public Player(Image sprite, Size spriteSize, PictureBox parentPictureBox, GameModel gameModel) : 
            base(sprite, spriteSize, PlayerStartPosition, parentPictureBox, gameModel)
        {
            ShipParams = new ShipParams();
            playerStepChecker.Tick += (sender, args) =>
            {
                if (!StepEnded) return;
                Thread.Sleep(100);
                GameModel.LetEnemiesDoTheirMove();
                StepEnded = false;
            };
            playerStepChecker.Start();
        }

        public void StartMovement(Action playerMoves)
        {
            if (GameModel.PlayerDoingSomething) return;
            IsMoving = true;
            var timer = new Timer { Interval = 300 };
            timer.Tick += (sender, args) =>
            {
                playerMoves();
                if (IsMoving) return;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
