using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class GameModel
    {
        private readonly PiraticoGame gameForm;
        private readonly ScoutMode scoutMode;

        private Timer timer = new Timer();
        private bool timerStarted;

        public bool IsScouting => scoutMode.IsScouting;
        public MapCell CurrentMapCell { get; private set; }
        public Player Player { get; }
        public static int TileSize { get; private set; } = 64;
        public static int Width { get; private set; } = 1280;

        public GameModel(PiraticoGame gameForm, ScoutData scoutData)
        {
            this.gameForm = gameForm;
            scoutMode = new ScoutMode(scoutData, this);
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

        public void MoveToNewMapCell()
        {

        }

        public void MovePlayerToNewTile(MapTile newMapTile)
        {
            if(timerStarted) return;
            timerStarted = true;
            timer = new Timer {Interval = 200};
            timer.Tick += (sender, args) => MoveShipToNextTile(Player, newMapTile);
            timer.Tick += (sender, args) => LetEnemiesDoTheirMove();
            timer.Start();
        }

        public void MoveShipToNextTile(Ship ship, MapTile newMapTile)
        {
            var (newTile, finalDirection) = CurrentMapCell.GetNextShipMove(ship.MapPosition, newMapTile.MapPosition);
            if (newTile == null) return;
            newTile.HasShipOnTile = true;
            CurrentMapCell.GetMapTile(ship.MapPosition).HasShipOnTile = false;
            ship.MoveToNextTile(newTile, finalDirection);
            gameForm.DrawShipInTile(ship, CurrentMapCell.GetMapTile(ship.MapPosition).SpriteBox);
            if (ship.MapPosition != newMapTile.MapPosition) return;
            timer.Stop();
            timerStarted = false;
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
