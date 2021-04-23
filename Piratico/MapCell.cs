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
        public static readonly Size MapSize = new Size(4, 3);
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
        private readonly List<Point> allTiles = new List<Point>();

        public readonly Panel MapCellController;

        public MapCell()
        {
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

        private MapCell(Directions direction, MapCell neighbor) : this()
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
            for (var i = 0; i < MapSize.Width; i++)
            for (var j = 0; j < MapSize.Height; j++)
            {
                var point = new Point(i, j);
                allTiles.Add(point);
                Map[i, j] = new MapTile(point, allTiles.Count - 1);
                Map[i, j].SpriteBox.Location = new Point(DeltaFromBorders + i * GameModel.TileSize, j * GameModel.TileSize);
                MapCellController.Controls.Add(Map[i, j].SpriteBox);
            }
            GetPathsBetweenTiles();
        }

        private void GetPathsBetweenTiles()
        {
            const int infinite = int.MaxValue;
            var length = allTiles.Count;
            for (var i = 0; i < length; i++)
            for (var j = i; j < length; j++)
            {
                if (i == j) paths[i, j] = Tuple.Create(0, Map[allTiles[i].X, allTiles[i].Y]);
                else if (MapDirections.ContainsKey(new Point(allTiles[i].X - allTiles[j].X,
                    allTiles[i].Y - allTiles[j].Y)))
                {
                    paths[i, j] = Tuple.Create(1, Map[allTiles[i].X, allTiles[i].Y]);
                    paths[j, i] = Tuple.Create(1, Map[allTiles[j].X, allTiles[j].Y]);
                }
                else
                {
                    paths[i, j] = Tuple.Create(infinite, Map[allTiles[j].X, allTiles[j].Y]);
                    paths[j, i] = Tuple.Create(infinite, Map[allTiles[i].X, allTiles[i].Y]);
                }
            }
            for (var k = 0; k < length; ++k)
            for (var i = 0; i < length; ++i)
            for (var j = 0; j < length; ++j)
            {
                if (paths[i, k].Item1 < infinite && paths[k, j].Item1 < infinite &&
                    paths[i, j].Item1 > paths[i, k].Item1 + paths[k, j].Item1)
                    paths[i, j] = Tuple.Create(paths[i, k].Item1 + paths[k, j].Item1,
                        Map[allTiles[k].X, allTiles[k].Y]);
            }
        }

        public void FillPathBetweenMapTiles(Queue<MapTile> path, MapTile start, MapTile finish)
        {
            if(paths[start.Index, finish.Index].Item2 != start)
            {
                FillPathBetweenMapTiles(path, start, paths[start.Index, finish.Index].Item2);
                FillPathBetweenMapTiles(path, paths[start.Index, finish.Index].Item2, finish);
                path.Enqueue(paths[start.Index, finish.Index].Item2);
            }
        }
    }
}
