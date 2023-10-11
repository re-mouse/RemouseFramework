using System;
using System.Collections.Generic;
using GameServer.Players;
using GameServer.ServerTransport;
using Shared.Math;
using Shared.DIContainer;
using Shared.GameSimulation;
using Shared.Utils;

namespace GameServer.InterestManagment
{
    public class GridBasedObservingStrategy : IEntityObservingStrategy
    {
        private const int VisibleRange = 30;
        
        private static readonly long RebuildInterval = TimeSpan.FromSeconds(1).Ticks;
        private static readonly int GridResolution = VisibleRange / 2;
        
        private readonly Grid2D<IPlayerConnection> _grid = new Grid2D<IPlayerConnection>(128);
        
        private GameSimulation _gameSimulation;
        private PlayerManager _manager;
        
        private long _lastRebuildTime;
        
        public void Construct(Container container)
        {
            _gameSimulation = container.Get<GameSimulation>();
            _manager = container.Get<PlayerManager>();
        }

        bool IEntityObservingStrategy.IsObserver(int entityId, IPlayerConnection observer)
        {
            throw new NotImplementedException();
            
            // if (!observer.data.sessionData.playerEntityId != 0)
            //     return false;
            //
            Vec2Int projected = ProjectToGrid(_gameSimulation.GetEntityPosition(entityId));

            int playerEntityId = observer.data.sessionData.playerEntityId.Value;
            
            Vec2Int observerProjected = ProjectToGrid(_gameSimulation.GetEntityPosition(playerEntityId));

            return (projected - observerProjected).sqrMagnitude <= 2;
        }

        void IEntityObservingStrategy.GetEntityObservers(int entityId, HashSet<IPlayerConnection> observersResult)
        {
            Vec2 entityPosition = _gameSimulation.GetEntityPosition(entityId);
            Vec2Int current = ProjectToGrid(entityPosition);
            
            _grid.GetWithNeighbours(current, observersResult);
        }

        void IEntityObservingStrategy.Update()
        {
            _grid.ClearNonAlloc();
            
            foreach (IPlayerConnection player in _manager.players)
            {
                // authenticated and joined world with a player?
                if (true) 
                    // if (player.IsSpawnedInWorld())
                {
                    Vec2Int position = ProjectToGrid(player.data.GetPosition());
                    
                    _grid.Add(position, player);
                }
            }
        }
        
        private Vec2Int ProjectToGrid(Vec2 position) => Vec2Int.RoundToInt(position / GridResolution);
    }
}