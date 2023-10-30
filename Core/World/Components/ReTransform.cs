using Remouse.MathLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.Components
{
    public struct ReTransform : INetworkComponent
    {
        public Vec2 position;
        public float rotationAngle;
        public void Serialize(INetworkWriter writer)
        {
            position.Serialize(writer);
        }

        public void Deserialize(INetworkReader reader)
        {
            position.Deserialize(reader);
        }
    }
}