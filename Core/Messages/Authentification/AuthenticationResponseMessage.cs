using Remouse.Serialization;

namespace Remouse.Models.Messages.Authentification
{
    public class AuthenticationResponseMessage : NetworkMessage
    {
        public Result result;

        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteByte((byte)result);
        }

        public override void Deserialize(INetworkReader reader)
        {
            result = (Result)reader.ReadByte();
        }
    }

    public enum Result : byte
    {
        Successful,
        Failed,
    }
}