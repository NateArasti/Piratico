using System;
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

        private void Init()
        {
            gameModel = new GameModel(ClientSize);
            Controls.Add(GameModel.CurrentMapCell.MapCellController);
            GameTimer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
        }
    }
}
