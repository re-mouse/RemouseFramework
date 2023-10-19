using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.World;
using Remouse.Shared.Math;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.Input
{
    public class SetMoveDirectionCommand : WorldCommand
    {
        public int entityId;
        public Vec2 movementDirection;
        
        public override void Apply(IWorld ecsWorld)
        {
            ref var directionComponent = ref ecsWorld.GetComponentRef<MovementDirection>(entityId);
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