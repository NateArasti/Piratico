using System.Windows.Forms;

namespace Piratico
{
    public class ShootMode
    {
        private readonly Game game;

        public ShootMode(Game game, Button shootButton)
        {
            this.game = game;
            shootButton.Click += (_, _) =>
            {
                if (game.PlayerDoingSomething) return;
                if (game.OnNewMapCell) game.ExitScoutModeManually();
                ChangeTilesSprites(!IsInShootMode);
                IsInShootMode = !IsInShootMode;
            };
        }

        public bool IsInShootMode { get; private set; }

        public void TurnOff()
        {
            IsInShootMode = false;
            ChangeTilesSprites(false);
        }

        private void ChangeTilesSprites(bool show)
        {
            foreach (var tile in game
                .CurrentMapCell
                .TileMap
                .GetHorizontalAndVerticalSeaTiles(game.Player.MapPosition))
                tile.SpriteBox.Image = show ? Resources.ShootModeTile : tile.OriginalTile;
        }
    }
}