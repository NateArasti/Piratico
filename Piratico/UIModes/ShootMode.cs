using System.Windows.Forms;

namespace Piratico
{
    public class ShootMode
    {
        public bool IsInShootMode { get; private set; }

        private readonly GameModel gameModel;

        public ShootMode(GameModel gameModel, Button shootButton)
        {
            this.gameModel = gameModel;
            shootButton.Click += (sender, args) =>
            {
                if (gameModel.PlayerDoingSomething) return;
                if (gameModel.OnNewMapCell) gameModel.ExitScoutModeManually();
                ChangeTilesSprites(!IsInShootMode);
                IsInShootMode = !IsInShootMode;
            };
        }

        public void TurnOff()
        {
            IsInShootMode = false;
            ChangeTilesSprites(false);
        }

        private void ChangeTilesSprites(bool show)
        {
            var newImage = show ? Resources.ChosenSeaTile : Resources.SimpleSeaTile;
            foreach (var tile in gameModel
                .CurrentMapCell
                .GetHorizontalAndVerticalSeaTiles(gameModel.Player.MapPosition))
            {
                tile.SpriteBox.Image = newImage;
            }
        }
    }
}
