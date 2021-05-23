using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public enum Rotations
    {
        None = -1,
        Right = 0,
        Up = 90,
        Left = 180,
        Down = 270
    }

    public class Ship
    {
        public static IReadOnlyDictionary<Direction, Rotations> DirectionsRotations = new Dictionary<Direction, Rotations>
        {
            {Direction.None, Rotations.None},
            {Direction.Down, Rotations.Down},
            {Direction.Up, Rotations.Up},
            {Direction.Right, Rotations.Right},
            {Direction.Left, Rotations.Left}
        };

        public Point MapPosition { get; private set; }
        public readonly PictureBox SpriteBox;
        private Rotations currentRotation = Rotations.Down;
        protected readonly GameModel GameModel;
        public MapTile CurrentMapTile => GameModel.CurrentMapCell.GetMapTile(MapPosition);

        public Ship(Image sprite, Size spriteSize, Point mapPosition, PictureBox parentPictureBox, GameModel gameModel)
        {
            GameModel = gameModel;
            MapPosition = mapPosition;
            SpriteBox = new PictureBox
            {
                Image = sprite,
                Size = spriteSize,
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            parentPictureBox.Controls.Add(SpriteBox);
        }

        private void RotateTo(Rotations newRotation)
        {
            if(newRotation == Rotations.None) return;
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
            MapPosition = newTile.MapPosition;
        }

        public void BoardShip(Ship shipToBoard)
        {
            GameModel.MoveShipToNextTile(this, shipToBoard.CurrentMapTile);
            GameModel.DeleteShip(shipToBoard);
        }
    }
}
