using System;
using Remouse.Shared.Models.Messages;

namespace Remouse.GameServer.ServerTransport
{
    public interface IServerEvents
    {
        event Action<IPlayerConnection> Connected;
        event Action<IPlayerConnection> Disconnected;
        event Action<IPlayerConnection, NetworkMessage> MessageReceived;
        public void SubscribeToMessage<T>(Action<IPlayerConnection, T> handler) where T : NetworkMessage;
    }
}