using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
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