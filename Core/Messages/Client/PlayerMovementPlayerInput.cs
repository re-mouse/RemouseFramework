using Remouse.MathLib;
using Remouse.Models;
using Remouse.Serialization;

namespace Remouse.Core.Input
{
    public class PlayerMovementPlayerInput : PlayerInputMessage
    {
        public Vec2 movementVector;
        
        public override void Deserialize(INetworkReader reader)
        {
            movementVector.Deserialize(reader);
        }

        public override void Serialize(INetworkWriter writer)
        {
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