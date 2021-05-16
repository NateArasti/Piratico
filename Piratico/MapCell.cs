using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Right,
        Left
    }

    public class MapCell
    {
        private readonly GameModel gameModel;

        public List<Enemy> Enemies { get; } = new List<Enemy>();

        public static readonly Size MapSize = new Size(20, 15);
        public readonly int DeltaFromBorders;

        public static IReadOnlyDictionary<Point, Direction> MapDirections = new Dictionary<Point, Direction>
        {
            {new Point(0, 1), Direction.Down},
            {new Point(0, -1), Direction.Up},
            {new Point(1, 0), Direction.Right},
            {new Point(-1, 0), Direction.Left}
        };

        private readonly Dictionary<Direction, MapCell> neighbors;

        public readonly MapTile[,] Map = new MapTile[MapSize.Width, MapSize.Height];

        private readonly int[,] paths = new int[MapSize.Width * MapSize.Height, MapSize.Width * MapSize.Height];
        private readonly List<Point> allSeaTiles = new List<Point>();

        public readonly Panel MapCellController;

        public MapCell(GameModel gameModel)
        {
            this.gameModel = gameModel;
            DeltaFromBorders = (GameModel.Width - MapSize.Width * GameModel.TileSize) / 2;
            MapCellController = new Panel
            {
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                ForeColor = Color.Transparent
            };
            neighbors = new Dictionary<Direction, MapCell>
            {
                {Direction.None, this},
                {Direction.Down, null},
                {Direction.Up, null},
                {Direction.Left, null},
                {Direction.Right, null}
            };
            GenerateMap();
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

        public void GenerateNeighbors()
        {
            for (var i = 0; i < 5; i++)
            {
                var direction = (Direction) i;
                if (neighbors[direction] == null)
                    neighbors[direction] = new MapCell(direction, this);
            }
        }

        public MapCell GetNeighbor(Direction direction) => neighbors[direction];

        private void GenerateMap()
        {
            var islandsMap = IslandsGenerator.GenerateIslands(MapSize.Width, MapSize.Height);
            for (var i = 0; i < MapSize.Width; i++)
            for (var j = 0; j < MapSize.Height; j++)
            {
                var point = new Point(i, j);
                switch (islandsMap[i, j].Item1)
                {
                    case MapTileType.Island:
                        Map[i, j] = new MapTile(point, -1, gameModel, islandsMap[i, j].Item1, islandsMap[i, j].Item2);
                        break;
                    case MapTileType.Shallow:
                        allSeaTiles.Add(point);
                        Map[i, j] = new MapTile(point, allSeaTiles.Count - 1, gameModel, islandsMap[i, j].Item1, islandsMap[i, j].Item2);
                        break;
                    case MapTileType.Sea:
                        allSeaTiles.Add(point);
                        Map[i, j] = new MapTile(point, allSeaTiles.Count - 1, gameModel, MapTileType.Sea, null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Map[i, j].SpriteBox.Location = new Point(DeltaFromBorders + i * GameModel.TileSize, j * GameModel.TileSize);
                MapCellController.Controls.Add(Map[i, j].SpriteBox);
            }

            Map[Player.PlayerStartPosition.X, Player.PlayerStartPosition.Y].HasShipOnTile = true;
            SpawnEnemies();
            GetPathsBetweenTiles();
        }

        private void SpawnEnemies()
        {
            var random = new Random();
            var enemyCount = random.Next(5);
            do
            {
                var mapPosition = new Point(random.Next(Map.GetLength(0)), random.Next(Map.GetLength(1)));
                if (Map[mapPosition.X, mapPosition.Y].TileType != MapTileType.Sea) continue;
                enemyCount -= 1;
                Enemies.Add(
                    new Enemy(Resources.EnemyShip,
                        new Size(GameModel.TileSize, GameModel.TileSize),
                        mapPosition,
                        Map[mapPosition.X, mapPosition.Y].SpriteBox,
                        gameModel)
                );
                Map[mapPosition.X, mapPosition.Y].HasShipOnTile = true;
            } while (enemyCount > 0);
        }

        private void GetPathsBetweenTiles()
        {
            const int infinite = int.MaxValue;
            var length = allSeaTiles.Count;
            for (var i = 0; i < length; i++)
            for (var j = i; j < length; j++)
            {
                if (i == j) paths[i, j] = 0;
                else if (MapDirections.ContainsKey(new Point(allSeaTiles[i].X - allSeaTiles[j].X,
                    allSeaTiles[i].Y - allSeaTiles[j].Y)))
                {
                    paths[i, j] = 1;
                    paths[j, i] = 1;
                }
                else
                {
                    paths[i, j] = infinite;
                    paths[j, i] = infinite;
                }
            }
            for (var k = 0; k < length; ++k)
            for (var i = 0; i < length; ++i)
            for (var j = 0; j < length; ++j)
            {
                if (paths[i, k] < infinite && paths[k, j] < infinite &&
                    paths[i, j] > paths[i, k] + paths[k, j])
                    paths[i, j] = paths[i, k] + paths[k, j];
            }
        }

        public (MapTile newTile, Point finalDirection) GetNextShipMove(Point shipMapPosition, Point finishMapPosition)
        {
            var currentTile = Map[shipMapPosition.X, shipMapPosition.Y];
            var finish = Map[finishMapPosition.X, finishMapPosition.Y];
            (MapTile newTile, var finalDirection) = (null, new Point());
            var minPathLength = paths[currentTile.Index, finish.Index];
            foreach (var direction in MapDirections.Keys)
            {
                var newPoint = new Point(shipMapPosition.X + direction.X, shipMapPosition.Y + direction.Y);
                if (!InBorders(newPoint) || 
                    Map[newPoint.X, newPoint.Y].TileType == MapTileType.Island ||
                    paths[Map[newPoint.X, newPoint.Y].Index, finish.Index] > minPathLength) continue;
                minPathLength = paths[currentTile.Index, finish.Index];
                newTile = Map[newPoint.X, newPoint.Y];
                finalDirection = direction;
            }

            return (newTile, finalDirection);
        }

        public IEnumerable<MapTile> GetNeighborTiles(Point mapPosition) =>
            from direction in MapDirections.Keys 
                select new Point(mapPosition.X + direction.X, mapPosition.Y + direction.Y) 
                into newPoint 
                where InBorders(newPoint) && Map[newPoint.X, newPoint.Y].TileType != MapTileType.Island 
                select Map[newPoint.X, newPoint.Y];

        private static bool InBorders(Point point) => point.X >= 0 && point.X < MapSize.Width && point.Y >= 0 && point.Y < MapSize.Height;
    }
}
