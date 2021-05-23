using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class MapCell
    {
        public static Size MapSize = new Size(20, 15);

        private readonly GameModel gameModel;
        private readonly Dictionary<Direction, MapCell> neighbors;
        private readonly TileMap tileMap;

        public List<Enemy> Enemies { get; } = new List<Enemy>();
        public readonly Panel MapCellControlPanel;

        public MapCell(GameModel gameModel)
        {
            this.gameModel = gameModel;
            var deltaFromBorders = (GameModel.Width - MapSize.Width * GameModel.TileSize) / 2;
            MapCellControlPanel = new Panel
            {
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                ForeColor = Color.Transparent
            };
            tileMap = new TileMap(deltaFromBorders, gameModel, MapCellControlPanel);
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

        private MapCell(Direction direction, MapCell neighbor) : this(neighbor.gameModel)
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
        }

        private void SpawnEnemies()
        {
            var random = new Random();
            var enemyCount = random.Next(3);
            do
            {
                var mapPosition = new Point(random.Next(MapSize.Width), random.Next(MapSize.Height));
                var mapTile = GetMapTile(mapPosition);
                if (mapTile.TileType != MapTileType.Sea ||
                    mapTile.HasShipOnTile) continue;
                enemyCount -= 1;
                Enemies.Add(
                    new Enemy(Resources.EnemyShip,
                        new Size(GameModel.TileSize, GameModel.TileSize),
                        mapPosition,
                        mapTile.SpriteBox,
                        gameModel)
                );
                mapTile.HasShipOnTile = true;
            } while (enemyCount > 0);
        }

        public void GenerateNeighbors()
        {
            foreach (var value in Enum.GetValues(typeof(Direction)))
            {
                var direction = (Direction)value;
                if (neighbors[direction] == null)
                    neighbors[direction] = new MapCell(direction, this);
            }
        }

        public MapCell GetNeighbor(Direction direction) => neighbors[direction];

        public (MapTile newTile, Point finalDirection)
            GetNextShipMove(Point shipMapPosition, Point finishMapPosition) =>
            tileMap.GetNextShipMove(shipMapPosition, finishMapPosition);

        public IEnumerable<MapTile> GetNeighborTiles(Point mapPosition) => tileMap.GetNeighborTiles(mapPosition);

        public MapTile GetMapTile(Point mapPosition) => tileMap.GetMapTile(mapPosition);

        public int GetPathLengthToTile(MapTile startMapTile, MapTile endMapTile) =>
            tileMap.GetPathLengthToTile(startMapTile, endMapTile);
    }
}