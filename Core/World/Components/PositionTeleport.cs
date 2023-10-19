using Remouse.MathLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
{
    public struct PositionTeleport : INetworkComponent
    {
        public Vec2 destinationPosition;
        public void Serialize(INetworkWriter writer)
        {
            destinationPosition.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            destinationPosition.Deserialize(reader);
        }
    }
}