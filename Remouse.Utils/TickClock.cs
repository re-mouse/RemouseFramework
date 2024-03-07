using System;
using System.Diagnostics;

namespace Remouse.Simulation
{
    public class TickClock
    {
        private double _millisecondsPerTick;
        private Stopwatch _stopwatch = new Stopwatch();
        private double _lastTickMilliseconds;
        private int _millisecondsDelay;

        public TickClock(int ticksInSecond)
        {
            _millisecondsPerTick = 1000 / ticksInSecond;
        }

        public void Start()
        {
            _stopwatch.Start();
            _lastTickMilliseconds = _stopwatch.Elapsed.TotalMilliseconds;
        }

        public void SetMillisecondsDelay(int milliseconds)
        {
            _millisecondsDelay = milliseconds;
        }

        public int CalculateTicksElapsed()
        {
            double elapsed = _stopwatch.Elapsed.TotalMilliseconds - _millisecondsDelay - _lastTickMilliseconds;
            if (elapsed < 0)
                return 0;
            int requiredTicks = (int) (elapsed / _millisecondsPerTick);
            _lastTickMilliseconds += _millisecondsPerTick * requiredTicks;

            return requiredTicks;
        }
    }
}