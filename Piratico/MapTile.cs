using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public enum MapTileType
    {
        Sea,
        Shallow,
        Island
    }

    public class MapTile
    {
        private readonly Image chosenTile = Resources.ChosenSeaTile;
        public readonly MapTileType TileType;
        public readonly PictureBox SpriteBox;

        public readonly Point MapPosition;
        public readonly int Index;

        public bool HasShipOnTile { get; set; }

        public MapTile(Point mapPosition, int index, GameModel gameModel, MapTileType tileType, Image sprite)
        {
            Index = index;
            MapPosition = mapPosition;
            TileType = tileType;
            var originalTile = sprite ?? Resources.SimpleSeaTile;
            SpriteBox = new PictureBox
            {
                Image = originalTile,
                Size = new Size(GameModel.TileSize, GameModel.TileSize),
                SizeMode = PictureBoxSizeMode.Zoom,
                ForeColor = Color.Transparent,
                BackColor = Color.Transparent
            };

            if(TileType != MapTileType.Island)
            {
                SpriteBox.MouseEnter += (sender, args) => SpriteBox.Image = chosenTile;
                SpriteBox.MouseLeave += (sender, args) => SpriteBox.Image = originalTile;
                SpriteBox.MouseDoubleClick += (sender, args) =>
                {
                    if (gameModel.IsScouting)
                        gameModel.MoveToNewMapCell();
                    else
                        gameModel.MovePlayerToNewTile(this);
                };
            }
        }
    }
}
