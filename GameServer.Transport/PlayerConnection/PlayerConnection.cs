using Shared.Online.Commands;
using Shared.Online.Models;
using Shared.Serialization;
using Shared.Utils.Log;

namespace GameServer.ServerTransport
{
    internal class PlayerConnection : IPlayerConnection
    {
        public PlayerData data { get; }
        
        private readonly Connection _connection;

        private INetworkWriter _writer = new NetworkBytesWriter();

        internal PlayerConnection(PlayerData playerData, Connection connection)
        {
            this.data = playerData;
            _connection = connection;
        }

        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            _writer.Clear();

            NetworkMessageSerializer.Serialize(message, _writer);
            
            Logger.Current.LogInfo(this, $"Sending message to playerId {data?.saveData?.id}, message = {message}, deliveryMethod = {deliveryMethod}");
            
            _connection.Send(_writer, deliveryMethod);
        }
    }
}