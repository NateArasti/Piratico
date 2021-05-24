using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public class Ship
    {
        public static IReadOnlyDictionary<Direction, Rotation> DirectionsRotations = new Dictionary<Direction, Rotation>
        {
            {Direction.None, Rotation.None},
            {Direction.Down, Rotation.Down},
            {Direction.Up, Rotation.Up},
            {Direction.Right, Rotation.Right},
            {Direction.Left, Rotation.Left}
        };

        public readonly PictureBox SpriteBox;

        public MapTile CurrentMapTile => GameModel.CurrentMapCell.GetMapTile(MapPosition);
        public Point MapPosition { get; private set; }

        protected ShipParams ShipParams;

        private Rotation currentRotation = Rotation.Down;
        public bool IsShooting;
        protected readonly GameModel GameModel;

        public Ship(Image sprite, Size spriteSize, Point mapPosition, PictureBox parentPictureBox, GameModel gameModel)
        {
            GameModel = gameModel;
            MapPosition = mapPosition;
            SpriteBox = new PictureBox
            {
                Parent = parentPictureBox,
                Image = sprite,
                Size = spriteSize,
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom
            };
        }

        private void RotateTo(Rotation newRotation)
        {
            if(newRotation == Rotation.None) return;
            var delta = newRotation > currentRotation
                ? 360 - (newRotation - currentRotation)
                : currentRotation - newRotation;
            switch (delta)
            {
                case 90:
                    SpriteBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 180:
                    SpriteBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270:
                    SpriteBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            currentRotation = newRotation;
        }

        public void MoveToNextTile(MapTile newTile, Point direction = new Point())
        {
            RotateTo(DirectionsRotations[TileMap.MapDirections[direction]]);
            CurrentMapTile.HasShipOnTile = false;
            newTile.HasShipOnTile = true;
            MapPosition = newTile.MapPosition;
        }

        public void BoardShip(Ship shipToBoard)
        {
            GameModel.MoveShipToNextTile(this, shipToBoard.CurrentMapTile);
            GameModel.DeleteShip(shipToBoard);
        }

        public void Shoot(Ship shipToShoot)
        {
            Direction direction;
            if (shipToShoot.MapPosition.Y == MapPosition.Y)
                direction = shipToShoot.MapPosition.X < MapPosition.X ? Direction.Left : Direction.Right;
            else if(shipToShoot.MapPosition.X == MapPosition.X)
                direction = shipToShoot.MapPosition.Y < MapPosition.Y ? Direction.Up : Direction.Down;
            else 
                return;
            var ball = new CannonBall(direction, this);
            ball.StartMovement(shipToShoot.SpriteBox.Parent.Location);
            GameModel.ExitShootModeManually();
        }
    }
}
