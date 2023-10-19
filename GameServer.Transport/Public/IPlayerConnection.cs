using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;

namespace Remouse.GameServer.ServerTransport
{
    public interface IPlayerConnection
    {
        public PlayerData Data { get; }
        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered);
    }
}