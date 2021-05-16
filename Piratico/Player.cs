using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class Player : Ship
    {
        public static readonly Point PlayerStartPosition =
            new Point(MapCell.MapSize.Width / 2, MapCell.MapSize.Height / 2);

        public Player(Image sprite, Size spriteSize, PictureBox parentPictureBox, GameModel gameModel) : 
            base(sprite, spriteSize, PlayerStartPosition, parentPictureBox, gameModel)
        {
        }
    }
}
