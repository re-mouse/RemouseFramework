using System.Collections.Generic;
using Remouse.MathLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
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