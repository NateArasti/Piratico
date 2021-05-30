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
        private readonly Game game;
        private readonly Dictionary<Direction, Button> scoutButtons;

        private readonly Button scoutModeButton;

        public ScoutMode(ScoutData scoutData, Game game)
        {
            scoutModeButton = scoutData.ScoutModeButton;
            scoutButtons = scoutData.ScoutButtons;
            this.game = game;
            InitializeScoutButtons();
        }

        public bool IsScouting { get; private set; }
        public Direction LastScoutDirection { get; private set; } = Direction.None;
        public bool OnNewMapCell => LastScoutDirection != Direction.None;

        private void InitializeScoutButtons()
        {
            scoutModeButton.Click += (_, _) =>
            {
                if (game.PlayerDoingSomething) return;
                if (!IsScouting)
                {
                    if (game.IsInShootMode) game.ExitShootModeManually();
                    SwitchScoutButtonVisibility(true);
                }
                else
                {
                    game.SwitchToMapCell((Direction) (-(int) LastScoutDirection));
                    LastScoutDirection = Direction.None;
                    SwitchScoutButtonVisibility(false);
                }

                IsScouting = !IsScouting;
            };
            foreach (var button in scoutButtons)
            {
                var oppositeButton = scoutButtons[(Direction) (-(int) button.Key)];
                button.Value.Click += (_, _) => ScoutMoveButtonClick(oppositeButton, button.Key);
            }
        }

        private void ScoutMoveButtonClick(Button oppositeButton, Direction direction)
        {
            if (game.PlayerDoingSomething) return;
            if (game.IsInShootMode) game.ExitShootModeManually();
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

            game.SwitchToMapCell(direction);
        }

        private void SwitchScoutButtonVisibility(bool visible)
        {
            foreach (var button in scoutButtons.Values) button.Visible = visible;
        }

        public void ExitScoutModeManually()
        {
            if (OnNewMapCell)
            {
                scoutModeButton.PerformClick();
            }
            else
            {
                LastScoutDirection = Direction.None;
                IsScouting = false;
                SwitchScoutButtonVisibility(false);
            }
        }
    }
}