using System;
using System.Collections.Generic;
using System.Drawing;

namespace Piratico
{
    class IslandsGenerator
    {
        private static readonly (Bitmap bigSprite, MapTileType[,] tileTypes, Image[,] tileSprites)[] Islands =
        {
            (
                //Размеры массивов указаны для удобства
                // ReSharper disable RedundantExplicitArraySize
                Resources.SimpleIsland,
                new MapTileType[5, 5]
                {
                    {MapTileType.Shallow, MapTileType.Shallow, MapTileType.Shallow, MapTileType.Shallow, MapTileType.Shallow},
                    {MapTileType.Shallow, MapTileType.Island, MapTileType.Island, MapTileType.Island, MapTileType.Shallow},
                    {MapTileType.Shallow, MapTileType.Island, MapTileType.Island, MapTileType.Island, MapTileType.Shallow},
                    {MapTileType.Shallow, MapTileType.Island, MapTileType.Island, MapTileType.Island, MapTileType.Shallow},
                    {MapTileType.Shallow, MapTileType.Shallow, MapTileType.Shallow, MapTileType.Shallow, MapTileType.Shallow}
                },
                new Image[5, 5]
                {
                    {null, null, null, null, null},
                    {null, null, null, null, null},
                    {null, null, null, null, null},
                    {null, null, null, null, null},
                    {null, null, null, null, null}
                }
            )
        };

        private static readonly Point CheckPoint = new Point(-1, -1);
        private static readonly Random Random = new Random();

        public static (MapTileType tileType, Image tileSprite)[,] GenerateIslands(int mapWidth, int mapHeight)
        {
            var resultMap = new (MapTileType tileType, Image sprite)[mapWidth, mapHeight];
            for (var i = 0; i < resultMap.GetLength(0); i++)
            for (var j = 0; j < resultMap.GetLength(1); j++)
                resultMap[i, j] = (MapTileType.Sea, null);

            const int percentDelta = 30;
            for (var chanceOfGeneration = 95; chanceOfGeneration > 0; chanceOfGeneration -= percentDelta)
            {
                if (Random.Next(101) >= chanceOfGeneration) break;
                var (spot, islandIndex) = FindSpotForIsland(resultMap);
                if(spot != CheckPoint)
                    PutIslandOnMap(resultMap, spot, islandIndex);
            }

            return resultMap;
        }

        private static (Point spot, int islandIndex) FindSpotForIsland((MapTileType tileType, Image tileSprite)[,] currentMap)
        {
            var possibleResults = new List<(Point spot, int islandIndex)>();
            for (var i = 0; i < currentMap.GetLength(0); i++)
            for (var j = 0; j < currentMap.GetLength(1); j++)
            {
                var point = new Point(i, j);
                for (var k = 0; k < Islands.Length; k++)
                    if (CheckIfFits(point, k, currentMap))
                        possibleResults.Add((point, k));
            }
            return possibleResults.Count == 0 ? (CheckPoint, -1) : possibleResults[Random.Next(possibleResults.Count)];
        }

        private static bool CheckIfFits(Point spot, int islandIndex, (MapTileType tileType, Image tileSprite)[,] currentMap)
        {
            var island = Islands[islandIndex].tileTypes;
            var width = island.GetLength(0);
            var height = island.GetLength(1);
            if (spot.X + width >= currentMap.GetLength(0) || spot.Y + height >= currentMap.GetLength(1) ||
                spot.X <= Player.PlayerStartPosition.X && Player.PlayerStartPosition.X < spot.X + width ||
                spot.Y <= Player.PlayerStartPosition.Y && Player.PlayerStartPosition.Y < spot.Y + height) return false;
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (currentMap[spot.X + i, spot.Y + j].tileType != MapTileType.Sea && 
                    island[i, j] != MapTileType.Sea)
                    return false;
            return true;
        }

        private static void PutIslandOnMap((MapTileType tileType, Image tileSprite)[,] currentMap, Point spot, int islandIndex)
        {
            var (bitmap, tileTypes, tiles) = Islands[islandIndex];
            var width = tileTypes.GetLength(0);
            var height = tileTypes.GetLength(1);
            var spriteSize = new Size(bitmap.Width / width,
                bitmap.Height / height);
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                if (tileTypes[i, j] != MapTileType.Sea)
                {
                    if (tiles[i, j] == null)
                        tiles[i, j] = CropImage(Islands[islandIndex].bigSprite,
                            new Rectangle(i * spriteSize.Width, j * spriteSize.Width, 
                                spriteSize.Width, spriteSize.Height));
                    currentMap[spot.X + i, spot.Y + j] = (tileTypes[i, j], tiles[i, j]);
                }
            }
        }

        private static Image CropImage(Image image, Rectangle selection)
        {
            var bmp = new Bitmap(image);
            var cropBmp = bmp.Clone(selection, bmp.PixelFormat);

            return cropBmp;
        }
    }
}
