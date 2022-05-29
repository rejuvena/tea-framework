using System;
using System.Collections.Generic;

namespace TeaFramework.API.Features.Events
{
    /// <summary>
    ///     Describes an object capable of dispatching events to listeners.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        ///     The dictionary of event types pointing to all subscribed listeners.
        /// </summary>
        Dictionary<Type, List<IEventListener>> Listeners { get; }

        /// <summary>
        ///     Subscribes a listener to this event bus, handles event type subscription.
        /// </summary>
        /// <param name="listener"></param>
        void Subscribe(IEventListener listener);

        /// <summary>
        ///     Unsubscribes a listener from this event bus.
        /// </summary>
        /// <param name="listener"></param>
        void Unsubscribe(IEventListener listener);

        /// <summary>
        ///     Posts an event so that all event listeners may receive the event.
        /// </summary>
        /// <param name="event">The event instance.</param>
        /// <typeparam name="TEvent">The event type that event listeners should handle.</typeparam>
        void Post<TEvent>(TEvent @event)
            where TEvent : TeaEvent;
    }
}
