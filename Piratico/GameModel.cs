using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class GameModel
    {
        public bool PlayerDoingSomething => Player.IsMoving || Player.IsShooting; 

        private readonly PiraticoGame gameForm;

        public bool OnNewMapCell => gameForm.ScoutMode.OnNewMapCell;
        // Если нужно будет добавить еще один режим просмотра, то код здесь станет запутанней.
        // Хорошо бы если бы можно было легко добавить новые режимы
        public bool IsInShootMode => gameForm.ShootMode.IsInShootMode;
        public MapCell CurrentMapCell { get; private set; }
        public Player Player { get; }
        public static int TileSize { get; private set; } = 64;
        public static int Width { get; private set; } = 1280;

        public GameModel(PiraticoGame gameForm)
        {
            this.gameForm = gameForm;
            TileSize = gameForm.ClientSize.Height / MapCell.MapSize.Height;
            Width = gameForm.ClientSize.Width;
            CurrentMapCell = new MapCell(this);
            CurrentMapCell.GenerateNeighbors();
            Player = new Player(
                Resources.PlayerShip,
                new Size(TileSize, TileSize),
                CurrentMapCell.GetMapTile(Player.PlayerStartPosition).SpriteBox,
                this);
        }

        public void SwitchToMapCell(Direction direction)
        {
            var newMapCell = CurrentMapCell.GetNeighbor(direction);
            if(newMapCell == null) return;
            gameForm.DrawMapCell(newMapCell.MapCellControlPanel);
            CurrentMapCell = newMapCell;
        }

        public void MoveToNewMapCell(MapTile endTile)
        {
            var direction = gameForm.ScoutMode.LastScoutDirection;
            var newMapCell = CurrentMapCell;
            ExitScoutModeManually();
            var originMapCell = CurrentMapCell;
            var (originBorderPoint, newBorderPoint) = TileMap.GetMinPathBetweenMapCells(
                originMapCell, 
                newMapCell,
                direction,
                Player.CurrentMapTile, 
                endTile);
            var partOfPathIsDone = false;
            var originBorderTile = originMapCell.GetMapTile(originBorderPoint);
            var newBorderTile = newMapCell.GetMapTile(newBorderPoint);
            void PlayerSteps()
            {
                if (!partOfPathIsDone)
                {
                    if (Player.CurrentMapTile == originBorderTile) 
                        partOfPathIsDone = true;
                    else
                    {
                        MoveShipToNextTile(Player, originBorderTile);
                        return;
                    }
                }

                if (CurrentMapCell != newMapCell)
                {
                    SwitchToMapCell(direction);
                    Player.MoveToNextTile(newBorderTile);
                    gameForm.DrawShipInTile(Player, newBorderTile.SpriteBox);
                    return;
                }

                Player.IsMoving = Player.CurrentMapTile != endTile;
                if(!Player.IsMoving) return;
                MoveShipToNextTile(Player, endTile);
            }

            Player.StartMovement(PlayerSteps);
            newMapCell.GenerateNeighbors();
        }

        public void MoveShipToNextTile(Ship ship, MapTile newMapTile)
        {
            if (ship.CurrentMapTile == newMapTile) return;
            var (newTile, finalDirection) = CurrentMapCell.GetNextShipMove(ship.MapPosition, newMapTile.MapPosition);
            if (newTile == null) return;
            ship.MoveToNextTile(newTile, finalDirection);
            gameForm.DrawShipInTile(ship, CurrentMapCell.GetMapTile(ship.MapPosition).SpriteBox);
            if (ship is Player player) player.StepEnded = true;
        }

        public void LetEnemiesDoTheirMove()
        {
            foreach (var enemy in CurrentMapCell.Enemies) enemy.ChooseAndExecuteBestAction();
        }

        public void DeleteShip(Ship ship)
        {
            // стоит сделать какую-то задержку или индикацию того, что игра закончена, иначе игрок не понимает что происходит
            if (Player.Equals(ship))
                Application.Restart();
            else
                gameForm.DeleteShip(ship);
        }

        public void ExitScoutModeManually() =>
            gameForm.ScoutMode.ExitScoutModeManually();

        public void ExitShootModeManually() => gameForm.ShootMode.TurnOff();
    }
}
