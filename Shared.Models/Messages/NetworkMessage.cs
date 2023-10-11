using Shared.Serialization;

namespace Shared.Online.Commands
{
    public abstract class NetworkMessage : INetworkSerializable
    {
        public abstract void Serialize(INetworkWriter writer);
        public abstract void Deserialize(INetworkReader reader);
    }
}