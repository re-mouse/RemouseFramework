using Remouse.Serialization;
using Remouse.World;

namespace Remouse.Simulation.Network
{
    public abstract class BasePlayerInputCommand : BaseWorldCommand
    {
        public int playerGameId;

        public abstract bool Validate(IReadOnlyWorld world);

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