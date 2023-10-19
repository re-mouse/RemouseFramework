using Remouse.Shared.Core;
using Remouse.Shared.Core.World;
using Remouse.Shared.Models;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.World.Commands
{
    public class ApplyWorldStateCommand : WorldCommand
    {
        private readonly WorldState _worldState;

        public ApplyWorldStateCommand(WorldState worldState)
        {
            _worldState = worldState;
        }

        public override void Apply(IWorld world)
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