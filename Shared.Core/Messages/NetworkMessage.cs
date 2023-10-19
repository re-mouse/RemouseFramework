using Remouse.Shared.Serialization;

namespace Remouse.Shared.Models.Messages
{
    public abstract class NetworkMessage : INetworkSerializable, ISerializableType
    {
        public abstract void Serialize(INetworkWriter writer);
        public abstract void Deserialize(INetworkReader reader);
    }
}