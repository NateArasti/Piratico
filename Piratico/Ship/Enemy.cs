using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    public class Enemy : Ship
    {
        private readonly List<(Action move, Func<bool> isPossible)> enemyActions =
            new List<(Action move, Func<bool> isPossible)>();

        public Enemy(Image sprite, Size spriteSize, Point mapPosition, PictureBox parentPictureBox, GameModel gameModel) : 
            base(sprite, spriteSize, mapPosition, parentPictureBox, gameModel)
        {
            SpriteBox.DoubleClick += (sender, args) =>
            {
                if (!GameModel.PlayerDoingSomething)
                {
                    GameModel.Player.Shoot(this);
                }
            };
            ShipParams = new ShipParams();//TODO: Make enemy power progression due to distance from start MapCell
            InitializeActions();
        }

        private void InitializeActions()
        {
            enemyActions.Add((
                () => BoardShip(GameModel.Player),
                () => GameModel.CurrentMapCell.GetNeighborTiles(MapPosition).Contains(GameModel.Player.CurrentMapTile)
            ));
            enemyActions.Add((
                MoveToBoardPlayerShip,
                () => true
            ));
            enemyActions.Add((
                () => Shoot(GameModel.Player),
                () => MapPosition.X == GameModel.Player.MapPosition.X || MapPosition.Y == GameModel.Player.MapPosition.Y
            ));
        }

        public void ChooseAndExecuteBestAction()
        {
            enemyActions.FirstOrDefault(action => action.isPossible()).move();
        }

        private void MoveToBoardPlayerShip()
        {
            GameModel.MoveShipToNextTile(this, GameModel.Player.CurrentMapTile);
        }
    }
}
