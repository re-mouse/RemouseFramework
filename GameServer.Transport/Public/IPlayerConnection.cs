using Shared.Online.Commands;
using Shared.Online.Models;

namespace GameServer.ServerTransport
{
    public interface IPlayerConnection
    {
        public PlayerData data { get; }
        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered);
    }
}