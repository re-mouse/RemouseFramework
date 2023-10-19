using Remouse.Models;
using Remouse.Models.Messages;
using Remouse.Transport;

namespace Remouse.GameServer.ServerTransport
{
    public interface IPlayer
    {
        public PlayerData Data { get; }
        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered);
    }
}