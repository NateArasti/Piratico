using System.Drawing;
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
            _timer = new Timer {Interval = 200};
            _timer.Tick += (sender, args) => MoveToNextTile(newMapTile);
            _timer.Start();
        }

        private static void MoveToNextTile(MapTile finishTile)
        {
            var nextTile = _currentMapCell.Map[_player.MapPosition.X, _player.MapPosition.Y];
            Rotations newRotation = Rotations.Down;
            foreach (var direction in MapCell.MapDirections)
            {
                if (MapCell.CheckIfInBorders(_player.MapPosition.X + direction.Value.X,
                        _player.MapPosition.Y + direction.Value.Y) &&
                    nextTile.PathsLengths[finishTile.MapPosition.X, finishTile.MapPosition.Y] >
                    _currentMapCell.Map[_player.MapPosition.X + direction.Value.X, _player.MapPosition.Y + direction.Value.Y]
                        .PathsLengths[finishTile.MapPosition.X, finishTile.MapPosition.Y])
                {
                    nextTile = _currentMapCell.Map[_player.MapPosition.X + direction.Value.X, _player.MapPosition.Y + direction.Value.Y];
                    newRotation = Player.DirectionsRotations[direction.Key];
                }

            }
            _player.RotateTo(newRotation);
            _currentMapCell.Map[_player.MapPosition.X, _player.MapPosition.Y].SpriteBox.Controls.Remove(_player.SpriteBox);
            nextTile.SpriteBox.Controls.Add(_player.SpriteBox);
            _player.MapPosition = nextTile.MapPosition;
            if (_player.MapPosition == finishTile.MapPosition)
                _timer.Stop();
        }
    }
}
