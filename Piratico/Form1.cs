using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public partial class PiraticoGame : Form
    {
        private GameModel gameModel;

        public void DrawMapCell() => Controls.Add(gameModel.CurrentMapCell.MapCellController);

        public PiraticoGame()
        {
            InitializeComponent();
            Init();
            Invalidate();
        }

        private void Init()
        {
            gameModel = new GameModel(this);
            DrawMapCell();
            GameTimer.Start();
        }

        public void DrawPlayerInTile(PictureBox newPlayerTile)
        {
            gameModel.CurrentPlayerTile.SpriteBox.Controls.Remove(gameModel.Player.SpriteBox);
            newPlayerTile.Controls.Add(gameModel.Player.SpriteBox);
        }
    }
}
