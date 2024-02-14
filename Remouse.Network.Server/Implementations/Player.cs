using Remouse.Network.Models;
using Remouse.Serialization;
using Remouse.Utils;
using Remouse.Network.Sockets;

namespace Remouse.Network.Server
{
    internal class Player : IPlayer
    {
        public PlayerIdentity Identity { get; }
        public long Id { get => Identity.id; }
        public int GameId { get; }
        
        private readonly Connection _connection;

        private IBytesWriter _writer = new BytesWriter();

        internal Player(PlayerIdentity identity, Connection connection, int gameId)
        {
            Identity = identity;
            GameId = gameId;
            _connection = connection;
        }

        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            _writer.Clear();

            _writer.Write(message);
            
            _connection.Send(_writer, deliveryMethod);
            
            LLogger.Current.LogPlayerTrace(this, this, $"Send message [Message:{message}] [DeliveryMethod:{deliveryMethod}]");
        }

        public void Disconnect()
        {
            LLogger.Current.LogPlayerInfo(this, this, $"Requested disconnect");

            _connection.Disconnect();
        }
    }
}