using System;
using System.Collections.Generic;
using Remouse.GameServer.ServerShards;
using Remouse.Shared.Core;
using Remouse.Shared.DIContainer;
using Remouse.Shared.Utils.Buffer;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer
{
    public class WorldCommandBuffer
    {
        private const int InputBufferCapacity = 30;
        private const int InputExpireTicksTime = 70;

        private RoundBuffer<HashSet<WorldCommand>> _roundBuffer = new RoundBuffer<HashSet<WorldCommand>>(InputBufferCapacity);
        
        private SimulationHost _simulationHost;
        private long _currentBufferTick;
        
        private long SimulationTick { get { return _simulationHost?.Simulation?.CurrentTick ?? 0; } }

        private void Construct(Container container)
        {
            container.Get(out _simulationHost);
            
            _currentBufferTick = _simulationHost.Simulation.CurrentTick;
        }

        //input accepting only on next tick
        public void Enqueue(WorldCommand command, long tick)
        {
            long tickDifference = (int)(tick - _currentBufferTick);
            
            if (tickDifference < -InputExpireTicksTime)
            {
                Logger.Current.LogTrace(this, $"Received tick is earlier than current. Received - {tick}, current - {SimulationTick}");
                return;
            }

            if (tickDifference > InputBufferCapacity)
            {
                Logger.Current.LogTrace(this, $"exceeds buffer's future capacity. Received - {tick}, current - {SimulationTick}");
                return;
            }

            int bufferIndex = tickDifference < 0 ? 0 : (int)tickDifference;
            
            var inputs = _roundBuffer.GetAt(bufferIndex);
            inputs.Add(command);
        }
        
        public void Enqueue(WorldCommand command)
        {
            var inputs = _roundBuffer.GetCurrent();
            inputs.Add(command);
        }
        
        public HashSet<WorldCommand> GetCurrentCommands()
        {
            return _roundBuffer.GetCurrent();
        }

        public void SyncTickWithSimulation()
        {
            long inputsToClear = SimulationTick - _currentBufferTick;

            if (inputsToClear == 0) return;

            if (inputsToClear < 0)
            {
                Logger.Current.LogException(this, new Exception($"Undefined behaviour, simulation went back in time. Difference in ticks {inputsToClear}"));
                return;
            }

            if (inputsToClear > InputBufferCapacity)
                inputsToClear = InputBufferCapacity;

            _currentBufferTick = SimulationTick;
            for (int i = 0; i < inputsToClear; i++)
            {
                _roundBuffer.GetCurrent().Clear();
                _roundBuffer.MoveToNext();
            }
        }
    }
}