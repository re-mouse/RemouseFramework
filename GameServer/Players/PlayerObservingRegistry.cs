using System.Collections.Generic;
using GameServer.InterestManagment;
using GameServer.ServerShards;
using GameServer.ServerTransport;
using Shared.DIContainer;

namespace GameServer.Players
{
    public class PlayerObservingRegistry
    {
        private IEntityObservingStrategy _observingStrategy;
        private NetworkEntityRegistry _networkEntityRegistry;
        
        private Dictionary<NetworkEntity, NetworkObservingData> _datas =
            new Dictionary<NetworkEntity, NetworkObservingData>();

        public IReadOnlyDictionary<NetworkEntity, NetworkObservingData> observingDatas => _datas;

        public void Construct(Container container)
        {
            _observingStrategy = container.Get<IEntityObservingStrategy>();
            _networkEntityRegistry = container.Get<NetworkEntityRegistry>();
        }
        
        public void Update()
        {
            _observingStrategy.Update();
            
            foreach (var networkEntity in _networkEntityRegistry.entities.Values)
            {
                if (!_datas.TryGetValue(networkEntity, out var data))
                {
                    data = new NetworkObservingData();
                    _datas[networkEntity] = data;
                }

                data.rawOldObservers = data.rawObservers;
                data.rawObservers = new HashSet<Player>();
                
                _observingStrategy.GetEntityObservers(networkEntity.entityId, data.rawObservers);
                
                CompareObservers(data);
            }
        }

        private void CompareObservers(NetworkObservingData data)
        {
            data.startedToSee.Clear();
            data.alreadySee.Clear();
            data.stoppedToSee.Clear();
            
            foreach (var observer in data.rawObservers)
            {
                if (!data.rawOldObservers.Contains(observer))
                {
                    data.startedToSee.Add(observer);
                }
                else
                {
                    data.alreadySee.Add(observer);
                }
            }

            foreach (var oldObserver in data.rawOldObservers)
            {
                if (!data.rawObservers.Contains(oldObserver))
                {
                    data.stoppedToSee.Add(oldObserver);
                }
            }
        }
    }

    public class NetworkObservingData
    {
        public HashSet<IPlayerConnection> startedToSee = new HashSet<IPlayerConnection>();
        public HashSet<IPlayerConnection> stoppedToSee = new HashSet<IPlayerConnection>();
        public HashSet<IPlayerConnection> alreadySee = new HashSet<IPlayerConnection>();

        internal HashSet<IPlayerConnection> rawObservers = new HashSet<IPlayerConnection>();
        internal HashSet<IPlayerConnection> rawOldObservers = new HashSet<IPlayerConnection>();
    }
}