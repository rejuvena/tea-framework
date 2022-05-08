using System;
using System.Collections.Generic;
using TeaFramework.API.Events;

namespace TeaFramework.Impl.Events
{
    public class EventBus : IEventBus
    {
        public Dictionary<Type, List<IEventListener>> Listeners { get; } = new();

        public void Subscribe(IEventListener listener) => GetListeners(listener.Type).Add(listener);

        public void Unsubscribe(IEventListener listener) => GetListeners(listener.Type).Remove(listener);

        public void Post<TEvent>(TEvent @event) where TEvent : TeaEvent
        {
            foreach (IEventListener listener in GetListeners(typeof(TEvent)))
                listener.HandleEvent(@event);
        }

        private List<IEventListener> GetListeners(Type type)
        {
            if (!Listeners.ContainsKey(type))
                Listeners[type] = new List<IEventListener>();

            return Listeners[type];
        }
    }
}
