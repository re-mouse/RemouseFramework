using System;
using System.Collections.Generic;
using Remouse.GameServer.Players;
using Remouse.Shared.Models.Messages;

namespace Remouse.GameServer.ServerTransport
{
    public class ServerEventsBus : IServerEvents, IPlayersDataHandler, IPlayersConnectionHandler
    {
        private readonly Dictionary<Type, Delegate> _subscriptions = new Dictionary<Type, Delegate>();

        public event Action<IPlayerConnection> Connected;
        public event Action<IPlayerConnection> Disconnected;
        public event Action<IPlayerConnection, NetworkMessage> MessageReceived;

        void IServerEvents.SubscribeToMessage<T>(Action<IPlayerConnection, T> handler)
        {
            if (_subscriptions.ContainsKey(typeof(T)))
            {
                throw new ArgumentException("Multiple subscriptions to a given message are not allowed");
            }

            _subscriptions.Add(typeof(T), handler);
        }
        
        void IPlayersDataHandler.HandleMessage(PlayerConnection player, NetworkMessage message)
        {
            var messageType = message.GetType();
            
            if (_subscriptions.TryGetValue(messageType, out var subscription))
            {
                subscription.DynamicInvoke(player, message);
            }
            
            MessageReceived?.Invoke(player, message);
        }
        
        void IPlayersConnectionHandler.HandleConnected(PlayerConnection player)
        {
            Connected?.Invoke(player);
        }

        void IPlayersConnectionHandler.HandleDisconnected(PlayerConnection player)
        {
            Disconnected?.Invoke(player);
        }
    }
}