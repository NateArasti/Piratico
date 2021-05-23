using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    public class Enemy : Ship
    {
        public Enemy(Image sprite, Size spriteSize, Point mapPosition, PictureBox parentPictureBox, GameModel gameModel) : 
            base(sprite, spriteSize, mapPosition, parentPictureBox, gameModel)
        {
        }

        public void ChooseAndExecuteBestAction()
        {
            //TODO:Add some actions like move to shoot, escape from battle, leave a bomb bottle etc.
            if(GameModel.CurrentMapCell.GetNeighborTiles(MapPosition).Contains(GameModel.Player.CurrentMapTile))
                BoardShip(GameModel.Player);
            else
                MoveToBoardPlayerShip();
        }

        private void MoveToBoardPlayerShip()
        {
            GameModel.MoveShipToNextTile(this, GameModel.Player.CurrentMapTile);
        }
    }
}
