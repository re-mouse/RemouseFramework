using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public class SimulationMapInfoMessage : NetworkMessage
    {
        public string MapId { get; private set; }

        public SimulationMapInfoMessage()
        {
            
        }

        public SimulationMapInfoMessage(string mapId)
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