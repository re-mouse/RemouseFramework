using Remouse.Shared.Core.World;
using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core
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