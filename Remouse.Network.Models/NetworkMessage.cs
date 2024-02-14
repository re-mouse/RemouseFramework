using Remouse.Serialization;

namespace Remouse.Network.Models
{
    public abstract class NetworkMessage : IBytesSerializable
    {
        public abstract void Serialize(IBytesWriter writer);
        public abstract void Deserialize(IBytesReader reader);
    }
}