using Shared.Math;
using Shared.Serialization;

namespace Shared.GameSimulation.ECS.Components
{
    public struct MovementDirection : IBaseComponent
    {
        public Vec2 movementDirection;

        public void Serialize(INetworkWriter writer)
        {
            movementDirection.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            movementDirection.Deserialize(reader);
        }
    }
}