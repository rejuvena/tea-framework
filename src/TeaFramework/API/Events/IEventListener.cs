using System;
using Terraria.ModLoader;

namespace TeaFramework.API.Events
{
    /// <summary>
    ///     Describes an object with listens to events.
    /// </summary>
    public interface IEventListener<in TEvent> : IEventListener where TEvent : TeaEvent
    {
        void HandleEvent(TEvent @event);

        Type IEventListener.Type => typeof(TEvent);
        
        void IEventListener.HandleEvent(object @event) => HandleEvent((TEvent) @event);
    }
    
    /// <summary>
    ///     Describes an object with listens to events.
    /// </summary>
    public interface IEventListener : ILoadable
    {
        Type Type { get; }
        
        void HandleEvent(object @event);
    }
}
