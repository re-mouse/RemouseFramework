using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
{
    public struct PlayerTag : INetworkComponent
    {
        public long playerId;
        public void Serialize(INetworkWriter writer)
        {
            
        }

        public void Deserialize(INetworkReader reader)
        {
            
        }
    }
}