using System.Collections.Generic;
using Remouse.Simulation;
using ReDI;

namespace Remouse.GameClient
{
    public class ReceivedWorldCommandsBuffer
    {
        private Dictionary<long, HashSet<BaseWorldCommand>> _worldCommands = new Dictionary<long, HashSet<BaseWorldCommand>>();
        private readonly HashSet<BaseWorldCommand> _emptyCommands = new HashSet<BaseWorldCommand>();

        [Inject]
        private void SubscribeToEvents(ISimulationHost host)
        {
            host.SimulationChanged += HandleSimulationChanged;
        }

        private void HandleSimulationChanged(ISimulation simulation)
        {
            _worldCommands.Clear();
        }

        public void Enqueue(long tick, BaseWorldCommand command)
        {
            if (!_worldCommands.ContainsKey(tick))
                _worldCommands[tick] = new HashSet<BaseWorldCommand>();

            _worldCommands[tick].Add(command);
        }

        public HashSet<BaseWorldCommand> GetCommands(long tick)
        {
            return _worldCommands.ContainsKey(tick) ? _worldCommands[tick] : _emptyCommands;
        }
    }
}