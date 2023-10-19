using Remouse.Core.World;
using Remouse.Serialization;

namespace Remouse.Models
{
    public interface INetworkComponent : INetworkSerializable, IComponent, ISerializableType
    {
        
    }
}