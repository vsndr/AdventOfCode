using System;

namespace AdventOfCode.shared.dataStructures
{
    public struct Vector3
    {
        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
            => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vector3 operator +(Vector3 a, float b)
            => new Vector3(a.x + b, a.y + b, a.z + b);

        public static Vector3 operator -(Vector3 a, Vector3 b)
            => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);

        public static Vector3 operator -(Vector3 a, float b)
            => new Vector3(a.x - b, a.y - b, a.z - b);

        public static Vector3 operator /(Vector3 a, Vector3 b)
            => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);

        public static Vector3 operator /(Vector3 a, float b)
            => new Vector3(a.x / b, a.y / b, a.z / b);

        public static Vector3 operator *(Vector3 a, float b)
            => new Vector3(a.x * b, a.y * b, a.z * b);

        public static bool operator ==(Vector3 a, Vector3 b)
             => (int)a.x == (int)b.x && (int)a.y == (int)b.y && (int)a.z == (int)b.z;

        public static bool operator !=(Vector3 a, Vector3 b)
            => !(a == b);

        public double GetMagnitude()
        {
            return Math.Sqrt(x*x + y*y + z*z);
        }
    }
}
