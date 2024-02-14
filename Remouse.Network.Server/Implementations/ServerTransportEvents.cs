using System;
using Remouse.Network.Models;

namespace Remouse.Network.Server
{
    internal class ServerTransportEvents : IServerTransportEvents
    {
        public event Action<IPlayer> Connected;
        public event Action<IPlayer> Disconnected;
        public event Action<IPlayer, NetworkMessage> MessageReceived;
        
        internal void RaiseMessage(Player player, NetworkMessage message)
        {
            MessageReceived?.Invoke(player, message);
        }
        
        internal void RaiseConnected(Player player)
        {
            Connected?.Invoke(player);
        }

        internal void RaiseDisconnected(Player player)
        {
            Disconnected?.Invoke(player);
        }
    }
}