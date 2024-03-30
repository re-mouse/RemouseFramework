using System.Net;
using LiteNetLib;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LiteNetLib.Utils;
using Remouse.Serialization;
using Remouse.Utils;

namespace Remouse.Network.Sockets
{

    public class LiteNetLibClientSocket : IClientSocket
    {
        private const string SecretKey = "SecretKey";

        private EventBasedNetListener _listener;
        private NetManager _netManager;

        private NetPeer? _connectionToServer;
        public bool IsConnecting { get => _connectingTcs != null; }
        public int PingInMilliseconds { get => _connectionToServer?.Ping ?? 0; }
        public bool IsConnected { get => _connectionToServer != null; }
        public event Action Connected;
        public event Action Disconnected;
        public event Action<IBytesReader> DataReceived;

        private TaskCompletionSource<SocketConnectResult> _connectingTcs;

        public LiteNetLibClientSocket()
        {
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);
            
            _listener.PeerConnectedEvent += HandleConnect;
            _listener.PeerDisconnectedEvent += HandleDisconnect;
            _listener.NetworkReceiveEvent += HandleReceive;
            _listener.NetworkErrorEvent += HandleNetworkError;
            
            #if DEBUG
            _netManager.DisconnectTimeout = 2000000;
#endif
        }

        private void HandleDisconnect(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            LLogger.Current.LogInfo(this, $"Disconnected [Reason:{disconnectInfo.Reason}] [SocketErrorCode:{disconnectInfo.SocketErrorCode}]");

            _connectionToServer = null;

            if (IsConnecting)
            {
                if (disconnectInfo.Reason == DisconnectReason.Timeout)
                    TrySetConnectResult(SocketConnectResult.Timeout);
                else if (disconnectInfo.Reason == DisconnectReason.ConnectionRejected)
                    TrySetConnectResult(SocketConnectResult.Rejected);
                else
                    TrySetConnectResult(SocketConnectResult.Error);
            }
            
            Disconnected?.Invoke();
        }

        private void HandleReceive(NetPeer peer, NetPacketReader reader, byte channel, LiteNetLib.DeliveryMethod deliveryMethod)
        {
            LLogger.Current.LogTrace(this, $"Received data  [ByteSize:{reader.AvailableBytes}]");

            DataReceived?.Invoke(new BytesReader(reader.GetRemainingBytes()));
        }

        private void HandleConnect(NetPeer peer)
        {
            LLogger.Current.LogInfo(this, "Connected");
            
            _connectionToServer = peer;
            
            if (IsConnecting)
            {
                TrySetConnectResult(SocketConnectResult.Successful);
            }
            
            Connected?.Invoke();
        }

        private void HandleNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            LLogger.Current.LogError(this, $"Got socket error {socketError}");
            Disconnected?.Invoke();
        }
        
        public async Task<SocketConnectResult> ConnectAsync(IPEndPoint host, string authorizationString,
            CancellationToken cancellationToken = default)
        {
            LLogger.Current.LogInfo(this, $"Start connecting [ServerIp:{host}]");

            _connectingTcs = new TaskCompletionSource<SocketConnectResult>(cancellationToken);
            
            cancellationToken.Register(() => _connectingTcs?.TrySetResult(SocketConnectResult.Cancelled));
            
            if (!_netManager.IsRunning)
            {
                _netManager.Start();
            }
            
            if (authorizationString != null)
            {
                LLogger.Current.LogInfo(this, $"Connecting with connection string [AuthorizationString:{authorizationString}]");
                _netManager.Connect(host, authorizationString);
            }
            else
            {
                LLogger.Current.LogInfo(this, $"Connecting without connection data");
                _netManager.Connect(host, SecretKey);
            }

            await _connectingTcs.Task;

            if (cancellationToken.IsCancellationRequested)
            {
                LLogger.Current.LogInfo(this, $"Connecting cancelled");
                return SocketConnectResult.Cancelled;
            }

            if (_connectingTcs.Task.IsCompletedSuccessfully)
            {
                LLogger.Current.LogInfo(this, $"Connecting completed");
                return _connectingTcs.Task.Result;
            }
            
            return SocketConnectResult.Error;
        }
        
        private void TrySetConnectResult(SocketConnectResult result)
        {
            _connectingTcs?.TrySetResult(result);
        }
        
        public void Disconnect()
        {
            LLogger.Current.LogInfo(this, $"Disconnect called");

            _netManager.DisconnectAll();
        }

        public void Send(IBytesWriter writer, DeliveryMethod deliveryMethod)
        {
            var bytes = writer.GetBytes().ToArray();
            LLogger.Current.LogTrace(this, $"Sending data [ByteSize:{bytes.Length}]");
            _connectionToServer?.Send(bytes, (LiteNetLib.DeliveryMethod)deliveryMethod);
        }
        
        public void PollEvents()
        {
            _netManager.PollEvents();
        }
    }
}