using System.Collections.Generic;

namespace Remouse.Simulation.Network
{
    public class NetworkEntityRegistry
    {
        public int LastUsedNetworkId { get => _lastUsedId; }
        
        private Dictionary<int, int> _entities = new Dictionary<int, int>();
        private int _lastUsedId;

        public void Clear()
        {
            _lastUsedId = 0;
            _entities.Clear();
        }

        public bool IsEntityExist(int networkId)
        {
            return _entities.ContainsKey(networkId);
        }
        
        public void SetLastUsedNetworkId(int networkId)
        {
            _lastUsedId = networkId;
        }

        public void SetEntity(int entityId, int networkId)
        {
            _entities[networkId] = entityId;
        }

        public int GetEntity(int networkId)
        {
            if (_entities.ContainsKey(networkId))
            {
                return _entities[networkId];
            }

            return 0;
        }
        
        public int AssignNetworkId(int entityId)
        {
            _entities[_lastUsedId] = entityId;

            return _lastUsedId++;
        }
    }
}