using System;

namespace Remouse.Network.Server
{
    public interface IServerMessageBus
    {
        public void SubscribeToMessage<T>(Action<IPlayer, T> handler);
    }
}