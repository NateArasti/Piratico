using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piratico
{
    public enum Rotations
    {
        Right = 0,
        Up = 90,
        Left = 180,
        Down = 270
    }

    // Player сейчас почти ничего не умеет (судя по коду).
    // Он пассивен. В нем должна быть логика его действий.
    internal class Player
    {
        public static IReadOnlyDictionary<Directions, Rotations> DirectionsRotations = new Dictionary<Directions, Rotations>
        {
            {Directions.Down, Rotations.Down},
            {Directions.Up, Rotations.Up},
            {Directions.Right, Rotations.Right},
            {Directions.Left, Rotations.Left}
        };

        public Point MapPosition { get; set; }
        public readonly PictureBox SpriteBox;
        private Rotations currentRotation = Rotations.Down;

        public Player(Image sprite, Point position, Size spriteSize, Point mapPosition)
        {
            MapPosition = mapPosition;
            SpriteBox = new PictureBox
            {
                Image = sprite,
                Size = spriteSize,
                Location = position,
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom
            };
        }

        public void RotateTo(Rotations newRotation)
        {
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
    }
}
