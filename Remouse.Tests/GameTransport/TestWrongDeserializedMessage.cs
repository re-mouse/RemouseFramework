using Remouse.Network.Models;
using Remouse.Serialization;

namespace Remouse.Network.Sockets.Tests
{
    public class TestWrongDeserializedMessage : NetworkMessage
    {
        public string text;
        public override void Serialize(IBytesWriter writer)
        {
            writer.WriteString(text);
        }

        public override void Deserialize(IBytesReader reader)
        {
            text = reader.ReadString();
            reader.ReadLong();
        }
    }
}