using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;
using Remouse.Shared.Serialization;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer.ServerTransport
{
    internal class PlayerConnection : IPlayerConnection
    {
        public PlayerData Data { get; }
        
        private readonly Connection _connection;

        private INetworkWriter _writer = new NetworkBytesWriter();

        internal PlayerConnection(PlayerData playerData, Connection connection)
        {
            Data = playerData;
            _connection = connection;
        }

        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            _writer.Clear();

            NetworkMessageSerializer.Serialize(message, _writer);
            
            Logger.Current.LogInfo(this, $"Sending message to playerId {Data?.cloudData?.id}, message = {message}, deliveryMethod = {deliveryMethod}");
            
            _connection.Send(_writer, deliveryMethod);
        }
    }
}