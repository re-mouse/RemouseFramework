using Shared.Math;
using Shared.Serialization;

namespace Shared.GameSimulation.ECS.Components
{
    public struct Transform : IBaseComponent
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