using System.Collections.Generic;
using Remouse.Serialization;

namespace Remouse.Simulation
{
    public class WorldState : IBytesSerializable
    {
        public List<EntityState> entities = new List<EntityState>();

        public void Serialize(IBytesWriter writer)
        {
            writer.WriteList(entities);
        }

        public void Deserialize(IBytesReader reader)
        {
            reader.ReadList(ref entities);
        }
    }
    
    public struct EntityState : IBytesSerializable
    {
        public List<SerializedComponent> components;

        public void Serialize(IBytesWriter writer)
        {
            writer.WriteList(components);
        }

        public void Deserialize(IBytesReader reader)
        {
            reader.ReadList(ref components);
        }
    }
    
    public struct SerializedComponent : IBytesSerializable
    {
        public ushort componentTypeId;
        public IBytesSerializableComponent component;

        public SerializedComponent(IBytesSerializableComponent component)
        {
            this.component = component;
            componentTypeId = TypeSerializer<IBytesSerializableComponent>.GetTypeId(component);
        }
        
        public void Serialize(IBytesWriter writer)
        {
            writer.WriteUShort(componentTypeId);
            component.Serialize(writer);
        }

        public void Deserialize(IBytesReader reader)
        {
            componentTypeId = reader.ReadUShort();

            var emptyComponent = TypeSerializer<IBytesSerializableComponent>.GetNew(componentTypeId);
            emptyComponent.Deserialize(reader);

            component = emptyComponent;
        }
    }
}