using System.Collections.Generic;
using Shared.Math;
using Shared.Serialization;

namespace Shared.GameSimulation.ECS.Components
{
    public struct WaypointPath : IBaseComponent
    {
        public List<Vec2> waypoints;

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteList(waypoints);
        }

        public void Deserialize(INetworkReader reader)
        {
            reader.ReadList(ref waypoints);
        }
    }
}