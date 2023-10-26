using Remouse.Core.World;

namespace Remouse.Models
{
    public class WorldStatePacker
    {
        public static WorldState Pack(IReadOnlyWorld world)
        {
            var entities = world.GetEntities();
            WorldState worldState = new WorldState();

            foreach (var entityId in entities)
            {
                worldState.entityInfos.Add(GetEntityInfo(world, entityId));
            }

            return worldState;
        }

        private static EntityInfo GetEntityInfo(IReadOnlyWorld world, int entityId)
        {
            var entityInfo = new EntityInfo();
            entityInfo.entityId = entityId;
            
            foreach (var componentType in world.GetComponentTypes())
            {
                var component = world.GetComponent(componentType, entityId) as INetworkComponent;
                if (component == null)
                    continue;
                
                entityInfo.componentsContainers.Add(new SerializableComponentContainer(component));
            }
            
            return entityInfo;
        }
    }
}