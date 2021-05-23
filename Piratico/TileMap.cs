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
        private readonly GameModel gameModel;
        public readonly MapTile[,] Map = new MapTile[MapSize.Width, MapSize.Height];
        private readonly int[,] paths = new int[MapSize.Width * MapSize.Height, MapSize.Width * MapSize.Height];
        private readonly List<Point> allSeaTiles = new List<Point>();

        public TileMap(int deltaFromBorders, GameModel gameModel, Panel mapCellControlPanel)
        {
            this.deltaFromBorders = deltaFromBorders;
            this.gameModel = gameModel;
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
                switch (islandsMap[i, j].Item1)
                {
                    case MapTileType.Island:
                        Map[i, j] = new MapTile(point, -1, gameModel, islandsMap[i, j].Item1, islandsMap[i, j].Item2);
                        break;
                    case MapTileType.Shallow:
                        allSeaTiles.Add(point);
                        Map[i, j] = new MapTile(point, allSeaTiles.Count - 1, gameModel, islandsMap[i, j].Item1,
                            islandsMap[i, j].Item2);
                        break;
                    case MapTileType.Sea:
                        allSeaTiles.Add(point);
                        Map[i, j] = new MapTile(point, allSeaTiles.Count - 1, gameModel, MapTileType.Sea, null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Map[i, j].SpriteBox.Location =
                    new Point(deltaFromBorders + i * GameModel.TileSize, j * GameModel.TileSize);
                mapCellControlPanel.Controls.Add(Map[i, j].SpriteBox);
            }

            Map[Player.PlayerStartPosition.X, Player.PlayerStartPosition.Y].HasShipOnTile = true;
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

        private static bool InBorders(Point point) =>
            point.X >= 0 && point.X < MapSize.Width && point.Y >= 0 && point.Y < MapSize.Height;

        public IEnumerable<MapTile> GetNeighborTiles(Point mapPosition) =>
            from direction in MapDirections.Keys
            select new Point(mapPosition.X + direction.X, mapPosition.Y + direction.Y)
            into newPoint
            where InBorders(newPoint) && Map[newPoint.X, newPoint.Y].TileType != MapTileType.Island
            select Map[newPoint.X, newPoint.Y];

        public MapTile GetMapTile(Point mapPosition)
        {
            if (!InBorders(mapPosition)) throw new IndexOutOfRangeException();
            return Map[mapPosition.X, mapPosition.Y];
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
                var length = from.GetPathLengthToTile(startTile, from.GetMapTile(fromBorderPoint)) +
                             to.GetPathLengthToTile(to.GetMapTile(toBorderPoint), endTile);
                if (minPath <= length) continue;
                minPath = length;
                result = (fromBorderPoint, toBorderPoint);
            }

            return result;
        }

        private static (Point fromBorderPoint, Point toBorderPoint, Size addValue) GetPathPoints(Direction direction)
        {
            switch (direction)
            {
                case Direction.None:
                    throw new ArgumentNullException();
                case Direction.Up:
                    return (new Point(0, 0), new Point(0, MapSize.Height - 1), new Size(1, 0));
                case Direction.Down:
                    return (new Point(0, MapSize.Height - 1), new Point(0, 0), new Size(1, 0));
                case Direction.Right:
                    return (new Point(MapSize.Width - 1, 0), new Point(0, 0), new Size(0, 1));
                case Direction.Left:
                    return (new Point(0, 0), new Point(MapSize.Width - 1, 0), new Size(0, 1));
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
