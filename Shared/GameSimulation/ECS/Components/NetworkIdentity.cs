using Shared.Serialization;

namespace Shared.GameSimulation.ECS.Components
{
    public struct NetworkIdentity : IBaseComponent
    {
        public NetworkVisibility visibility;

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteByte((byte)visibility);
        }

        public void Deserialize(INetworkReader reader)
        {
            visibility = (NetworkVisibility)reader.ReadByte();
        }
    }

    public enum NetworkVisibility : byte
    {
        Default,
        ForceShown,
        Hidden
    }
}