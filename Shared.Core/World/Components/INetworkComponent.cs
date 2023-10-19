using Remouse.Shared.Core.World;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Models
{
    public interface INetworkComponent : INetworkSerializable, IComponent, ISerializableType
    {
        
    }
}