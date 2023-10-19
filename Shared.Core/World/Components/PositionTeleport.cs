using Remouse.Shared.Math;
using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
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