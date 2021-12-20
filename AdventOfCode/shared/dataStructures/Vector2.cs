using System;

namespace AdventOfCode.shared.dataStructures
{
    public struct Vector2
    {

        public float x { get; private set; }
        public float y { get; private set; }

        public static readonly Vector2 Zero = new Vector2(0, 0);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.x + b.x, a.y + b.y);

        public static Vector2 operator +(Vector2 a, float b)
            => new Vector2(a.x + b, a.y + b);

        public static Vector2 operator -(Vector2 a, Vector2 b)
            => new Vector2(a.x - b.x, a.y - b.y);

        public static Vector2 operator -(Vector2 a, float b)
            => new Vector2(a.x - b, a.y - b);

        public static Vector2 operator /(Vector2 a, Vector2 b)
            => new Vector2(a.x / b.x, a.y / b.y);

        public static Vector2 operator /(Vector2 a, float b)
            => new Vector2(a.x / b, a.y / b);

        public static Vector2 operator *(Vector2 a, float b)
            => new Vector2(a.x * b, a.y * b);

        public static bool operator ==(Vector2 a, Vector2 b)
             => (int)a.x == (int)b.x && (int)a.y == (int)b.y;

        public static bool operator !=(Vector2 a, Vector2 b)
            => !(a == b);

        public Vector2 Normalize()
        {
            var magnitude = this.GetMagnitude();
            return new Vector2(x / magnitude, y / magnitude);
        }

        public float GetMagnitude()
        {
            return (float) Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        /// <summary>
        /// This is some really dumb method to return the maximum of X and Y. By default, Math.Max(x, y) returns NaN if either x or y is NaN... which sucks.
        /// </summary>
        /// <returns></returns>
        public float GetMaxOfXandY()
        {
            if (float.IsNaN(x) && float.IsNaN(y))
            {
                return 0;
            }
            else if (float.IsNaN(x))
            {
                return y;
            }
            else if (float.IsNaN(y))
            {
                return x;
            }
            else
            {
                return Math.Max(x, y);
            }
        }
    }
}
