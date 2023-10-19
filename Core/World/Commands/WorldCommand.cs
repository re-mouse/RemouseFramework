using Remouse.Core.World;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core
{
    public abstract class WorldCommand : INetworkSerializable, ISerializableType
    {
        public abstract void Apply(IWorld world);
        public abstract void Serialize(INetworkWriter writer);
        public abstract void Deserialize(INetworkReader reader);
    }

    public class EmptyCommand : WorldCommand
    {
        public override void Apply(IWorld ecsWorld)
        {
        }

        public override void Serialize(INetworkWriter writer)
        {
            
        }

        public override void Deserialize(INetworkReader reader)
        {
            
        }
    }
}