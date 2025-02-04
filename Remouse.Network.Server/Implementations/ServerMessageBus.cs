using System;
using System.Collections.Generic;
using ReDI;
using Remouse.Network.Models;

namespace Remouse.Network.Server
{
    internal class ServerMessageBus : IServerMessageBus
    {
        private readonly Dictionary<Type, Delegate> _subscriptions = new Dictionary<Type, Delegate>();

        [Inject]
        private void SubscribeOnMessages(IServerTransportEvents events)
        {
            events.MessageReceived += HandleMessage;
        }

        public void SubscribeToMessage<T>(Action<IPlayer, T> handler)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                throw new ArgumentException("Multiple subscriptions to a given message are not allowed");
            }

            _subscriptions.Add(typeof(T), handler);
        }
        
        internal void HandleMessage(IPlayer player, NetworkMessage message)
        {
            var messageType = message.GetType();
            
            if (_subscriptions.TryGetValue(messageType, out var subscription))
            {
                subscription.DynamicInvoke(player, message);
            }
        }
    }
}