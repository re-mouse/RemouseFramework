using System;
using Remouse.Models.Messages;

namespace Remouse.GameServer.ServerTransport
{
    public interface IPlayerEvents
    {
        event Action<IPlayer> Connected;
        event Action<IPlayer> Disconnected;
        event Action<IPlayer, NetworkMessage> MessageReceived;
        public void SubscribeToMessage<T>(Action<IPlayer, T> handler) where T : NetworkMessage;
    }
}