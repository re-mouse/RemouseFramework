using Remouse.Core.Components;
using Remouse.Core.World;
using Remouse.MathLib;
using Remouse.Serialization;

namespace Remouse.Core.Input
{
    public class SetMoveDirectionCommand : WorldCommand
    {
        public int entityId;
        public Vec2 movementDirection;
        
        public override void Apply(IWorld ecsWorld)
        {
            ref var directionComponent = ref ecsWorld.GetComponentRef<MoveInDirection>(entityId);
            directionComponent.movementDirection = movementDirection;
        }

        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteInt(entityId);
            movementDirection.Serialize(writer);
        }

        public override void Deserialize(INetworkReader reader)
        {
            entityId = reader.ReadInt();
            movementDirection.Deserialize(reader);
        }
    }
}