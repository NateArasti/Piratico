using System;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class CannonBall
    {
        private const int Speed = 16;
        private readonly PictureBox pictureBox;
        private readonly Size positionDelta;
        private readonly Ship shooterShip;
        private bool isEnded;

        public CannonBall(Direction direction, Ship ship)
        {
            shooterShip = ship;
            pictureBox = new PictureBox
            {
                Location = ship.CurrentMapTile.SpriteBox.Location,
                Image = Resources.CannonBall,
                Size = new Size(Game.TileSize, Game.TileSize),
                SizeMode = PictureBoxSizeMode.CenterImage,
                ForeColor = Color.Transparent,
                BackgroundImage = Resources.SimpleSeaTile
            };
            ship.CurrentMapTile.SpriteBox.Parent.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            switch (direction)
            {
                case Direction.None:
                    throw new ArgumentException();
                case Direction.Up:
                    pictureBox.Location += new Size(0, -Game.TileSize / 2);
                    positionDelta = new Size(0, -Speed);
                    break;
                case Direction.Down:
                    pictureBox.Location += new Size(0, Game.TileSize / 2);
                    positionDelta = new Size(0, Speed);
                    break;
                case Direction.Right:
                    pictureBox.Location += new Size(Game.TileSize / 2, 0);
                    positionDelta = new Size(Speed, 0);
                    break;
                case Direction.Left:
                    pictureBox.Location += new Size(-Game.TileSize / 2, 0);
                    positionDelta = new Size(-Speed, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void StartMovement(Point endPoint, Action shootEndAction)
        {
            var timer = new Timer {Interval = 20};
            timer.Tick += (_, _) =>
            {
                if (isEnded)
                {
                    timer.Stop();
                    shootEndAction();
                    pictureBox.Parent.Controls.Remove(pictureBox);
                    shooterShip.IsShooting = false;
                    if (shooterShip is not Player playerShip) return;
                    playerShip.EndStep();
                }
                else
                {
                    Move(endPoint);
                }
            };
            timer.Start();
        }

        private void Move(Point endPoint)
        {
            pictureBox.Location += positionDelta;
            if (Math.Abs(endPoint.X - pictureBox.Location.X) < Game.TileSize &&
                Math.Abs(endPoint.Y - pictureBox.Location.Y) < Game.TileSize)
                isEnded = true;
        }
    }
}