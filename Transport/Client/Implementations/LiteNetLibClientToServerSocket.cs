using System.Net;
using LiteNetLib;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using LiteNetLib.Utils;
using Remouse.Serialization;

namespace Remouse.Transport
{

    public class LiteNetLibClientToServerSocket : IClientToServerSocket
    {
        private EventBasedNetListener _listener;
        private NetManager _netManager;

        private NetPeer? _connectionToServer;
        public bool IsConnecting { get => _currentConnectionTask != null && !_currentConnectionTask.Task.IsCompleted; }
        
        public bool IsConnected { get => _connectionToServer != null; }
        public event Action Connected;
        public event Action Disconnected;
        public event Action<INetworkReader> DataReceived;

        private TaskCompletionSource<SocketConnectResult> _currentConnectionTask;

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
            if (IsConnecting)
            {
                if (disconnectInfo.Reason == DisconnectReason.Timeout)
                    TrySetConnectResult(SocketConnectResult.Timeout);
                else if (disconnectInfo.Reason == DisconnectReason.ConnectionRejected)
                    TrySetConnectResult(SocketConnectResult.Rejected);
                else
                    TrySetConnectResult(SocketConnectResult.Error);
            }
            
            _connectionToServer = null;
            
            Disconnected?.Invoke();
        }

        private void HandleReceive(NetPeer peer, NetPacketReader reader, byte channel, LiteNetLib.DeliveryMethod deliveryMethod)
        {
            DataReceived?.Invoke(new LiteNetLibReader(reader));
        }

        private void HandleConnect(NetPeer peer)
        {
            if (IsConnecting)
            {
                TrySetConnectResult(SocketConnectResult.Successful);
            }
            _connectionToServer = peer;
            
            Connected?.Invoke();
        }

        private void HandleNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Disconnected?.Invoke();
        }
        
        public async Task<SocketConnectResult> ConnectAsync(IPEndPoint host, INetworkWriter connectData, int timeOutMilliseconds, CancellationToken cancellationToken)
        {
            if (_netManager.IsRunning)
                throw new InvalidOperationException("NetManager is already running.");

            if (connectData == null)
                throw new NullReferenceException();

            _netManager.MaxConnectAttempts = 5;
            _netManager.DisconnectTimeout = timeOutMilliseconds / _netManager.MaxConnectAttempts;
            
            _netManager.Start();

            _currentConnectionTask = new TaskCompletionSource<SocketConnectResult>(cancellationToken);

            cancellationToken.Register(() => _currentConnectionTask?.TrySetResult(SocketConnectResult.Cancelled));

            _netManager.Connect(host, NetDataWriter.FromBytes(connectData.GetBytes().ToArray(), true));

            await _currentConnectionTask.Task;

            if (cancellationToken.IsCancellationRequested)
                return SocketConnectResult.Cancelled;

            if (_currentConnectionTask.Task.IsCompletedSuccessfully)
                return _currentConnectionTask.Task.Result;
            
            return SocketConnectResult.Error;
        }

        private void TrySetConnectResult(SocketConnectResult result)
        {
            _currentConnectionTask?.TrySetResult(result);
            _currentConnectionTask = null;
        }
        
        public void Disconnect()
        {
            _netManager.DisconnectAll();
        }

        public void Send(INetworkWriter writer, DeliveryMethod deliveryMethod)
        {
            _connectionToServer?.Send(writer.GetBytes().ToArray(), (LiteNetLib.DeliveryMethod)deliveryMethod);
        }
        
        public void Update()
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