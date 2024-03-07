using ReDI;
using Remouse.World;
using Remouse.Serialization;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public abstract class BaseWorldCommand : IBytesSerializable
    {
        public abstract void Run(EcsWorld world);
        public abstract void Serialize(IBytesWriter writer);
        public abstract void Deserialize(IBytesReader reader);
    }
}