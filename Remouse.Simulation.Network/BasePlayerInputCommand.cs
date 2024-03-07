using ReDI;
using Remouse.Serialization;
using Remouse.World;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation.Network
{
    public abstract class BasePlayerInputCommand : BaseWorldCommand
    {
        public int playerGameId;

        public abstract bool Validate(EcsWorld world);

        public override void Serialize(IBytesWriter writer)
        {
            writer.WriteInt(playerGameId);
        }

        public override void Deserialize(IBytesReader reader)
        {
            playerGameId = reader.ReadInt();
        }
    }
}