using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class MapCell
    {
        public static Size MapSize = new(24, 18);
        private readonly int distanceFromStartCell;

        private readonly Game game;

        public readonly Panel MapCellControlPanel;
        private readonly Dictionary<Direction, MapCell> neighbors;
        public readonly TileMap TileMap;

        public MapCell(Game game)
        {
            this.game = game;
            var deltaFromBorders = (Game.Width - MapSize.Width * Game.TileSize) / 2;
            MapCellControlPanel = new Panel
            {
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                ForeColor = Color.Transparent
            };
            TileMap = new TileMap(deltaFromBorders, game, MapCellControlPanel);
            SpawnEnemies();
            neighbors = new Dictionary<Direction, MapCell>
            {
                {Direction.None, this},
                {Direction.Down, null},
                {Direction.Up, null},
                {Direction.Left, null},
                {Direction.Right, null}
            };
        }

        private MapCell(Direction direction, MapCell neighbor) : this(neighbor.game)
        {
            switch (direction)
            {
                case Direction.Down:
                    neighbors[Direction.Up] = neighbor;
                    break;
                case Direction.Up:
                    neighbors[Direction.Down] = neighbor;
                    break;
                case Direction.Left:
                    neighbors[Direction.Right] = neighbor;
                    break;
                case Direction.Right:
                    neighbors[Direction.Left] = neighbor;
                    break;
                case Direction.None:
                    throw new ArgumentException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            distanceFromStartCell = neighbor.distanceFromStartCell + 1;
        }

        public List<Enemy> Enemies { get; } = new();

        private void SpawnEnemies()
        {
            var random = new Random();
            var enemyCount = random.Next(3);
            do
            {
                var mapPosition = new Point(random.Next(MapSize.Width), random.Next(MapSize.Height));
                var mapTile = TileMap.GetMapTile(mapPosition);
                if (mapTile.TileType != MapTileType.Sea ||
                    Player.PlayerStartPosition == mapPosition ||
                    mapTile.HasShipOnTile) continue;
                enemyCount -= 1;
                var enemy = new Enemy(
                    new Size(Game.TileSize, Game.TileSize),
                    mapPosition,
                    mapTile.SpriteBox,
                    game,
                    distanceFromStartCell);
                Enemies.Add(enemy);
                mapTile.HasShipOnTile = true;
            } while (enemyCount > 0);
        }

        public void GenerateNeighbors()
        {
            foreach (var value in Enum.GetValues(typeof(Direction)))
            {
                var direction = (Direction) value;
                neighbors[direction] ??= new MapCell(direction, this);
            }
        }

        public MapCell GetNeighbor(Direction direction)
        {
            return neighbors[direction];
        }
    }
}