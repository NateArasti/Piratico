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
        private readonly Image chosenTile = Resources.ChosenTile;
        public readonly int Index;

        public readonly Point MapPosition;
        public readonly Image OriginalTile;
        public readonly PictureBox SpriteBox;

        public readonly MapTileType TileType;

        public bool HasShipOnTile;

        public MapTile(Point mapPosition, int index, Game game, MapTileType tileType, Image sprite)
        {
            Index = index;
            MapPosition = mapPosition;
            TileType = tileType;
            OriginalTile = sprite ?? Resources.SimpleSeaTile;
            SpriteBox = new PictureBox
            {
                Image = OriginalTile,
                Size = new Size(Game.TileSize, Game.TileSize),
                SizeMode = PictureBoxSizeMode.Zoom,
                ForeColor = Color.Transparent,
                BackColor = Color.Transparent
            };

            if (TileType != MapTileType.Island)
            {
                SpriteBox.MouseEnter += (_, _) =>
                {
                    if (game.IsInShootMode) return;
                    SpriteBox.Image = chosenTile;
                };
                SpriteBox.MouseLeave += (_, _) =>
                {
                    if (game.IsInShootMode) return;
                    SpriteBox.Image = OriginalTile;
                };
                SpriteBox.MouseDoubleClick += (_, _) =>
                {
                    if (game.IsInShootMode) return;
                    if (game.OnNewMapCell)
                        game.MoveToNewMapCell(this);
                    else
                        game.Player.StartMovement(() =>
                        {
                            game.MoveShipToNextTile(game.Player, this);
                            game.Player.IsMoving = game.Player.CurrentMapTile != this;
                        });
                };
            }
        }
    }
}