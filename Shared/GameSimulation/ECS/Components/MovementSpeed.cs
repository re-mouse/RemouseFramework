using Shared.Serialization;

namespace Shared.GameSimulation.ECS.Components
{
    public struct MovementSpeed : IBaseComponent
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