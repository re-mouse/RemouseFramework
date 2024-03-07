using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ReDI;
using Remouse.Network.Models;
using Remouse.Utils;

namespace Remouse.Network.Client
{
    public class ClientMessageBus : IClientMessageBus
    {
        private readonly CollectionsDictionary<Type, Delegate> _subscriptions = new CollectionsDictionary<Type, Delegate>();

        private readonly CollectionsDictionary<Type, UniTaskCompletionSource<NetworkMessage>> _awaitingMessages = new CollectionsDictionary<Type, UniTaskCompletionSource<NetworkMessage>>();
        
        private void SubscribeOnEvents(IClientTransportEvents events)
        {
            events.MessageReceived += HandleMessage;
        }

        public void SubscribeToMessage<T>(Action<T> handler) where T : NetworkMessage
        {
            LLogger.Current.LogTrace(this, $"Subscribed to message [NetworkMessageType:{typeof(T).Name}");
            _subscriptions.Add(typeof(T), handler);
        }

        public async UniTask<T> AwaitMessageAsync<T>(CancellationToken cancellationToken) where T : NetworkMessage
        {
            LLogger.Current.LogTrace(this, $"Awaiting message [NetworkMessageType:{typeof(T).Name}");

            var tcs = new UniTaskCompletionSource<NetworkMessage>();

            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                _awaitingMessages.Add(typeof(T), tcs);
                var result = await tcs.Task;
                return (T)result;
            }
        }
        
        private void HandleMessage(NetworkMessage message)
        {
            var messageType = message.GetType();
            
            if (_subscriptions.TryGetValue(messageType, out var subscriptions))
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.DynamicInvoke(message);
                }
            }
            
            if (_awaitingMessages.TryGetValue(messageType, out var completionSources))
            {
                foreach (var completionSource in completionSources)
                {
                    completionSource.TrySetResult(message);
                }
                
                _awaitingMessages.Remove(messageType);
            }
        }
    }
}