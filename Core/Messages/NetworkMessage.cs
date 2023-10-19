using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public abstract class NetworkMessage : INetworkSerializable, ISerializableType
    {
        public abstract void Serialize(INetworkWriter writer);
        public abstract void Deserialize(INetworkReader reader);
    }
}