using System.Collections.Generic;
using Shared.Online.Commands;

namespace GameClient
{
    class GameSimulationUpdatesBuffer
    {
        private readonly Dictionary<long, SimulationUpdateMessage> _entityUpdateMessages = new Dictionary<long, SimulationUpdateMessage>();
        
        public void Enqueue(SimulationUpdateMessage simulationUpdateMessage)
        {
            //implement new
            // _entityUpdateMessages[simulationUpdateMessage.simulationTick] = simulationUpdateMessage;
        }
        
        public SimulationUpdateMessage? GetAt(long tick)
        {
            _entityUpdateMessages.TryGetValue(tick, out var value);

            return value;
        }

        public void ClearUntil(long tick)
        {
            var keysToRemove = new List<long>();
            foreach (var key in _entityUpdateMessages.Keys)
            {
                if (key < tick)
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _entityUpdateMessages.Remove(key);
            }
        }

        public void Clear()
        {
            _entityUpdateMessages.Clear();
        }
    }
}