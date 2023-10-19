using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
{
    public struct NetworkIdentity : INetworkComponent
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