using System;
using System.Collections.Generic;

namespace TeaFramework.API.Features.Events
{
    /// <summary>
    ///     Describes an object capable of dispatching events to listeners.
    /// </summary>
    public interface IEventBus
    {
        Dictionary<Type, List<IEventListener>> Listeners { get; }
        
        void Subscribe(IEventListener listener);

        void Unsubscribe(IEventListener listener);
        
        void Post<TEvent>(TEvent @event) where TEvent : TeaEvent;
    }
}
