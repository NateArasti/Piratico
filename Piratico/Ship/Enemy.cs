using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    public class Enemy : Ship
    {
        private readonly List<(Action move, Func<bool> isPossible)> enemyActions = new();

        public Enemy(Image sprite, Size spriteSize, Point mapPosition, PictureBox parentPictureBox, Game game) : 
            base(sprite, spriteSize, mapPosition, parentPictureBox, game)
        {
            StateSprites = new Image[]
                {Resources.EnemyShip, Resources.EnemyShip_66, Resources.EnemyShip_33, Resources.EnemyShip_0};
            SpriteBox.MouseEnter += (_, _) =>
                game.CurrentMapCell.TileMap.GetMapTile(MapPosition).SpriteBox.Image = Resources.AttackTile; 
            SpriteBox.MouseLeave += (_, _) =>
                game.CurrentMapCell.TileMap.GetMapTile(MapPosition).SpriteBox.Image = Resources.SimpleSeaTile; 
            SpriteBox.DoubleClick += (_, _) =>
            {
                if(game.PlayerDoingSomething) return;
                if (game.IsInShootMode)
                {
                    Game.Player.Shoot(this);
                }
                else if (game.CurrentMapCell.TileMap.GetNeighborTiles(MapPosition).Contains(game.Player.CurrentMapTile))
                {
                    game.Player.BoardShip(this);
                    game.Player.EndStep();
                }
            };
            ShipParams = new ShipParams(5, 100, 100, 10);//TODO: Make enemy power progression due to distance from start MapCell
            InitializeActions();
        }

        private void InitializeActions()
        {
            enemyActions.Add((
                () => Shoot(Game.Player),
                () => Game.CurrentMapCell.TileMap
                    .GetHorizontalAndVerticalSeaTiles(MapPosition).Contains(Game.Player.CurrentMapTile)
            ));
            enemyActions.Add((
                () => BoardShip(Game.Player),
                () => Game.CurrentMapCell.TileMap.GetNeighborTiles(MapPosition).Contains(Game.Player.CurrentMapTile)
            ));
            enemyActions.Add((
                MoveToBoardPlayerShip,
                () => true
            ));
        }

        public void ChooseAndExecuteBestAction()
        {
            if(IsDead) return;
            enemyActions.FirstOrDefault(action => action.isPossible()).move();
        }

        private void MoveToBoardPlayerShip()
        {
            Game.MoveShipToNextTile(this, Game.Player.CurrentMapTile);
        }
    }
}
