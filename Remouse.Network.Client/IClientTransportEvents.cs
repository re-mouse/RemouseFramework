using System;
using Remouse.Network.Models;

namespace Remouse.Network.Client
{
    public interface IClientTransportEvents
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action<NetworkMessage> MessageReceived;
    }
}