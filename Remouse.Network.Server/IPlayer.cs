using Remouse.Network.Models;
using Remouse.Network.Sockets;

namespace Remouse.Network.Server
{
    public interface IPlayer
    {
        public long Id { get => Identity?.id ?? 0; }
        public int GameId { get; }
        public PlayerIdentity Identity { get; }
        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered);
        public void Disconnect();
    }
}