using System;
using System.Globalization;

namespace Shared.Math
{
    [Serializable]
    public struct Vec3 : IEquatable<Vec3>
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static readonly Vec3 Zero = new Vec3(0, 0, 0);
        public static readonly Vec3 One = new Vec3(1, 1, 1);
        public static readonly Vec3 Right = new Vec3(1, 0, 0);
        public static readonly Vec3 Up = new Vec3(0, 1, 0);

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3 operator -(Vec3 a)
        {
            return new Vec3(-a.x, -a.y, -a.z);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vec3 operator *(Vec4 rotation, Vec3 point)
        {
            float num1 = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num1;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num1;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vec3 vector3;
            vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x +
                                ((double)num7 - (double)num12) * (double)point.y +
                                ((double)num8 + (double)num11) * (double)point.z);
            vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x +
                                (1.0 - ((double)num4 + (double)num6)) * (double)point.y +
                                ((double)num9 - (double)num10) * (double)point.z);
            vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x +
                                ((double)num9 + (double)num10) * (double)point.y +
                                (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
            return vector3;
        }

        public static Vec3 operator *(Vec3 a, float d)
        {
            return new Vec3(a.x * d, a.y * d, a.z * d);
        }

        public static Vec3 operator *(float d, Vec3 a)
        {
            return new Vec3(a.x * d, a.y * d, a.z * d);
        }

        public static Vec3 operator /(Vec3 a, float d)
        {
            return new Vec3(a.x / d, a.y / d, a.z / d);
        }

        public float SquaredLength() => x * x + y * y + z * z;

        public float Length() => (float)System.Math.Sqrt(x * x + y * y + z * z);

        public bool Equals(Vec3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vec3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }

        public Vec3 Normalized()
        {
            float totalLength = x + y + z;
            return new Vec3(x / totalLength, y / totalLength, z / totalLength);
        }

        public static bool operator ==(Vec3 a, Vec3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vec3 a, Vec3 b)
        {
            return !(a == b);
        }

        public static Fix64 Distance(Vec3 a, Vec3 b)
        {
            return (Fix64)System.Math.Sqrt(
                System.Math.Pow(a.x - b.x, 2) +
                System.Math.Pow(a.y - b.y, 2) +
                System.Math.Pow(a.z - b.z, 2));
        }

        public static float SignedAngle(in Vec3 from, in Vec3 to, in Vec3 axis)
        {
            var angle = Angle(from, to);
            var cross = Cross(from, to);
            var dot = Dot(cross, axis);

            return angle * (dot >= 0f ? 1f : -1f);
        }

        public static float Angle(in Vec3 from, in Vec3 to)
        {
            float num = (float)System.Math.Sqrt(from.SquaredLength() * to.SquaredLength());
            return num < 1E-15
                ? 0.0f
                : (float)System.Math.Acos(MathUtility.Clamp(Dot(from, to) / num, -1f, 1f)) * MathUtility.Rad2Deg;
        }

        public static Vec3 Cross(in Vec3 a, in Vec3 b)
        {
            return new Vec3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x);
        }

        public static float Dot(in Vec3 a, in Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        private string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F1";
            return
                $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
        }
    }
}