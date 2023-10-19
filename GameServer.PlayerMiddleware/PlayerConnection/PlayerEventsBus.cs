using System;
using System.Collections.Generic;
using Remouse.Models.Messages;

namespace Remouse.GameServer.ServerTransport
{
    public class PlayerEventsBus : IPlayerEvents
    {
        private readonly Dictionary<Type, Delegate> _subscriptions = new Dictionary<Type, Delegate>();

        public event Action<IPlayer> Connected;
        public event Action<IPlayer> Disconnected;
        public event Action<IPlayer, NetworkMessage> MessageReceived;

        void IPlayerEvents.SubscribeToMessage<T>(Action<IPlayer, T> handler)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                throw new ArgumentException("Multiple subscriptions to a given message are not allowed");
            }

            _subscriptions.Add(typeof(T), handler);
        }
        
        internal void RaiseMessage(Player player, NetworkMessage message)
        {
            var messageType = message.GetType();
            
            if (_subscriptions.TryGetValue(messageType, out var subscription))
            {
                subscription.DynamicInvoke(player, message);
            }
            
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