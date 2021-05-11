using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    class GameModel
    {
        private readonly PiraticoGame gameForm;

        private Timer timer = new Timer();
        private bool timerStarted;

        public MapCell CurrentMapCell { get; private set; }
        public Player Player { get; }
        public MapTile CurrentPlayerTile => CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y];
        public static int TileSize { get; private set; } = 64;
        public static int Width { get; private set; } = 1280;

        public GameModel(PiraticoGame gameForm)
        {
            this.gameForm = gameForm;
            TileSize = gameForm.ClientSize.Height / MapCell.MapSize.Height;
            Width = gameForm.ClientSize.Width;
            CurrentMapCell = new MapCell(this);
            CurrentMapCell.GenerateNeighbors();
            Player = new Player(Resources.PlayerShip, new Point(0, 0), new Size(TileSize, TileSize), new Point(10, 10));
            CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y].SpriteBox.Controls.Add(Player.SpriteBox);
        }

        public void MovePlayerToNewTile(MapTile newMapTile)
        {
            if(timerStarted) return;
            timerStarted = true;

            var path = new HashSet<MapTile>();
            CurrentMapCell.FillPathBetweenMapTiles(path,
                CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y], 
                newMapTile);
            path.Add(newMapTile);

            timer = new Timer {Interval = 200};
            timer.Tick += (sender, args) => MoveToNextTile(path);
            timer.Start();
        }

        private void MoveToNextTile(HashSet<MapTile> path)
        {
            var (newTile, finalDirection) = CurrentMapCell.GetNextPlayerMove(path, Player.MapPosition);
            if (newTile == null) throw new NullReferenceException();
            Player.MoveToNextTileInPath(newTile, finalDirection);
            gameForm.DrawPlayerInTile(CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y].SpriteBox);
            if (path.Count == 0)
            {
                timer.Stop();
                timerStarted = false;
            }
        }
    }
}
