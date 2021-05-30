using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public partial class PiraticoGame : Form
    {
        private readonly Game game;
        public readonly ScoutMode ScoutMode;
        public readonly ShootMode ShootMode;

        public PiraticoGame()
        {
            InitializeComponent();
            game = new Game(this);
            var scoutButtons = new Dictionary<Direction, Button>
            {
                [Direction.Up] = UpButton,
                [Direction.Down] = DownButton,
                [Direction.Left] = LeftButton,
                [Direction.Right] = RightButton
            };
            ScoutMode = new ScoutMode(new ScoutData(Scout, scoutButtons), game);
            ShootMode = new ShootMode(game, Shoot);
            DrawMapCell(game.CurrentMapCell.MapCellControlPanel);
            InitializePlayerParamsUI();
            Skip.Click += (_, _) =>
            {
                if (!game.PlayerDoingSomething && !game.EnemiesTurn)
                    game.Player.EndStep();
            };
            InitializeIntroUI();
            InitializeEndGameUI();
        }

        private void InitializeIntroUI()
        {
            StartButton.Parent = IntroPanel;
            IntroText.Parent = IntroPanel;
            IntroPanel.Parent = this;
            IntroPanel.BringToFront();
            StartButton.Click += (_, _) => Controls.Remove(IntroPanel);
        }

        private void InitializeEndGameUI()
        {
            RestartButton.Parent = EndGamePanel;
            EndGameText.Parent = EndGamePanel;
            EndGamePanel.Parent = this;
            Controls.Remove(EndGamePanel);
            RestartButton.Click += (_, _) => Application.Restart();
        }

        private void InitializePlayerParamsUI()
        {
            DrawPlayerResources();
            foreach (var control in new Control[] { Strength, Crew, Gold, Consumables, Upgrade, UpgradeCost })
            {
                control.BringToFront();
            }
            Upgrade.Click += (_, _) => game.Player.UpgradeStats();
        }

        public void DrawPlayerResources()
        {
            var (strength, crewAmount, consumables, gold, upgradeCost) = game.Player.GetShipParams();
            // ReSharper disable LocalizableElement
            Strength.Text = $"Strength\n{strength}%";
            Crew.Text = $"Crew\n{crewAmount}";
            Gold.Text = $"Gold\n{gold}";
            Consumables.Text = $"Consumables\n{consumables}";
            UpgradeCost.Text = $"Cost: {upgradeCost}";
            HighlightResourcesUI();
        }

        private void HighlightResourcesUI()
        {
            var timer = new Timer {Interval = 20};
            var highlightedColor = Color.Gold;
            var simpleColor = Color.Cornsilk;
            Strength.ForeColor = highlightedColor;
            Crew.ForeColor = highlightedColor;
            Gold.ForeColor = highlightedColor;
            Consumables.ForeColor = highlightedColor;
            UpgradeCost.ForeColor = highlightedColor;
            var count = 0;
            timer.Tick += (_, _) =>
            {
                count += 1;
                if (count != 5) return;
                timer.Stop();
                Strength.ForeColor = simpleColor;
                Crew.ForeColor = simpleColor;
                Gold.ForeColor = simpleColor;
                Consumables.ForeColor = simpleColor;
                UpgradeCost.ForeColor = simpleColor;
            };
            timer.Start();
        }

        public void DrawMapCell(Panel newMapCell)
        {
            Controls.Remove(game.CurrentMapCell.MapCellControlPanel);
            Controls.Add(newMapCell);
        }

        public void DrawShipInTile(Ship ship, PictureBox newPlayerTile)
        {
            ship.SpriteBox.Parent.Controls.Remove(ship.SpriteBox);
            newPlayerTile.Controls.Add(ship.SpriteBox);
        }

        public void EndGame()
        {
            Controls.Add(EndGamePanel);
            EndGamePanel.Visible = true;
            EndGamePanel.BringToFront();
        }
    }
}