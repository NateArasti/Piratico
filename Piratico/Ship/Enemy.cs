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

        private readonly ShipParams standardEnemyParams =
            new(5, 100, 100, 10);

        public Enemy(Size spriteSize, Point mapPosition, PictureBox parentPictureBox, Game game,
            int distanceFromStartCell) :
            base(spriteSize, mapPosition, parentPictureBox, game)
        {
            CalculateEnemyParams(distanceFromStartCell);
            StateSprites = new Image[]
                {Resources.EnemyShip, Resources.EnemyShip_66, Resources.EnemyShip_33, Resources.EnemyShip_0};
            CheckCurrentShipState();
            InitializeActions();

            SpriteBox.MouseEnter += (_, _) =>
                game.CurrentMapCell.TileMap.GetMapTile(MapPosition).SpriteBox.Image = Resources.AttackTile;
            SpriteBox.MouseLeave += (_, _) =>
                game.CurrentMapCell.TileMap.GetMapTile(MapPosition).SpriteBox.Image =
                    game.CurrentMapCell.TileMap.GetMapTile(MapPosition).OriginalTile;
            SpriteBox.DoubleClick += (_, _) =>
            {
                if (game.PlayerDoingSomething) return;
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
        }

        private void CalculateEnemyParams(int distanceFromStartCell)
        {
            var modifier = 1 + (double) distanceFromStartCell / 200;
            ShipParams = new ShipParams(
                (int) (standardEnemyParams.CrewAmount * modifier),
                (int) (standardEnemyParams.Gold * modifier),
                (int) (standardEnemyParams.Consumables * modifier),
                (int) (standardEnemyParams.MaxDamage * modifier));
        }

        private void InitializeActions()
        {
            enemyActions.Add((
                () => BoardShip(Game.Player),
                () => Game.CurrentMapCell.TileMap.GetNeighborTiles(MapPosition).Contains(Game.Player.CurrentMapTile)
            ));
            enemyActions.Add((
                () => Shoot(Game.Player),
                () => Game.CurrentMapCell.TileMap
                    .GetHorizontalAndVerticalSeaTiles(MapPosition).Contains(Game.Player.CurrentMapTile)
            ));
            enemyActions.Add((
                MoveToShoot,
                () =>
                {
                    var delta = GetDeltaToShootPlayer();
                    var newPosition = MapPosition + delta;
                    return Game.CurrentMapCell.TileMap.GetMapTile(newPosition).TileType != MapTileType.Island &&
                        (delta.Width < 5 || delta.Height < 5);
                }
            ));
            enemyActions.Add((
                MoveToBoardPlayerShip,
                () => ShipParams.Strength > 70
            ));
            enemyActions.Add((
                () => { },
                () => true
            ));
        }

        public void ChooseAndExecuteBestAction()
        {
            if (IsDead) return;
            enemyActions.FirstOrDefault(action => action.isPossible()).move();
        }

        private void MoveToBoardPlayerShip()
        {
            Game.MoveShipToNextTile(this, Game.Player.CurrentMapTile);
        }

        private Size GetDeltaToShootPlayer()
        {
            var delta = new Size(int.MaxValue, int.MaxValue);
            foreach (var tile in Game.CurrentMapCell.TileMap.GetHorizontalAndVerticalSeaTiles(Game.Player.MapPosition))
            {
                var newDelta = (Size)tile.MapPosition - (Size) MapPosition;
                if(newDelta.Width != 0 && newDelta.Height !=0) continue;
                if (newDelta.Width < delta.Width || newDelta.Width < delta.Height || 
                    newDelta.Height < delta.Height || newDelta.Height < delta.Width) delta = newDelta;
            }
            return delta;
        }

        private void MoveToShoot()
        {
            Game.MoveShipToNextTile(this,
                Game.CurrentMapCell.TileMap.GetMapTile(MapPosition + GetDeltaToShootPlayer()));
        }
    }
}