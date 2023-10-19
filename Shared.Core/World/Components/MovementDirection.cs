using Remouse.Shared.Math;
using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
{
    public struct MovementDirection : INetworkComponent
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