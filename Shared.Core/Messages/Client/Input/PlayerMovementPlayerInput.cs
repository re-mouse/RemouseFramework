using Remouse.Shared.Math;
using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.Input
{
    public class PlayerMovementPlayerInput : PlayerInputMessage
    {
        public Vec2 movementVector;
        
        public override void Deserialize(INetworkReader reader)
        {
            ((NetworkMessage)this).Deserialize(reader);
            movementVector.Deserialize(reader);
        }

        public override void Serialize(INetworkWriter writer)
        {
            ((NetworkMessage)this).Serialize(writer);
            movementVector.Serialize(writer);
        }

        public override WorldCommand AsCommand(PlayerData playerData)
        {
            if (playerData?.sessionData?.playerEntityId == null)
                return new EmptyCommand();
            
            return new SetMoveDirectionCommand
            {
                entityId = playerData.sessionData.playerEntityId.Value, 
                movementDirection = movementVector
            };
        }
    }
}