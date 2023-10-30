using Remouse.Core.World;
using Remouse.Serialization;

namespace Remouse.Core
{
    public abstract class WorldCommand : INetworkSerializable, ISerializableType
    {
        public abstract void Run(IWorld world);
        public abstract void Serialize(INetworkWriter writer);
        public abstract void Deserialize(INetworkReader reader);
    }

    public class EmptyCommand : WorldCommand
    {
        public override void Run(IWorld ecsWorld)
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