using System;

namespace Remouse.Math
{
    public class DeterminedRandom
    {
        private const double RealUnit = 1.0 / (uint.MaxValue + 1.0);
        
        public uint x;
        public uint y;
        public uint z;
        public uint  w;

        public DeterminedRandom(uint x = 42, uint y = 842502087, uint z = 3579807591, uint w = 273326509)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public int Next(int lowerBound, int upperBound)
        {
            if (lowerBound > upperBound)
                throw new ArgumentOutOfRangeException(nameof(upperBound), upperBound, "upperBound must be >= lowerBound");

            uint t = GenerateNextUint();
            int range = upperBound - lowerBound;

            if (range < 0)
            {
                long longRange = (long)upperBound - lowerBound;
                return lowerBound + (int)(RealUnit * t * longRange);
            }
            else
            {
                return lowerBound + (int)(RealUnit * (int)t * range);
            }
        }

        public double NextDouble()
        {
            return RealUnit * GenerateNextUint();
        }

        private uint GenerateNextUint()
        {
            uint t = x ^ (x << 11);
            x = y;
            y = z;
            z = w;
            w = w ^ (w >> 19) ^ t ^ (t >> 8);
            return w;
        }
    }
}