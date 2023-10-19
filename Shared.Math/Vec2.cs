using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Remouse.Shared.Math
{
    static class MethodImplOptionsEx
    {
        public const short AggressiveInlining = 256;
    }
    
    [Serializable]
    public struct Vec2 : IEquatable<Vec2>, IFormattable
    {
        public const float REG_TO_DEGREES = 57.29578f;
        // X component of the vector.
        public float x;
        // Y component of the vector.
        public float y;

        // Access the /x/ or /y/ component using [0] or [1] respectively.
        public float this[int index]
        {
            [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }

            [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        // Constructs a new vector with given x, y components.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public Vec2(float x, float y) { this.x = x; this.y = y; }

        // Set x and y components of an existing Vector2.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public void Set(float newX, float newY) { x = newX; y = newY; }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (float) System.Math.Sqrt(x * (double) x + y * (double) y);
        }
        
        // Linearly interpolates between two vectors without clamping the interpolant
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 LerpUnclamped(Vec2 a, Vec2 b, float t)
        {
            return new Vec2(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t
            );
        }

        // Moves a point /current/ towards /target/.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 MoveTowards(Vec2 current, Vec2 target, float maxDistanceDelta)
        {
            // avoid vector ops because current scripting backends are terrible at inlining
            float toVector_x = target.x - current.x;
            float toVector_y = target.y - current.y;

            float sqDist = toVector_x * toVector_x + toVector_y * toVector_y;

            if (sqDist == 0 || (maxDistanceDelta >= 0 && sqDist <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float dist = sqDist;

            return new Vec2(current.x + toVector_x / dist * maxDistanceDelta,
                current.y + toVector_y / dist * maxDistanceDelta);
        }

        // Multiplies two vectors component-wise.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 Scale(Vec2 a, Vec2 b) { return new Vec2(a.x * b.x, a.y * b.y); }

        // Multiplies every component of this vector by the same component of /scale/.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public void Scale(Vec2 scale) { x *= scale.x; y *= scale.y; }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public override string ToString()
        {
            return ToString(null, null);
        }
        
        // Returns a nicely formatted string for this vector.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F2";
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
        }

        // used to allow Vector2s to be used as keys in hash tables
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        // also required for being able to use Vector2s as keys in hash tables
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Vec2)) return false;

            return Equals((Vec2)other);
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public bool Equals(Vec2 other)
        {
            return x == other.x && y == other.y;
        }
        
        /// <summary>
        ///   <para>Makes this vector have a magnitude of 1.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vec2 Normalize()
        {
            float magnitude = this.magnitude;
            if (magnitude > 9.999999747378752E-06)
                return this / magnitude;
            return zero;
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator+(Vec2 a, Vec2 b) { return new Vec2(a.x + b.x, a.y + b.y); }
        // Subtracts one vector from another.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator-(Vec2 a, Vec2 b) { return new Vec2(a.x - b.x, a.y - b.y); }
        // Multiplies one vector by another.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator*(Vec2 a, Vec2 b) { return new Vec2(a.x * b.x, a.y * b.y); }
        // Divides one vector over another.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator/(Vec2 a, Vec2 b) { return new Vec2(a.x / b.x, a.y / b.y); }
        // Negates a vector.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator-(Vec2 a) { return new Vec2(-a.x, -a.y); }
        // Multiplies a vector by a number.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator*(Vec2 a, float d) { return new Vec2(a.x * d, a.y * d); }
        // Multiplies a vector by a number.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator*(float d, Vec2 a) { return new Vec2(a.x * d, a.y * d); }
        // Divides a vector by a number.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Vec2 operator/(Vec2 a, float d) { return new Vec2(a.x / d, a.y / d); }
        // Returns true if the vectors are equal.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool operator==(Vec2 lhs, Vec2 rhs)
        {
            // Returns false in the presence of NaN values.
            float diff_x = lhs.x - rhs.x;
            float diff_y = lhs.y - rhs.y;
            return (diff_x * diff_x + diff_y * diff_y) < kEpsilon * kEpsilon;
        }

        // Returns true if vectors are different.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool operator!=(Vec2 lhs, Vec2 rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

        static readonly Vec2 zeroVector = new Vec2(0F, 0F);
        static readonly Vec2 oneVector = new Vec2(1F, 1F);
        static readonly Vec2 upVector = new Vec2(0F, 1F);
        static readonly Vec2 downVector = new Vec2(0F, -1F);
        static readonly Vec2 leftVector = new Vec2(-1F, 0F);
        static readonly Vec2 rightVector = new Vec2(1F, 0F);
        static readonly Vec2 positiveInfinityVector = new Vec2(float.PositiveInfinity, float.PositiveInfinity);
        static readonly Vec2 negativeInfinityVector = new Vec2(float.NegativeInfinity, float.NegativeInfinity);


        // Shorthand for writing @@Vector2(0, 0)@@
        public static Vec2 zero { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => zeroVector; }
        // Shorthand for writing @@Vector2(1, 1)@@
        public static Vec2 one { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => oneVector; }
        // Shorthand for writing @@Vector2(0, 1)@@
        public static Vec2 up { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => upVector; }
        // Shorthand for writing @@Vector2(0, -1)@@
        public static Vec2 down { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => downVector; }
        // Shorthand for writing @@Vector2(-1, 0)@@
        public static Vec2 left { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => leftVector; }
        // Shorthand for writing @@Vector2(1, 0)@@
        public static Vec2 right { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => rightVector; }
        // Shorthand for writing @@Vector2(float.PositiveInfinity, float.PositiveInfinity)@@
        public static Vec2 positiveInfinity { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => positiveInfinityVector; }
        // Shorthand for writing @@Vector2(float.NegativeInfinity, float.NegativeInfinity)@@
        public static Vec2 negativeInfinity { [MethodImpl(MethodImplOptionsEx.AggressiveInlining)] get => negativeInfinityVector; }

        // *Undocumented*
        public const float kEpsilon = 0.00001F;
        // *Undocumented*
        public const float kEpsilonNormalSqrt = 1e-15f;
    }
}