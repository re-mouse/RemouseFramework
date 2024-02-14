using Remouse.Network.Models;
using Remouse.Serialization;

namespace Remouse.Network.Sockets.Tests
{
    public class TestWrongSerializedMessage : NetworkMessage
    {
        public string text;
        public override void Serialize(IBytesWriter writer)
        {
            writer.WriteString(text);
            writer.WriteUShort(10);
        }

        public override void Deserialize(IBytesReader reader)
        {
            text = reader.ReadString();
        }
    }
}