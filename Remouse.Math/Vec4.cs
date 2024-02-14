using System;
using System.Globalization;

namespace Remouse.Math
{
    [Serializable]
    public struct Vec4
    {
        public static Vec4 Identity = new Vec4(0, 0, 0, 1);

        public float x, y, z, w;

        public Vec3 eulerAngles { get => this.ToEulerAngles(); }

        public float squaredLength { get => x * x + y * y + z * z + w * w; }

        public float length { get => (float)System.Math.Sqrt(x * x + y * y + z * z + w * w); }

        public Vec4 normalized
        {
            get
            {
                var ilen = 1f / length;

                return new Vec4(x * ilen, y * ilen, z * ilen, w * ilen);
            }
        }

        public Vec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vec3 AsQuaternionOfThree()
        {
            return new Vec3(x / w, y / w, z / w);
        }

        public static Vec4 FromQuaternionOfThree(Vec3 q)
        {
            return new Vec4(q.x, q.y, q.z, 1);
        }

        public static Vec3 ToEulerAngles(Vec4 rotation)
        {
            rotation = rotation.normalized;

            float sqw = rotation.w * rotation.w;
            float sqx = rotation.x * rotation.x;
            float sqy = rotation.y * rotation.y;
            float sqz = rotation.z * rotation.z;
            float unit = sqx + sqy + sqz + sqw;
            float test = rotation.x * rotation.w - rotation.y * rotation.z;
            Vec3 v;

            if (test > 0.4995f * unit)
            {
                v.y = 2f * (float) System.Math.Atan2(rotation.y, rotation.x);
                v.x = (float) System.Math.PI / 2;
                v.z = 0;
                return MathUtility.NormalizeAngles(v * (180f / (float) System.Math.PI));
            }

            if (test < -0.4995f * unit)
            {
                v.y = -2f * (float) System.Math.Atan2(rotation.y, rotation.x);
                v.x = -(float) System.Math.PI / 2;
                v.z = 0;
                return MathUtility.NormalizeAngles(v * (180f / (float) System.Math.PI));
            }

            Vec4 q = new Vec4(rotation.w, rotation.z, rotation.x, rotation.y);
            v.y = (float) System.Math.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));
            v.x = (float) System.Math.Asin(2f * (q.x * q.z - q.w * q.y));
            v.z = (float) System.Math.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));
            return MathUtility.NormalizeAngles(v * (180f / (float) System.Math.PI));
        }

        public static Vec4 FromEulerAngles(Vec3 v)
        {
            return FromEulerAngles(v.x, v.y, v.z);
        }

        public static Vec4 FromEulerAngles(float x, float y, float z)
        {
            return ToQuaternion(y, x, z);
        }

        public static Vec4 ToQuaternion(float yaw, float pitch, float roll)
        {
            yaw *= MathUtility.Deg2Rad;
            pitch *= MathUtility.Deg2Rad;
            roll *= MathUtility.Deg2Rad;

            float rollOver2 = roll * 0.5f;
            float sinRollOver2 = (float)System.Math.Sin(rollOver2);
            float cosRollOver2 = (float)System.Math.Cos(rollOver2);
            float pitchOver2 = pitch * 0.5f;
            float sinPitchOver2 = (float)System.Math.Sin(pitchOver2);
            float cosPitchOver2 = (float)System.Math.Cos(pitchOver2);
            float yawOver2 = yaw * 0.5f;
            float sinYawOver2 = (float)System.Math.Sin(yawOver2);
            float cosYawOver2 = (float)System.Math.Cos(yawOver2);

            var w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            var x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            var y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            var z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return new Vec4(x, y, z, w);
        }

        public override string ToString()
        {
            var format = CultureInfo.InvariantCulture.NumberFormat;
            return
                $"({x.ToString("F1", format)}, " +
                $"{y.ToString("F1", format)}, " +
                $"{z.ToString("F1", format)}, " +
                $"w: {w.ToString("F1", format)})";
        }
    }

    public static class Vec4Extensions
    {
        public static Vec3 ToEulerAngles(this Vec4 q)
        {
            return Vec4.ToEulerAngles(q);
        }

        /// <summary>
        /// Convert from normalized quaternion with scalar of 1.
        /// </summary>
        public static Vec4 ToQuaternion(this Vec3 v3)
        {
            return Vec4.FromQuaternionOfThree(v3);
        }
    }
}