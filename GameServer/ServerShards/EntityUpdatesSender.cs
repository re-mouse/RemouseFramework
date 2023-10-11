using System.Collections.Generic;
using GameServer.Players;
using GameServer.ServerTransport;
using Shared;
using Shared.DIContainer;
using Shared.GameSimulation;
using Shared.Online.Commands;
using Shared.Online.Models;

namespace GameServer.ServerShards
{
    public class EntityUpdatesSender
    {
        private readonly Dictionary<PlayerData, SimulationChanges> _updatesMap = new Dictionary<PlayerData, SimulationChanges>();

        private readonly PlayerObservingRegistry _observingRegistry;
        private readonly NetworkEntityRegistry _networkEntityRegistry;
        private readonly GameSimulation _simulation;
        
        public EntityUpdatesSender(Container container)
        {
            container.Get(out _observingRegistry);
            container.Get(out _networkEntityRegistry);
            container.Get(out _simulation);
        }
        
        public void SendUpdatesToObservers(SimulationChanges changes)
        {
            _networkEntityRegistry.Update(changes);
            _observingRegistry.Update();
            
            _updatesMap.Clear();

            FillObservingStatusUpdates();
            
            FillDeletedComponents(changes);
            
            FillUpdatedComponents(changes);

            foreach (var updateByPlayer in _updatesMap)
            {
                var player = updateByPlayer.Key;
                var update = updateByPlayer.Value;
                
                player.Send(player, new SimulationUpdateMessage(update), DeliveryMethod.ReliableOrdered);
            }
        }

        private void FillUpdatedComponents(SimulationChanges changes)
        {
            foreach (var updated in changes.updatedComponents)
            {
                var entityId = updated.entityId;

                var entity = _networkEntityRegistry.entities[entityId];

                var observingData = _observingRegistry.observingDatas[entity];

                foreach (var player in observingData.alreadySee)
                {
                    SendUpdatedComponent(player, updated);
                }
            }
        }

        private void FillDeletedComponents(SimulationChanges changeses)
        {
            foreach (var deletedComponentUpdate in changeses.deletedComponents)
            {
                var entityId = deletedComponentUpdate.entityId;

                var entity = _networkEntityRegistry.entities[entityId];

                var observingData = _observingRegistry.observingDatas[entity];

                foreach (var player in observingData.alreadySee)
                {
                    SendDeletedComponent(player, deletedComponentUpdate);
                }
            }
        }

        private void FillObservingStatusUpdates()
        {
            foreach (var entity in _networkEntityRegistry.entities.Values)
            {
                var observingData = _observingRegistry.observingDatas[entity];

                foreach (var player in observingData.startedToSee)
                {
                    SendCreatedEntity(player, entity);
                }

                foreach (var player in observingData.stoppedToSee)
                {
                    SendDestroyedEntityData(player, entity);
                }
            }
        }

        private void SendUpdatedComponent(PlayerData playerData, EntityComponentUpdate updatedComponentData)
        {
            var updates = GetPlayerRelatedUpdates(playerData);
            
            updates.updatedComponents.Add(updatedComponentData);
        }

        private void SendCreatedEntity(PlayerData playerData, NetworkEntity entity)
        {
            var updates = GetPlayerRelatedUpdates(playerData);

            CreatedEntity createdEntity = new CreatedEntity();
            createdEntity.entityId = entity.entityId;
            createdEntity.entityName = entity.name;
            
            foreach (var type in entity.components)
            {
                var pool = _simulation.World.GetPoolByType(type);
                var component = pool.GetRaw(entity.entityId) as IBaseComponent;
                
                if (component == null)
                    continue;

                createdEntity.componentsContainers = new List<SerializableComponentContainer>();
                createdEntity.componentsContainers.Add(new SerializableComponentContainer(component));
            }
            
            updates.createdEntities.Add(createdEntity);
        }

        private void SendDestroyedEntityData(PlayerData playerData, NetworkEntity entity)
        {
            var updates = GetPlayerRelatedUpdates(playerData);
            
            updates.deletedEntities.Add(entity.entityId);
        }

        private void SendDeletedComponent(PlayerData playerData, EntityComponentDeleted entityComponentDeleted)
        {
            var updates = GetPlayerRelatedUpdates(playerData);

            updates.deletedComponents.Add(entityComponentDeleted);
        }

        private SimulationChanges GetPlayerRelatedUpdates(PlayerData playerData)
        {
            if (!_updatesMap.TryGetValue(playerData, out var updates))
            {
                _updatesMap[playerData] = new SimulationChanges();
                return _updatesMap[playerData];
            }

            return updates;
        }
    }
}