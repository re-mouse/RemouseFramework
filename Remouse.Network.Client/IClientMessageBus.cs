using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.Network.Models;

namespace Remouse.Network.Client
{
    public interface IClientMessageBus
    {
        public void SubscribeToMessage<T>(Action<T> handler) where T : NetworkMessage;
        public UniTask<T> AwaitMessageAsync<T>(CancellationToken cancellationToken) where T : NetworkMessage;
    }
}