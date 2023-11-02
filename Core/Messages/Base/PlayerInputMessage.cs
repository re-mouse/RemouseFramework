using Remouse.Models;
using Remouse.Models.Messages;
using Remouse.Serialization;

namespace Remouse.Core.Input
{
    public abstract partial class PlayerInputMessage : NetworkMessage
    {
        public abstract WorldCommand AsCommand(PlayerData playerData);
    }
}