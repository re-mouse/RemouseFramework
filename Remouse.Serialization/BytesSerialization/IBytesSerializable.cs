namespace Remouse.Serialization
{
    public interface IBytesSerializable
    {
        public void Serialize(IBytesWriter writer);
        public void Deserialize(IBytesReader reader);
    }
}