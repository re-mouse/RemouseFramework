using System.Collections.Generic;
using Remouse.Shared.Math;
using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
{
    public struct WaypointPath : INetworkComponent
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