using System;
using System.Net;
using GameClient.Transport;
using Remouse.Shared.DIContainer;
using Remouse.Shared.Models.Messages;
using Remouse.Shared.Serialization;
using Remouse.Shared.Utils.Log;

namespace GameClient
{
    public class ClientSocketManager : IClientEventsHandler, IClientCommandsSender
    {
        private ClientSocket _socket;
        private INetworkWriter _networkWriter = new NetworkBytesWriter();
        
        private ILogger _logger;

        private bool _isActive;

        public Action disconnected;
        public Action connected;
        public Action<NetworkMessage> onMessage;

        public bool IsConnected { get; private set; }

        public void Construct(Container container)
        {
            container.Get(out _logger);
            container.Get(out _socket);
            _socket.SetListener(this);
        }

        public void Start(IPEndPoint endPoint)
        {
            Logger.Current.LogInfo(this, "Starting client..");

            _isActive = true;

            _socket.Connect(endPoint);
        }

        public void Stop()
        {
            _isActive = false;
        }
        
        void IClientEventsHandler.HandleConnected()
        {
            IsConnected = true;
            
            Logger.Current.LogInfo(this, "Connected to server");
        }

        void IClientEventsHandler.HandleDisconnected()
        {
            IsConnected = false;
            _isActive = false;
            
            disconnected?.Invoke();
            
            Logger.Current.LogInfo(this, "Disconnected from the server");
        }

        void IClientEventsHandler.HandleData(INetworkReader data)
        {
            var message = NetworkMessageSerializer.Deserialize(data);
            
            onMessage?.Invoke(message);
        }
        
        public void Tick()
        {
            _socket.Update();
        }

        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.Sequenced)
        {
            Logger.Current.LogInfo(this, $"Sent message : {message}");
            _networkWriter.Clear();
            
            NetworkMessageSerializer.Serialize(message, _networkWriter);
            
            _socket.Send(_networkWriter.GetBytes(), deliveryMethod);
        }
    }
}