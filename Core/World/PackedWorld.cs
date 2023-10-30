using System.Collections.Generic;
using Remouse.Core.World;
using Remouse.Serialization;

namespace Remouse.Models
{
    public class PackedWorld : INetworkSerializable
    {
        public List<EntityInfo> entityInfos = new List<EntityInfo>();

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteList(entityInfos);
        }

        public void Deserialize(INetworkReader reader)
        {
            reader.ReadList(ref entityInfos);
        }
        
        public static PackedWorld Pack(IReadOnlyWorld world)
        {
            var entities = world.GetEntities();
            PackedWorld packedWorld = new PackedWorld();

            foreach (var entityId in entities)
            {
                packedWorld.entityInfos.Add(GetEntityInfo(world, entityId));
            }

            return packedWorld;
        }

        public static void Unpack(PackedWorld packedWorld, IWorld world)
        {
            foreach (var entityInfo in packedWorld.entityInfos)
            {
                int entity = world.NewEntity();
                foreach (var componentContainer in entityInfo.componentsContainers)
                {
                    world.SetComponent(componentContainer.component.GetType(), entity, componentContainer.component);
                }
            }
        }

        private static EntityInfo GetEntityInfo(IReadOnlyWorld world, int entityId)
        {
            var entityInfo = new EntityInfo();
            entityInfo.componentsContainers = new List<SerializableComponentContainer>();
            
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
    
    public struct EntityInfo : INetworkSerializable
    {
        public List<SerializableComponentContainer> componentsContainers;

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteList(componentsContainers);
        }

        public void Deserialize(INetworkReader reader)
        {
            reader.ReadList(ref componentsContainers);
        }
    }
    
    public struct SerializableComponentContainer : INetworkSerializable
    {
        public ushort componentTypeId;
        public INetworkComponent component;

        public SerializableComponentContainer(INetworkComponent component)
        {
            this.component = component;
            componentTypeId = TypeSerializer<INetworkComponent>.GetTypeId(component);
        }
        
        public void Serialize(INetworkWriter writer)
        {
            writer.WriteUShort(componentTypeId);
            component.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            componentTypeId = reader.ReadUShort();

            var emptyComponent = TypeSerializer<INetworkComponent>.GetNew(componentTypeId);
            emptyComponent.Deserialize(reader);

            component = emptyComponent;
        }
    }
}