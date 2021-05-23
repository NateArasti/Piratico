using System;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class GameModel
    {
        private readonly PiraticoGame gameForm;

        private Timer timer = new Timer();
        private bool timerStarted;

        public bool OnNewMapCell => gameForm.ScoutMode.OnNewMapCell;
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
            SwitchToMapCell((Direction)(-(int)direction));
            var originMapCell = CurrentMapCell;
            var (originBorderPoint, newBorderPoint) = TileMap.GetMinPathBetweenMapCells(
                originMapCell, 
                newMapCell, 
                gameForm.ScoutMode.LastScoutDirection,
                Player.CurrentMapTile, 
                endTile);
            gameForm.ScoutMode.ExitScoutModeManually();
            newMapCell.GenerateNeighbors();
            var partOfPathIsDone = false;
            bool PlayerSteps()
            {
                if (!partOfPathIsDone)
                {
                    partOfPathIsDone = MoveShipToNextTile(Player, originMapCell.GetMapTile(originBorderPoint));
                    return false;
                }
                if (CurrentMapCell != newMapCell)
                {
                    SwitchToMapCell(direction);
                    Player.MoveToNextTile(newMapCell.GetMapTile(newBorderPoint));
                    gameForm.DrawShipInTile(Player, newMapCell.GetMapTile(newBorderPoint).SpriteBox);
                }
                else
                    return MoveShipToNextTile(Player, endTile);

                return false;
            }

            StartTimer(PlayerSteps);
        }

        public void StartTimer(Func<bool> playerSteps)
        {
            if(timerStarted) return;
            timerStarted = true;
            timer = new Timer {Interval = 200};
            timer.Tick += (sender, args) =>
            {
                if (!playerSteps()) return;
                timer.Stop();
                timerStarted = false;
            };
            timer.Tick += (sender, args) => LetEnemiesDoTheirMove();
            timer.Start();
        }

        public bool MoveShipToNextTile(Ship ship, MapTile newMapTile)
        {
            if (ship.CurrentMapTile == newMapTile) return true;
            var (newTile, finalDirection) = CurrentMapCell.GetNextShipMove(ship.MapPosition, newMapTile.MapPosition);
            if (newTile == null) return true;
            newTile.HasShipOnTile = true;
            CurrentMapCell.GetMapTile(ship.MapPosition).HasShipOnTile = false;
            ship.MoveToNextTile(newTile, finalDirection);
            gameForm.DrawShipInTile(ship, CurrentMapCell.GetMapTile(ship.MapPosition).SpriteBox);
            return ship.CurrentMapTile == newMapTile;
        }

        private void LetEnemiesDoTheirMove()
        {
            foreach (var enemy in CurrentMapCell.Enemies) enemy.ChooseAndExecuteBestAction();
        }

        public void DeleteShip(Ship ship)
        {
            if (Player.Equals(ship))
            {
                Application.Restart();
            }
            else
                gameForm.DeleteShip(ship);
        }
    }
}
