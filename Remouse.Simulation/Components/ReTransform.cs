using Remouse.Math;
using Models;
using Remouse.Serialization;

namespace Remouse.Simulation
{
    public struct ReTransform : IBytesSerializableComponent
    {
        public Vec2 position;
        public float rotation;
        
        public void Serialize(IBytesWriter writer)
        {
            position.Serialize(writer);
        }

        public void Deserialize(IBytesReader reader)
        {
            position.Deserialize(reader);
        }
    }
}