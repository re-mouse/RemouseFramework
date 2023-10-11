using System.Collections.Generic;
using GameServer.ServerTransport;

namespace GameServer.InterestManagment
{
    public interface IEntityObservingStrategy
    {
        public bool IsObserver(int entityId, IPlayerConnection observer);
        public void GetEntityObservers(int entityId, HashSet<IPlayerConnection> observersResult);
        public void Update();
    }
}