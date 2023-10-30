using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public class MapInfoMessage : NetworkMessage
    {
        public string MapId { get; private set; }

        public MapInfoMessage(string mapId)
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