using System;
using System.Drawing;
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
            gameModel = new GameModel(this);
            GameTimer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
        }
    }
}
