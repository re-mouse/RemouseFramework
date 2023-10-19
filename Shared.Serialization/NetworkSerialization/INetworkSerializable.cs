namespace Remouse.Shared.Serialization
{
    public interface INetworkSerializable
    {
        public void Serialize(INetworkWriter writer);
        public void Deserialize(INetworkReader reader);
    }
}