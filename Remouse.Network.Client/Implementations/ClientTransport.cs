using System;
using System.Net;
using System.Threading;
using Cysharp.Threading.Tasks;
using ReDI;
using Remouse.Network.Models;
using Remouse.Serialization;
using Remouse.Utils;
using Remouse.Network.Sockets;
using NetworkSerializeExtensions = Remouse.Network.Models.NetworkSerializeExtensions;

namespace Remouse.Network.Client
{
    internal class ClientTransport : IClientTransport
    {
        [Inject] private IClientSocket _socket;
        [Inject] private ClientTransportEvents _transportEvents;
        [Inject] private IAuthorizeProvider _authorizeProvider;
        
        private IBytesWriter _bytesWriter = new BytesWriter();

        public bool IsConnected { get; private set; }
        public int PingInMilliseconds { get => _socket.PingInMilliseconds; }

        [Inject]
        private void SubscribeOnEvents()
        {
            _socket.Connected += HandleSocketConnected;
            _socket.Disconnected += HandleSocketDisconnected;
            _socket.DataReceived += HandleSocketData;
        }

        public async UniTask<ClientConnectResult> ConnectAsync(IPEndPoint endPoint,
            CancellationToken cancellationToken)
        {
            LLogger.Current.LogInfo(this, "Starting client..");

            var authenticationCredentials = await _authorizeProvider.ProvideCredentialsAsync(cancellationToken);
            
            var result = await _socket.ConnectAsync(endPoint, authenticationCredentials.AuthorizationString, cancellationToken);

            switch (result)
            {
                case SocketConnectResult.Successful:
                    return ClientConnectResult.Successful;
                case SocketConnectResult.Cancelled:
                    return ClientConnectResult.Cancelled;
                case SocketConnectResult.Rejected:
                    return ClientConnectResult.InvalidCredentials;
                case SocketConnectResult.Timeout:
                    return ClientConnectResult.Timeout;

                default:
                    return ClientConnectResult.UnknownError;
            }
        }

        public void Disconnect()
        {
            _socket.Disconnect();
        }
        
        public void Update()
        {
            _socket.PollEvents();
        }

        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.Sequenced)
        {
            _bytesWriter.Clear();
            
            _bytesWriter.Write(message);
            
            _socket.Send(_bytesWriter, deliveryMethod);
            
            LLogger.Current.LogTrace(this, $"Send message [NetworkMessage:{message}]");
        }
        
        void HandleSocketConnected()
        {
            IsConnected = true;

            LLogger.Current.LogInfo(this, "Connected to server");
            
            _transportEvents.HandleConnected();
        }

        void HandleSocketDisconnected()
        {
            IsConnected = false;
            
            LLogger.Current.LogInfo(this, "Disconnected from the server");
            
            _transportEvents.HandleDisconnected();
        }

        void HandleSocketData(IBytesReader data)
        {
            NetworkMessage message;
            try
            {
                message = NetworkSerializeExtensions.ReadNetworkMessage(data);
                LLogger.Current.LogTrace(this, $"Received message [NetworkMessage:{message}]");
            }
            catch (Exception exception)
            {
                LLogger.Current.LogException(this, exception, $"Got exception on deserialization");
                return;
            }

            if (message == null)
            {
                LLogger.Current.LogWarning(this, $"Received invalid network message");
                return;
            }
            
            LLogger.Current.LogTrace(this, $"Succesfuly deserialized [NetworkMessage:{message}]");
            
            _transportEvents.HandleMessage(message);
        }
    }
}