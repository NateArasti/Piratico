using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public enum MapTileType
    {
        Sea,
        Island
    }

    class MapTile
    {
        private readonly Image seaTile = Resources.SimpleSeaTile;
        private readonly Image chosenSeaTile = Resources.ChosenSeaTile;
        public readonly MapTileType TileType;
        public readonly PictureBox SpriteBox;

        public readonly Point MapPosition;
        public readonly int[,] PathsLengths = new int[MapCell.MapSize.Width, MapCell.MapSize.Height];

        public MapTile(Point mapPosition)
        {
            MapPosition = mapPosition;
            TileType = MapTileType.Sea;
            SpriteBox = new PictureBox
            {
                Image = seaTile,
                Size = new Size(GameModel.TileSize, GameModel.TileSize),
                SizeMode = PictureBoxSizeMode.Zoom,
                ForeColor = Color.Transparent,
                BackColor = Color.Transparent
            };
            SpriteBox.MouseEnter += (sender, args) => SpriteBox.Image = chosenSeaTile;
            SpriteBox.MouseLeave += (sender, args) => SpriteBox.Image = seaTile;
            SpriteBox.MouseDoubleClick += (sender, args) => GameModel.MovePlayerToNewTile(this);
        }
    }
}
