using System;
using Remouse.Network.Models;

namespace Remouse.Network.Server
{
    public interface IServerTransportEvents
    {
        event Action<IPlayer> Connected;
        event Action<IPlayer> Disconnected;
        event Action<IPlayer, NetworkMessage> MessageReceived;
    }
}