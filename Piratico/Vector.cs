using System;

namespace Piratico
{
    class Vector
    {
        protected bool Equals(Vector other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
        
        public int X { get; }
        public int Y { get; }

        public int SqrLength => X * X + Y * Y;

        public static Vector Zero => new Vector(0, 0);
        public static Vector Right => new Vector(1, 0);
        public static Vector Left => new Vector(-1, 0);
        public static Vector Up => new Vector(0, -1);
        public static Vector Down => new Vector(0, 1);

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int? Angle()
        {
            if (X == 0)
            {
                if (Y == 0) return null;
                if (Y > 0) return 270;
                return 90;
            }

            if (Y == 0)
            {
                if (X > 0) return 0;
                return 180;
            }
            return (int) -Math.Atan((double) Y / X);
        }

        public static bool operator ==(Vector v1, Vector v2) => !(v2 is null) && !(v1 is null) && v1.X == v2.X && v1.Y == v2.Y;
        public static bool operator !=(Vector v1, Vector v2) => !(v2 is null) && !(v1 is null) && (v1.X != v2.X || v1.Y != v2.Y);
        public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.X - v2.X, v1.Y - v2.Y);
        public static Vector operator *(Vector v1, int a) => new Vector(a * v1.X, a * v1.Y);
        public static Vector operator *(int a, Vector v1) => new Vector(a * v1.X, a * v1.Y);
        public static Vector operator /(Vector v1, int a) => new Vector(v1.X / a, v1.Y / a);
    }
}
