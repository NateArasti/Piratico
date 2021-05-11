using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    class GameModel
    {
        private static Timer _timer = new Timer();
        private static bool timerStarted;

        public static MapCell CurrentMapCell { get; private set; }
        public static Player Player { get; private set; }
        public static int TileSize { get; private set; } = 64;
        public static int Width { get; private set; } = 1280;

        public GameModel(Size clientSize)
        {
            TileSize = clientSize.Height / MapCell.MapSize.Height;
            Width = clientSize.Width;
            CurrentMapCell = new MapCell();
            CurrentMapCell.GenerateNeighbors();
            Player = new Player(Resources.PlayerShip, new Point(0, 0), new Size(TileSize, TileSize), new Point(0, 0));
            CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y].SpriteBox.Controls.Add(Player.SpriteBox);
        }

        // В следующих двух методах много рутины и низкоуровневых действий. Класс игры находится на высоком уровне абстракции.
        // Он должен делегировать большинство своей работы другим объектам.
        public static void MovePlayerToNewTile(MapTile newMapTile)
        {
            if(timerStarted) return;
            timerStarted = true;
            var path = new HashSet<MapTile>();
            CurrentMapCell.FillPathBetweenMapTiles(path,
                CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y], newMapTile);
            path.Add(newMapTile);
            // старый таймер был Disposable его нужно Dispose прежде, чем выбросить.
            // А может быть тут лучше не создавать таймер каждый раз.
            _timer = new Timer {Interval = 200};
            _timer.Tick += (sender, args) => MoveToNextTile(path);
            _timer.Start();
        }

        private static void MoveToNextTile(HashSet<MapTile> path)
        {
            // не стоит полагаться на то, что HashSet хранит объекты в порядке добавления.
            // Для явного порядка лучше использовать очередь.
            var newTile = path.First();
            foreach (var direction in MapCell.MapDirections.Keys)
            {
                var newPoint = new Point(Player.MapPosition.X + direction.X, Player.MapPosition.Y + direction.Y);
                if (!CurrentMapCell.InBorders(newPoint) || !path.Contains(CurrentMapCell.Map[newPoint.X, newPoint.Y])) continue;
                newTile = CurrentMapCell.Map[newPoint.X, newPoint.Y]; 
                path.Remove(newTile);
                Player.RotateTo(Player.DirectionsRotations[MapCell.MapDirections[direction]]);
                break;
            }
            CurrentMapCell.Map[Player.MapPosition.X, Player.MapPosition.Y].SpriteBox.Controls.Remove(Player.SpriteBox);
            newTile.SpriteBox.Controls.Add(Player.SpriteBox);
            Player.MapPosition = newTile.MapPosition;
            if (path.Count == 0)
            {
                _timer.Stop();
                timerStarted = false;
            }
        }
    }
}
