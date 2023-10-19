using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public class LoadMapMessage : NetworkMessage
    {
        public string MapId { get; private set; }

        public LoadMapMessage(string mapId)
        {
            MapId = mapId;
        }

        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteString(MapId);
        }

        public override void Deserialize(INetworkReader reader)
        {
            MapId = reader.ReadString();
        }
    }
}