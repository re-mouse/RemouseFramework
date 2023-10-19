using System;

namespace Remouse.MathLib
{
    public static class MathUtility
    {
        public const float Deg2Rad = 0.0174532924F;
        public const float Rad2Deg = 57.29578F;

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        public static int Max(params int[] values)
        {
            var length = values.Length;

            if (length == 0)
                return 0;

            var num = values[0];

            for (int index = 1; index < length; ++index)
            {
                if (values[index] > num)
                    num = values[index];
            }

            return num;
        }

        public static int RoundToInt(float x)
        {
            return (int)System.Math.Round(x);
        }

        public static int[] SplitEqually(int value, int splitAmount, bool keepRemainder = false)
        {
            if (value == 0 || splitAmount == 0)
            {
                throw new InvalidOperationException(
                    "Provided invalid data - " +
                    $"value: [{value}], " +
                    $"split amount: [{splitAmount}]");
            }

            int[] partitions = new int[keepRemainder ? splitAmount : System.Math.Min(value, splitAmount)];

            if (value < splitAmount)
            {
                if (keepRemainder)
                {
                    for (int i = splitAmount; i-- > value;)
                    {
                        partitions[i] = 0;
                    }
                }

                splitAmount = value;
            }

            if (value % splitAmount == 0)
            {
                for (int i = 0; i < splitAmount; i++)
                {
                    partitions[i] = value / splitAmount;
                }
            }
            else
            {
                int threshold = splitAmount - (value % splitAmount);
                int division = value / splitAmount;
                for (int i = 0; i < splitAmount; i++)
                {
                    if (i >= threshold)
                    {
                        partitions[i] = division + 1;
                    }
                    else
                    {
                        partitions[i] = division;
                    }
                }
            }

            return partitions;
        }

        public static Vec3 NormalizeAngles(Vec3 angles)
        {
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);
            return angles;
        }

        public static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
    }
}