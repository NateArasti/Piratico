using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
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
        public static readonly Size MapSize = new Size(8, 6);
        public readonly int DeltaFromBorders;

        public static IReadOnlyDictionary<Directions, Point> MapDirections = new Dictionary<Directions, Point>
        {
            {Directions.Down, new Point(0, 1)},
            {Directions.Up, new Point(0, -1)},
            {Directions.Right, new Point(1, 0)},
            {Directions.Left, new Point(-1, 0)}
        };
        
        private readonly Dictionary<Directions, MapCell> neighbors;

        public readonly MapTile[,] Map = new MapTile[MapSize.Width, MapSize.Height];

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
                Map[i, j] = new MapTile(new Point(i, j));
                Map[i, j].SpriteBox.Location = new Point(DeltaFromBorders + i * GameModel.TileSize, j * GameModel.TileSize);
                MapCellController.Controls.Add(Map[i, j].SpriteBox);
            }
            GetPathsBetweenTiles();
        }

        private void GetPathsBetweenTiles()
        {
            //TODO:Нужно ускорить этот алгоритм...
            for (var i = 0; i < MapSize.Width; i++)
            for (var j = 0; j < MapSize.Height; j++)
            {
                var startCell = new Point(i, j);
                Map[i, j].PathsLengths[i, j] = 0;
                var visited = new HashSet<Point> { startCell };
                var queue = new Queue<Point>();
                queue.Enqueue(startCell);
                while (queue.Count != 0)
                {
                    var currentCell = queue.Dequeue();
                    visited.Add(currentCell);
                    for (var dx = -1; dx <= 1; dx++)
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if(Math.Abs(dx) + Math.Abs(dy) != 1)
                            continue;
                        var newCell = new Point(currentCell.X + dx, currentCell.Y + dy);
                        if (CheckIfInBorders(newCell.X, newCell.Y) &&
                            !visited.Contains(newCell) && 
                            Map[newCell.X, newCell.Y].TileType == MapTileType.Sea)
                        {
                            queue.Enqueue(newCell);
                            Map[i, j].PathsLengths[newCell.X, newCell.Y] =
                                Map[i, j].PathsLengths[currentCell.X, currentCell.Y] + 1;
                        }
                    }
                }
            }
        }

        public static bool CheckIfInBorders(int x, int y) => x >= 0 && x < MapSize.Width &&
                                                             y >= 0 && y < MapSize.Height;
    }
}
