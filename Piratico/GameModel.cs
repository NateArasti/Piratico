using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    class GameModel
    {
        private static Timer _timer = new Timer();

        private static MapCell _currentMapCell;
        private static Player _player;
        public static int TileSize { get; private set; } = 64;
        public static int Width { get; private set; } = 1280;

        public GameModel(Control formControl)
        {
            TileSize = formControl.ClientSize.Height / MapCell.MapSize.Height;
            Width = formControl.ClientSize.Width;
            _currentMapCell = new MapCell();
            _currentMapCell.GenerateNeighbors();
            formControl.Controls.Add(_currentMapCell.MapCellController);
            _player = new Player(Resources.PlayerShip, new Point(0, 0), new Size(TileSize, TileSize), new Point(0, 0));
            _currentMapCell.Map[_player.MapPosition.X, _player.MapPosition.Y].SpriteBox.Controls.Add(_player.SpriteBox);
        }

        public static void MovePlayerToNewTile(MapTile newMapTile)
        {
            var path = new Queue<MapTile>();
            _currentMapCell.FillPathBetweenMapTiles(path,
                _currentMapCell.Map[_player.MapPosition.X, _player.MapPosition.Y], newMapTile);
            if (newMapTile.Index < _currentMapCell.Map[_player.MapPosition.X, _player.MapPosition.Y].Index)
                path = new Queue<MapTile>(path.Reverse());
            path.Enqueue(newMapTile);
            _timer = new Timer {Interval = 200};
            _timer.Tick += (sender, args) => MoveToNextTile(path);
            _timer.Start();
        }

        private static void MoveToNextTile(Queue<MapTile> path)
        {
            var newTile = path.Dequeue();
            var delta = new Point(newTile.MapPosition.X - _player.MapPosition.X,
                newTile.MapPosition.Y - _player.MapPosition.Y);
            _player.RotateTo(Player.DirectionsRotations[MapCell.MapDirections[delta]]);
            _currentMapCell.Map[_player.MapPosition.X, _player.MapPosition.Y].SpriteBox.Controls.Remove(_player.SpriteBox);
            newTile.SpriteBox.Controls.Add(_player.SpriteBox);
            _player.MapPosition = newTile.MapPosition;
            if (path.Count == 0)
                _timer.Stop();
        }
    }
}
