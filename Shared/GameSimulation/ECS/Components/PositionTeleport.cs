using Shared.Math;
using Shared.Serialization;

namespace Shared.GameSimulation.ECS.Components
{
    public struct PositionTeleport : IBaseComponent
    {
        public Vec2 DestinationPosition;
        public void Serialize(INetworkWriter writer)
        {
            DestinationPosition.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            DestinationPosition.Deserialize(reader);
        }
    }
}