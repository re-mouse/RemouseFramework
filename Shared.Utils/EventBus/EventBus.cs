using System;
using System.Collections.Generic;

namespace Remouse.Shared.Utils.Events
{
    public class EventBus<T> where T : IEvent
    {
        private readonly List<Action<T>> _subscribers = new List<Action<T>>();

        public void Raise(T @event) 
        {
            foreach(var subscriber in _subscribers)
            {
                subscriber.Invoke(@event);
            }
        }

        public void Subscribe(Action<T> handler)
        {
            _subscribers.Add(handler);
        }
    }
}