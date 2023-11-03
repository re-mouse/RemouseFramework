using System.Net;
using LiteNetLib;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using LiteNetLib.Utils;
using Remouse.Serialization;
using Remouse.Shared.Utils.Log;

namespace Remouse.Transport
{

    public class LiteNetLibClientToServerSocket : IClientToServerSocket
    {
        private const string SecretKey = "SecretKey";

        private EventBasedNetListener _listener;
        private NetManager _netManager;

        private NetPeer? _connectionToServer;
        public bool IsConnecting { get => _connectingTcs != null; }
        
        public bool IsConnected { get => _connectionToServer != null; }
        public event Action Connected;
        public event Action Disconnected;
        public event Action<INetworkReader> DataReceived;

        private TaskCompletionSource<SocketConnectResult> _connectingTcs;

        public LiteNetLibClientToServerSocket()
        {
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);
            
            _listener.PeerConnectedEvent += HandleConnect;
            _listener.PeerDisconnectedEvent += HandleDisconnect;
            _listener.NetworkReceiveEvent += HandleReceive;
            _listener.NetworkErrorEvent += HandleNetworkError;
        }

        private void HandleDisconnect(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Logger.Current.LogTrace(this, $"Disconnected - {disconnectInfo.Reason}, socket - {disconnectInfo.SocketErrorCode}");

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
            Logger.Current.LogTrace(this, $"Received data of size {reader.AvailableBytes}");

            DataReceived?.Invoke(new LiteNetLibReader(reader));
        }

        private void HandleConnect(NetPeer peer)
        {
            Logger.Current.LogTrace(this, "Connected");
            
            _connectionToServer = peer;
            
            if (IsConnecting)
            {
                TrySetConnectResult(SocketConnectResult.Successful);
            }
            
            Connected?.Invoke();
        }

        private void HandleNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Logger.Current.LogError(this, $"Got socket error {socketError}");
            Disconnected?.Invoke();
        }
        
        public async Task<SocketConnectResult> ConnectAsync(IPEndPoint host, INetworkWriter connectData, int timeOutMilliseconds, CancellationToken cancellationToken)
        {
            if (!_netManager.IsRunning)
            {
                _netManager.Start();
            }

            _netManager.MaxConnectAttempts = 3;
            _netManager.DisconnectTimeout = timeOutMilliseconds / _netManager.MaxConnectAttempts;

            _connectingTcs = new TaskCompletionSource<SocketConnectResult>(cancellationToken);
            
            cancellationToken.Register(() => _connectingTcs?.TrySetResult(SocketConnectResult.Cancelled));

            if (connectData != null)
            {
                _netManager.Connect(host, NetDataWriter.FromBytes(connectData.GetBytes().ToArray(), true));
            }
            else
            {
                _netManager.Connect(host, SecretKey);
            }

            await _connectingTcs.Task;

            if (cancellationToken.IsCancellationRequested)
                return SocketConnectResult.Cancelled;

            if (_connectingTcs.Task.IsCompletedSuccessfully)
                return _connectingTcs.Task.Result;
            
            return SocketConnectResult.Error;
        }
        
        private void TrySetConnectResult(SocketConnectResult result)
        {
            _connectingTcs?.TrySetResult(result);
        }
        
        public void Disconnect()
        {
            Logger.Current.LogTrace(this, $"Disconnect called");

            _netManager.DisconnectAll();
        }

        public void Send(INetworkWriter writer, DeliveryMethod deliveryMethod)
        {
            var bytes = writer.GetBytes().ToArray();
            Logger.Current.LogTrace(this, $"Sending data of size {bytes.Length}");
            _connectionToServer?.Send(bytes, (LiteNetLib.DeliveryMethod)deliveryMethod);
        }
        
        public void PollEvents()
        {
            _netManager.PollEvents();
        }

        public void Dispose()
        {
            Disconnected = null;
            Connected = null;
            DataReceived = null;
            
            TrySetConnectResult(SocketConnectResult.Cancelled);
            
            _netManager.Stop();
        }
    }
}