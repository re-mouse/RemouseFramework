using System.Collections.Generic;
using Remouse.Serialization;

namespace Remouse.Models
{
    public class WorldState : INetworkSerializable
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
    }
    
    public struct EntityInfo : INetworkSerializable
    {
        public int entityId;
        public string entityTypeId;
        public List<SerializableComponentContainer> componentsContainers;

        public EntityInfo(int entityId, string entityTypeId)
        {
            this.entityId = entityId;
            this.entityTypeId = entityTypeId;
            componentsContainers = new List<SerializableComponentContainer>();
        }

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteString(entityTypeId);
            
            writer.WriteList(componentsContainers);
        }

        public void Deserialize(INetworkReader reader)
        {
            entityId = reader.ReadInt();
            entityTypeId = reader.ReadString();
            
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