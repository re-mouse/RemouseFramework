using System;
using System.Collections.Generic;
using Remouse.Core;
using Remouse.DIContainer;
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
            _simulationHost = container.Resolve<SimulationHost>();
            
            _currentBufferTick = _simulationHost.Simulation.CurrentTick;
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