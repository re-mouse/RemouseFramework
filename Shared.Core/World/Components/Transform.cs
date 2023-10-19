using Remouse.Shared.Math;
using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
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