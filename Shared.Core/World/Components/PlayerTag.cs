using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
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