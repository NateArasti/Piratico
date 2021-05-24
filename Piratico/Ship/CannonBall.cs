using System;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class CannonBall
    {
        private readonly Ship shooterShip;
        private readonly PictureBox pictureBox;
        private bool isEnded;
        private readonly Size positionDelta;
        private const int Speed = 16;

        public CannonBall(Direction direction, Ship ship)
        {
            shooterShip = ship;
            pictureBox = new PictureBox
            {
                Location = ship.CurrentMapTile.SpriteBox.Location,
                Image = Resources.CannonBall,
                Size = new Size(GameModel.TileSize, GameModel.TileSize),
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
                    pictureBox.Location += new Size(0, -GameModel.TileSize / 2);
                    positionDelta = new Size(0, -Speed);
                    break;
                case Direction.Down:
                    pictureBox.Location += new Size(0, GameModel.TileSize / 2);
                    positionDelta = new Size(0, Speed);
                    break;
                case Direction.Right:
                    pictureBox.Location += new Size(GameModel.TileSize / 2, 0);
                    positionDelta = new Size(Speed, 0);
                    break;
                case Direction.Left:
                    pictureBox.Location += new Size(-GameModel.TileSize / 2, 0);
                    positionDelta = new Size(-Speed, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void StartMovement(Point endPoint)
        {
            var timer = new Timer {Interval = 20};
            timer.Tick += (sender, args) =>
            {
                if (isEnded)
                {
                    timer.Stop();
                    pictureBox.Parent.Controls.Remove(pictureBox);
                    shooterShip.IsShooting = false;
                    if (shooterShip is Player player) player.StepEnded = true;
                }
                else Move(endPoint);
            };
            timer.Start();
            shooterShip.IsShooting = true;
        }

        private void Move(Point endPoint)
        {
            pictureBox.Location += positionDelta;
            if (Math.Abs(endPoint.X - pictureBox.Location.X) < GameModel.TileSize &&
                Math.Abs(endPoint.Y - pictureBox.Location.Y) < GameModel.TileSize)
                isEnded = true;
        }
    }
}
