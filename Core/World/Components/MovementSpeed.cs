using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.ECS.Components
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