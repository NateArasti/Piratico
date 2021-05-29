using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piratico
{
    public class TileMap
    {
        private static Size MapSize => MapCell.MapSize;

        public static IReadOnlyDictionary<Point, Direction> MapDirections = new Dictionary<Point, Direction>
        {
            {new Point(), Direction.None },
            {new Point(0, 1), Direction.Down},
            {new Point(0, -1), Direction.Up},
            {new Point(1, 0), Direction.Right},
            {new Point(-1, 0), Direction.Left}
        };

        private readonly int deltaFromBorders;
        private readonly Game game;
        private readonly MapTile[,] map = new MapTile[MapSize.Width, MapSize.Height];
        private readonly int[,] paths = new int[MapSize.Width * MapSize.Height, MapSize.Width * MapSize.Height];
        private readonly List<Point> allSeaTiles = new();

        public TileMap(int deltaFromBorders, Game game, Panel mapCellControlPanel)
        {
            this.deltaFromBorders = deltaFromBorders;
            this.game = game;
            GenerateMap(mapCellControlPanel);
            GetPathsBetweenTiles();
        }

        private void GenerateMap(Panel mapCellControlPanel)
        {
            var islandsMap = IslandsGenerator.GenerateIslands(MapSize.Width, MapSize.Height);
            for (var i = 0; i < MapSize.Width; i++)
            for (var j = 0; j < MapSize.Height; j++)
            {
                var point = new Point(i, j);
                switch (islandsMap[i, j].tileType)
                {
                    case MapTileType.Island:
                        map[i, j] = new MapTile(point, -1, game, islandsMap[i, j].tileType, islandsMap[i, j].tileSprite);
                        break;
                    case MapTileType.Shallow:
                        allSeaTiles.Add(point);
                        map[i, j] = new MapTile(point, allSeaTiles.Count - 1, game, islandsMap[i, j].tileType,
                            islandsMap[i, j].tileSprite);
                        break;
                    case MapTileType.Sea:
                        allSeaTiles.Add(point);
                        map[i, j] = new MapTile(point, allSeaTiles.Count - 1, game, MapTileType.Sea, null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                map[i, j].SpriteBox.Location =
                    new Point(deltaFromBorders + i * Game.TileSize, j * Game.TileSize);
                mapCellControlPanel.Controls.Add(map[i, j].SpriteBox);
            }

            GetMapTile(Player.PlayerStartPosition).HasShipOnTile = true;
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
            var currentTile = map[shipMapPosition.X, shipMapPosition.Y];
            var finish = map[finishMapPosition.X, finishMapPosition.Y];
            (MapTile newTile, var finalDirection) = (null, new Point());
            var minPathLength = int.MaxValue;
            foreach (var direction in MapDirections.Keys.Skip(1))
            {
                var newPoint = new Point(shipMapPosition.X + direction.X, shipMapPosition.Y + direction.Y);
                if (!InBorders(newPoint) ||
                    GetMapTile(newPoint).HasShipOnTile ||
                    map[newPoint.X, newPoint.Y].TileType == MapTileType.Island ||
                    paths[map[newPoint.X, newPoint.Y].Index, finish.Index] > minPathLength) continue;
                minPathLength = paths[currentTile.Index, finish.Index];
                newTile = map[newPoint.X, newPoint.Y];
                finalDirection = direction;
            }

            return (newTile, finalDirection);
        }

        private static bool InBorders(Point point) =>
            point.X >= 0 && point.X < MapSize.Width && point.Y >= 0 && point.Y < MapSize.Height;

        public IEnumerable<MapTile> GetNeighborTiles(Point mapPosition) =>
            from direction in MapDirections.Keys
            select new Point(mapPosition.X + direction.X, mapPosition.Y + direction.Y)
            into newPoint
            where InBorders(newPoint) && map[newPoint.X, newPoint.Y].TileType != MapTileType.Island
            select map[newPoint.X, newPoint.Y];

        public MapTile GetMapTile(Point mapPosition)
        {
            if (!InBorders(mapPosition)) throw new IndexOutOfRangeException();
            return map[mapPosition.X, mapPosition.Y];
        }

        public int GetPathLengthToTile(MapTile startMapTile, MapTile endMapTile) =>
            paths[startMapTile.Index, endMapTile.Index];

        public static (Point originBorderPoint, Point newBorderPoint) GetMinPathBetweenMapCells(
            MapCell from,
            MapCell to,
            Direction direction,
            MapTile startTile,
            MapTile endTile)
        {
            var (fromBorderPoint, toBorderPoint, addValue) = GetPathPoints(direction);

            (Point originBorderPoint, Point newBorderPoint) result = (new Point(), new Point());
            var minPath = int.MaxValue;
            for (; InBorders(fromBorderPoint); fromBorderPoint += addValue, toBorderPoint += addValue)
            {
                var length = from.TileMap.GetPathLengthToTile(startTile, from.TileMap.GetMapTile(fromBorderPoint)) +
                             to.TileMap.GetPathLengthToTile(to.TileMap.GetMapTile(toBorderPoint), endTile);
                if (minPath <= length) continue;
                minPath = length;
                result = (fromBorderPoint, toBorderPoint);
            }

            return result;
        }

        private static (Point fromBorderPoint, Point toBorderPoint, Size addValue) GetPathPoints(Direction direction)
        {
            return direction switch
            {
                Direction.None => throw new ArgumentNullException(),
                Direction.Up => (new Point(0, 0), new Point(0, MapSize.Height - 1), new Size(1, 0)),
                Direction.Down => (new Point(0, MapSize.Height - 1), new Point(0, 0), new Size(1, 0)),
                Direction.Right => (new Point(MapSize.Width - 1, 0), new Point(0, 0), new Size(0, 1)),
                Direction.Left => (new Point(0, 0), new Point(MapSize.Width - 1, 0), new Size(0, 1)),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public IEnumerable<MapTile> GetHorizontalAndVerticalSeaTiles(Point startPosition)
        {
            var checks = new Dictionary<Direction, bool>
            {
                [Direction.Up] = false,
                [Direction.Down] = false,
                [Direction.Right] = false,
                [Direction.Left] = false,
            };
            for (var i = 1; ; i++)
            {
                if(checks.All(pair => pair.Value)) yield break;
                foreach (var point in MapDirections.Skip(1))
                {
                    if(checks[point.Value]) continue;
                    var newPoint = startPosition + new Size(point.Key.X * i, point.Key.Y * i);
                    if (!InBorders(newPoint) ||
                        GetMapTile(newPoint).TileType == MapTileType.Island)
                    {
                        checks[point.Value] = true;
                        continue;
                    }
                    if (GetMapTile(newPoint).HasShipOnTile) checks[point.Value] = true;
                    yield return GetMapTile(newPoint);
                }
            }
        }
    }
}