using Remouse.World;
using Models;
using Remouse.Serialization;

namespace Remouse.Simulation
{
    public struct Render : IBytesSerializableComponent
    {
        public bool preload;
        public int poolCapacity;

        public void Serialize(IBytesWriter writer)
        {
            writer.WriteBool(preload);
            writer.WriteInt(poolCapacity);
        }

        public void Deserialize(IBytesReader reader)
        {
            preload = reader.ReadBool();
            poolCapacity = reader.ReadInt();
        }
    }
}