using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public enum Directions
    {
        Up,
        Down,
        Right,
        Left
    }

    internal class MapCell
    {
        private readonly GameModel gameModel;

        public static readonly Size MapSize = new Size(20, 15);
        public readonly int DeltaFromBorders;

        public static IReadOnlyDictionary<Point, Directions> MapDirections = new Dictionary<Point, Directions>
        {
            {new Point(0, 1), Directions.Down},
            {new Point(0, -1), Directions.Up},
            {new Point(1, 0), Directions.Right},
            {new Point(-1, 0), Directions.Left}
        };

        private readonly Dictionary<Directions, MapCell> neighbors;

        public readonly MapTile[,] Map = new MapTile[MapSize.Width, MapSize.Height];

        private readonly Tuple<int, MapTile>[,] paths = new Tuple<int, MapTile>[MapSize.Width * MapSize.Height, MapSize.Width * MapSize.Height];
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
            neighbors = new Dictionary<Directions, MapCell>
            {
                {Directions.Down, null},
                {Directions.Up, null},
                {Directions.Left, null},
                {Directions.Right, null}
            };
            GenerateMap();
        }

        private MapCell(Directions direction, MapCell neighbor) : this(neighbor.gameModel)
        {
            switch (direction)
            {
                case Directions.Down:
                    neighbors[Directions.Up] = neighbor;
                    break;
                case Directions.Up:
                    neighbors[Directions.Down] = neighbor;
                    break;
                case Directions.Left:
                    neighbors[Directions.Right] = neighbor;
                    break;
                case Directions.Right:
                    neighbors[Directions.Left] = neighbor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void GenerateNeighbors()
        {
            for (var i = 0; i < 4; i++)
            {
                var direction = (Directions) i;
                if (neighbors[direction] == null)
                    neighbors[direction] = new MapCell(direction, this);
            }
        }

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
                    default:
                        allSeaTiles.Add(point);
                        Map[i, j] = new MapTile(point, allSeaTiles.Count - 1, gameModel, MapTileType.Sea, null);
                        break;
                }
                Map[i, j].SpriteBox.Location = new Point(DeltaFromBorders + i * GameModel.TileSize, j * GameModel.TileSize);
                MapCellController.Controls.Add(Map[i, j].SpriteBox);
            }
            GetPathsBetweenTiles();
        }

        private void GetPathsBetweenTiles()
        {
            const int infinite = int.MaxValue;
            var length = allSeaTiles.Count;
            for (var i = 0; i < length; i++)
            for (var j = i; j < length; j++)
            {
                if (i == j) paths[i, j] = Tuple.Create(0, Map[allSeaTiles[i].X, allSeaTiles[i].Y]);
                else if (MapDirections.ContainsKey(new Point(allSeaTiles[i].X - allSeaTiles[j].X,
                    allSeaTiles[i].Y - allSeaTiles[j].Y)))
                {
                    paths[i, j] = Tuple.Create(1, Map[allSeaTiles[i].X, allSeaTiles[i].Y]);
                    paths[j, i] = Tuple.Create(1, Map[allSeaTiles[j].X, allSeaTiles[j].Y]);
                }
                else
                {
                    paths[i, j] = Tuple.Create(infinite, Map[allSeaTiles[j].X, allSeaTiles[j].Y]);
                    paths[j, i] = Tuple.Create(infinite, Map[allSeaTiles[i].X, allSeaTiles[i].Y]);
                }
            }
            for (var k = 0; k < length; ++k)
            for (var i = 0; i < length; ++i)
            for (var j = 0; j < length; ++j)
            {
                if (paths[i, k].Item1 < infinite && paths[k, j].Item1 < infinite &&
                    paths[i, j].Item1 > paths[i, k].Item1 + paths[k, j].Item1)
                    paths[i, j] = Tuple.Create(paths[i, k].Item1 + paths[k, j].Item1,
                        Map[allSeaTiles[k].X, allSeaTiles[k].Y]);
            }
        }

        public void FillPathBetweenMapTiles(HashSet<MapTile> path, MapTile start, MapTile finish)
        {
            if(paths[start.Index, finish.Index].Item2 != start)
            {
                FillPathBetweenMapTiles(path, paths[start.Index, finish.Index].Item2, finish);
                FillPathBetweenMapTiles(path, start, paths[start.Index, finish.Index].Item2);
                path.Add(paths[start.Index, finish.Index].Item2);
            }
        }

        public (MapTile newTile, Point finalDirection) GetNextPlayerMove(HashSet<MapTile> path, Point playerMapPosition)
        {
            (MapTile newTile, Point finalDirection) = (null, new Point());
            foreach (var direction in MapCell.MapDirections.Keys)
            {
                var newPoint = new Point(playerMapPosition.X + direction.X, playerMapPosition.Y + direction.Y);
                if (!InBorders(newPoint) || !path.Contains(Map[newPoint.X, newPoint.Y])) continue;
                newTile = Map[newPoint.X, newPoint.Y];
                path.Remove(newTile);
                finalDirection = direction;
                break;
            }

            return (newTile, finalDirection);
        }

        private static bool InBorders(Point point) => point.X >= 0 && point.X < MapSize.Width && point.Y >= 0 && point.Y < MapSize.Height;
    }
}
