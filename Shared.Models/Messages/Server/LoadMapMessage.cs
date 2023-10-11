using Shared.Serialization;

namespace Shared.Online.Commands
{
    public class LoadMapMessage : NetworkMessage
    {
        public string simulationName { get; private set; }

        public LoadMapMessage(string simulationName)
        {
            this.simulationName = simulationName;
        }

        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteString(simulationName);
        }

        public override void Deserialize(INetworkReader reader)
        {
            simulationName = reader.ReadString();
        }
    }
}