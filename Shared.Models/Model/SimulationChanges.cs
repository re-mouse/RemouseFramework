using System.Collections.Generic;
using Shared.Online.Commands;
using Shared.Serialization;

namespace Shared.Online.Models
{
    public class SimulationChanges : INetworkSerializable
    {
        public List<CreatedEntity> createdEntities = new List<CreatedEntity>();
        public List<int> deletedEntities = new List<int>();
        
        public List<EntityComponentUpdate> updatedComponents = new List<EntityComponentUpdate>();
        public List<EntityComponentDeleted> deletedComponents = new List<EntityComponentDeleted>();

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteList(createdEntities);
            writer.WriteList(deletedEntities);
            writer.WriteList(updatedComponents);
            writer.WriteList(deletedComponents);
        }

        public void Deserialize(INetworkReader reader)
        {
            reader.ReadList(ref createdEntities);
            reader.ReadList(ref deletedEntities);
            reader.ReadList(ref updatedComponents);
            reader.ReadList(ref deletedComponents);
        }
    }
    
    public struct SerializableComponentContainer : INetworkSerializable
    {
        public ushort componentTypeId;
        public IBaseComponent component;

        public SerializableComponentContainer(IBaseComponent component)
        {
            this.component = component;
            this.componentTypeId = ComponentTypeSerializator.GetComponentId(component);
        }
        
        public void Serialize(INetworkWriter writer)
        {
            writer.WriteUShort(componentTypeId);
            component.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            componentTypeId = reader.ReadUShort();

            var emptyComponent = ComponentTypeSerializator.GetEmptyComponent(componentTypeId);
            emptyComponent.Deserialize(reader);

            component = emptyComponent;
        }
    }

    public struct EntityComponentUpdate : INetworkSerializable
    {
        public int entityId;
        public SerializableComponentContainer componentContainer;
        
        public EntityComponentUpdate(int entityId, IBaseComponent component)
        {
            this.entityId = entityId;
            this.componentContainer = new SerializableComponentContainer(component);
        }

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteInt(entityId);
            componentContainer.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            entityId = reader.ReadInt();
            componentContainer.Deserialize(reader);
        }
    }

    public struct EntityComponentDeleted : INetworkSerializable
    {
        public int entityId;
        public ushort componentTypeId;
        
        public EntityComponentDeleted(int entityId, ushort componentTypeId)
        {
            this.entityId = entityId;
            this.componentTypeId = componentTypeId;
        }

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteUShort(componentTypeId);
        }

        public void Deserialize(INetworkReader reader)
        {
            entityId = reader.ReadInt();
            componentTypeId = reader.ReadUShort();
        }
    }
    
    public struct CreatedEntity : INetworkSerializable
    {
        public int entityId;
        public string entityName;
        public List<SerializableComponentContainer> componentsContainers;

        public CreatedEntity(int entityId, string entityName)
        {
            this.entityId = entityId;
            this.entityName = entityName;
            componentsContainers = new List<SerializableComponentContainer>();
        }

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteString(entityName);
            writer.WriteList(componentsContainers);
        }

        public void Deserialize(INetworkReader reader)
        {
            entityId = reader.ReadInt();
            entityName = reader.ReadString();
            
            reader.ReadList(ref componentsContainers);
        }
    }
}