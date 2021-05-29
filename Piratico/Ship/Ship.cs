using System;
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

        public MapTile CurrentMapTile => Game.CurrentMapCell.TileMap.GetMapTile(MapPosition);
        public Point MapPosition { get; private set; }
        public bool IsShooting { get; set; }

        protected ShipParams ShipParams;
        protected readonly Game Game;
        protected IReadOnlyList<Image> StateSprites;
        protected bool IsDead;

        private Rotation currentRotation = Rotation.Down;

        public Ship(Image sprite, Size spriteSize, Point mapPosition, PictureBox parentPictureBox, Game game)
        {
            Game = game;
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
            var delta = newRotation > currentRotation ?
                360 - (newRotation - currentRotation) :
                currentRotation - newRotation;
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

        public void MoveToNextTile(MapTile newTile, Point direction = new())
        {
            RotateTo(DirectionsRotations[TileMap.MapDirections[direction]]);
            CurrentMapTile.HasShipOnTile = false;
            newTile.HasShipOnTile = true;
            MapPosition = newTile.MapPosition;
        }

        public void BoardShip(Ship shipToBoard)
        {
            if(shipToBoard.IsDead) CollectResources(shipToBoard.ShipParams, true);
            while (!IsDead && !shipToBoard.IsDead)
            {
                ApplyDamage(shipToBoard.CountDamage());
                if(IsDead)
                {
                    shipToBoard.CollectResources(shipToBoard.ShipParams);
                    return;
                }
                shipToBoard.ApplyDamage(CountDamage());
                if (shipToBoard.IsDead)
                {
                    CollectResources(ShipParams);
                    return;
                }
            }
        }

        private void CollectResources(ShipParams shipParams, bool shipWasDead = false)
        {
            if (shipParams.IsCollected) return;
            shipParams.MarkCollected();
            var collectionModifier = shipWasDead ? 0.3 : 0.6;
            ShipParams.AddCollectedResources(
                (int)(shipParams.CrewAmount * collectionModifier),
                (int)(shipParams.Consumables * collectionModifier),
                (int)(shipParams.Gold * collectionModifier));
            if (this is Player)
                Game.UpdatePlayerResourcesUI();
        }

        public void Shoot(Ship shipToShoot)
        {
            IsShooting = true;
            Direction direction;
            if (shipToShoot.MapPosition.Y == MapPosition.Y)
                direction = shipToShoot.MapPosition.X < MapPosition.X ? Direction.Left : Direction.Right;
            else if (shipToShoot.MapPosition.X == MapPosition.X)
                direction = shipToShoot.MapPosition.Y < MapPosition.Y ? Direction.Up : Direction.Down;
            else
                return;
            var ball = new CannonBall(direction, this);
            ball.StartMovement(shipToShoot.SpriteBox.Parent.Location, () => shipToShoot.ApplyDamage(CountDamage()));
        }

        private int CountDamage()
        {
            var random = new Random();
            return (int)(ShipParams.MaxDamage *
                         (ShipParams.Strength / 100.0 * 
                          ((double)ShipParams.CrewAmount / ShipParams.MaxCrewAmount) +
                          random.NextDouble()));
        }

        private void ApplyDamage(int damage)
        {
            ShipParams.CalculateDamageEffects(damage);
            CheckCurrentShipState();
            if(this is Player)
                Game.UpdatePlayerResourcesUI();
        }

        protected void CheckCurrentShipState()
        {
            SpriteBox.Image = ShipParams.Strength switch
            {
                > 66 => StateSprites[0],
                > 33 => StateSprites[1],
                > 0 => StateSprites[2],
                <= 0 => StateSprites[3]
            };
            var previousRotation = currentRotation;
            currentRotation = Rotation.Down;
            RotateTo(previousRotation);

            if (ShipParams.Strength > 0) return;
            IsDead = true;
            var count = 0;
            var endDelay = new Timer() {Interval = 200};
            endDelay.Tick += (_, _) =>
            {
                count += 1;
                if (count != 5) return;
                endDelay.Stop();
                Game.DeleteShip(this);
            };
            endDelay.Start();
        }
    }
}
