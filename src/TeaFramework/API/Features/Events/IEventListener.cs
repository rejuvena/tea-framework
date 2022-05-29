using System;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.Events
{
    /// <summary>
    ///     Describes an object with listens to events.
    /// </summary>
    /// <typeparam name="TEvent">The event type to listen for.</typeparam>
    public interface IEventListener<in TEvent> : IEventListener
        where TEvent : TeaEvent
    {
        Type IEventListener.Type => typeof(TEvent);

        void IEventListener.HandleEvent(object @event) {
            HandleEvent((TEvent) @event);
        }

        /// <summary>
        ///     Handles all posted event objects.
        /// </summary>
        /// <param name="event"></param>
        void HandleEvent(TEvent @event);
    }

    /// <summary>
    ///     Describes an object with listens to events.
    /// </summary>
    public interface IEventListener : ILoadable
    {
        /// <summary>
        ///     The base type to listen to.
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///     Handles all posted event objects.
        /// </summary>
        /// <param name="event"></param>
        void HandleEvent(object @event);
    }
}