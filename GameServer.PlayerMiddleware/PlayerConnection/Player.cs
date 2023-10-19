using Remouse.Models;
using Remouse.Models.Messages;
using Remouse.Serialization;
using Remouse.Shared.Utils.Log;
using Remouse.Transport;

namespace Remouse.GameServer.ServerTransport
{
    internal class Player : IPlayer
    {
        public PlayerData Data { get; }
        
        private readonly Connection _connection;

        private INetworkWriter _writer = new NetworkBytesWriter();

        internal Player(PlayerData playerData, Connection connection)
        {
            Data = playerData;
            _connection = connection;
        }

        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
        {
            _writer.Clear();

            NetworkMessageSerializer.Serialize(message, _writer);
            
            Logger.Current.LogInfo(this, $"Sending message to playerId {Data?.cloudData?.id}, message = {message}, deliveryMethod = {deliveryMethod}");
            
            _connection.Send(_writer.GetBytes().ToArray(), deliveryMethod);
        }
    }
}