using Remouse.World;
using Remouse.Serialization;

namespace Remouse.Simulation
{
    public abstract class BaseWorldCommand : IBytesSerializable
    {
        public abstract void Run(IWorld world);
        public abstract void Serialize(IBytesWriter writer);
        public abstract void Deserialize(IBytesReader reader);
    }
}