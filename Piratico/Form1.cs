using System.Collections.Generic;
using System.Windows.Forms;

namespace Piratico
{
    public partial class PiraticoGame : Form
    {
        private readonly GameModel gameModel;
        public readonly ScoutMode ScoutMode;

        public PiraticoGame()
        {
            InitializeComponent();
            gameModel = new GameModel(this);
            var scoutButtons = new Dictionary<Direction, Button>
            {
                [Direction.Up] = Up,
                [Direction.Down] = Down,
                [Direction.Left] = Left,
                [Direction.Right] = Right
            };
            ScoutMode = new ScoutMode(new ScoutData(Scout, scoutButtons), gameModel);
            DrawMapCell(gameModel.CurrentMapCell.MapCellControlPanel);
            Invalidate();
        }

        public void DrawMapCell(Panel newMapCell)
        {
            Controls.Remove(gameModel.CurrentMapCell.MapCellControlPanel);
            Controls.Add(newMapCell);
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
