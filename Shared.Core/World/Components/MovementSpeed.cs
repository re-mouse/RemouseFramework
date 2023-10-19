using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.ECS.Components
{
    public struct MovementSpeed : INetworkComponent
    {
        public float speed;

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteFloat(speed);
        }

        public void Deserialize(INetworkReader reader)
        {
            speed = reader.ReadFloat();
        }
    }
}