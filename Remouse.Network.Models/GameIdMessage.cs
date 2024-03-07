using Remouse.Serialization;

namespace Remouse.Network.Models
{
    public class GameIdMessage : NetworkMessage
    {
        public int gameId;

        public override void Serialize(IBytesWriter writer)
        {
            writer.WriteInt(gameId);
        }

        public override void Deserialize(IBytesReader reader)
        {
            gameId = reader.ReadInt();
        }
    }
}