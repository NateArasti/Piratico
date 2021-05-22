using System.Collections.Generic;
using System.Windows.Forms;

namespace Piratico
{
    public partial class PiraticoGame : Form
    {
        private GameModel gameModel;

        public PiraticoGame()
        {
            InitializeComponent();
            Init();
            Invalidate();
        }

        public void DrawMapCell(Panel newMapCell)
        {
            Controls.Remove(gameModel.CurrentMapCell.MapCellControlPanel);
            Controls.Add(newMapCell);
        }

        private void Init()
        {
            var scoutButtons = new Dictionary<Direction, Button>
            {
                [Direction.Up] = Up,
                [Direction.Down] = Down,
                [Direction.Left] = Left,
                [Direction.Right] = Right
            };
            gameModel = new GameModel(this, new ScoutData(Scout, scoutButtons));
            DrawMapCell(gameModel.CurrentMapCell.MapCellControlPanel);
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
