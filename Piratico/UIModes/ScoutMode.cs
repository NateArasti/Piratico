using System.Collections.Generic;
using System.Windows.Forms;

namespace Piratico
{
    public readonly struct ScoutData
    {
        public readonly Button ScoutModeButton;
        public readonly Dictionary<Direction, Button> ScoutButtons;

        public ScoutData(Button scoutModeButton, Dictionary<Direction, Button> scoutButtons)
        {
            ScoutModeButton = scoutModeButton;
            ScoutButtons = scoutButtons;
        }
    }

    public class ScoutMode
    {
        public bool IsScouting { get; private set; }
        public Direction LastScoutDirection { get; private set; } = Direction.None;
        public bool OnNewMapCell => LastScoutDirection != Direction.None;

        private readonly Button scoutModeButton;
        private readonly Dictionary<Direction, Button> scoutButtons;
        private readonly GameModel gameModel;

        public ScoutMode(ScoutData scoutData, GameModel gameModel)
        {
            scoutModeButton = scoutData.ScoutModeButton;
            scoutButtons = scoutData.ScoutButtons;
            this.gameModel = gameModel;
            InitializeScoutButtons();
        }

        private void InitializeScoutButtons()
        {
            scoutModeButton.Click += (sender, args) =>
            {
                if(gameModel.PlayerDoingSomething) return;
                if (!IsScouting)
                {
                    if (gameModel.IsInShootMode) gameModel.ExitShootModeManually();
                    SwitchScoutButtonVisibility(true);
                }
                else
                {
                    gameModel.SwitchToMapCell((Direction)(-(int)LastScoutDirection));
                    LastScoutDirection = Direction.None;
                    SwitchScoutButtonVisibility(false);
                }

                IsScouting = !IsScouting;
            };
            foreach (var button in scoutButtons)
            {
                var oppositeButton = scoutButtons[(Direction) (-(int) button.Key)];
                button.Value.Click += (sender, args) => ScoutMoveButtonClick(oppositeButton, button.Key);
            }
        }

        private void ScoutMoveButtonClick(Button oppositeButton, Direction direction)
        {
            if (gameModel.PlayerDoingSomething) return;
            if(gameModel.IsInShootMode) gameModel.ExitShootModeManually();
            if (oppositeButton.Visible)
            {
                SwitchScoutButtonVisibility(false);
                oppositeButton.Visible = true;
                LastScoutDirection = direction;
            }
            else
            {
                SwitchScoutButtonVisibility(true);
                LastScoutDirection = Direction.None;
            }
            gameModel.SwitchToMapCell(direction);
        }

        private void SwitchScoutButtonVisibility(bool visible)
        {
            foreach (var button in scoutButtons.Values) button.Visible = visible;
        }

        public void ExitScoutModeManually()
        {
            if(OnNewMapCell) scoutModeButton.PerformClick();
            else
            {
                LastScoutDirection = Direction.None;
                IsScouting = false;
                SwitchScoutButtonVisibility(false);
            }
        }
    }
}
