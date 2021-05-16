using System.Windows.Forms;

namespace Piratico
{
    public partial class PiraticoGame : Form
    {
        private GameModel gameModel;
        private Button[] scoutButtons;
        public bool Scouting { get; private set; }
        private Direction lastScoutDirection = Direction.None;

        public void DrawMapCell(Panel newMapCell)
        {
            Controls.Remove(gameModel.CurrentMapCell.MapCellController);
            Controls.Add(newMapCell);
        }

        public PiraticoGame()
        {
            InitializeComponent();
            Init();
            Invalidate();
        }

        private void Init()
        {
            gameModel = new GameModel(this);
            DrawMapCell(gameModel.CurrentMapCell.MapCellController);
            InitializeScoutButtons();
        }

        private void InitializeScoutButtons()
        {
            scoutButtons = new[] { Up, Left, Right, Down };
            Scout.Click += (sender, args) =>
            {
                if (!Scouting)
                    SwitchScoutButtonVisibility(true);
                else
                {
                    gameModel.SwitchToMapCell(lastScoutDirection);
                    lastScoutDirection = Direction.None;
                    SwitchScoutButtonVisibility(false);
                }

                Scouting = !Scouting;
            };
            Up.Click += (sender, args) =>
            {
                if(Down.Visible)
                {
                    SwitchScoutButtonVisibility(false);
                    Down.Visible = true;
                }
                else
                    SwitchScoutButtonVisibility(true);
                gameModel.SwitchToMapCell(Direction.Up);
                lastScoutDirection = Direction.Down;
            };
            Down.Click += (sender, args) =>
            {
                if (Up.Visible)
                {
                    SwitchScoutButtonVisibility(false);
                    Up.Visible = true;
                }
                else
                    SwitchScoutButtonVisibility(true);
                gameModel.SwitchToMapCell(Direction.Down);
                lastScoutDirection = Direction.Up;
            };
            Right.Click += (sender, args) =>
            {
                if (Left.Visible)
                {
                    SwitchScoutButtonVisibility(false);
                    Left.Visible = true;
                }
                else
                    SwitchScoutButtonVisibility(true);
                gameModel.SwitchToMapCell(Direction.Right);
                lastScoutDirection = Direction.Left;
            };
            Left.Click += (sender, args) =>
            {
                if (Right.Visible)
                {
                    SwitchScoutButtonVisibility(false);
                    Right.Visible = true;
                }
                else
                    SwitchScoutButtonVisibility(true);
                gameModel.SwitchToMapCell(Direction.Left);
                lastScoutDirection = Direction.Right;
            };
        }

        private void SwitchScoutButtonVisibility(bool visible)
        {
            foreach (var button in scoutButtons) button.Visible = visible;
        }

        public void DrawShipInTile(Ship ship, PictureBox newPlayerTile)
        {
            ship.SpriteBox.Parent.Controls.Remove(ship.SpriteBox);
            newPlayerTile.Controls.Add(ship.SpriteBox);
        }

        public void DeleteShip(Ship ship)
        {

        }
    }
}
