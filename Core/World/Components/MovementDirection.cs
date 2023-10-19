using Remouse.MathLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
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