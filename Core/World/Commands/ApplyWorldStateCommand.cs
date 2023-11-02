using Remouse.Core;
using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Shared.World.Commands
{
    public class ApplyWorldStateCommand : WorldCommand
    {
        private readonly PackedWorld _packedWorld;

        public ApplyWorldStateCommand(PackedWorld packedWorld)
        {
            _packedWorld = packedWorld;
        }

        public override void Run(IWorld world, Database database)
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