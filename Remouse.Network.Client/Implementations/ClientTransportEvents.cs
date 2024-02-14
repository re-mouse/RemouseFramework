using System;
using Remouse.Network;
using Remouse.Network.Models;

namespace Remouse.Network.Client
{
    internal class ClientTransportEvents : IClientTransportEvents
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action<NetworkMessage> MessageReceived;
        
        internal void HandleMessage(NetworkMessage message)
        {
            MessageReceived?.Invoke(message);
        }
        
        internal void HandleConnected()
        {
            Connected?.Invoke();
        }

        internal void HandleDisconnected()
        {
            Disconnected?.Invoke();
        }
    }
}