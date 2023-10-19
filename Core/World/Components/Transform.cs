using Remouse.MathLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
{
    public struct Transform : INetworkComponent
    {
        public Vec2 position;
        public float angle;
        public void Serialize(INetworkWriter writer)
        {
            position.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            position.Deserialize(reader);
        }
    }
}