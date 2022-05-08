namespace TeaFramework.API.Events
{
    /// <summary>
    ///     Describes an object capable of dispatching events to listeners.
    /// </summary>
    public interface IEventBus
    {
        void Subscribe(IEventListener listener);

        void Unsubscribe(IEventListener listener);
        
        void Post<TEvent>(TEvent @event) where TEvent : TeaEvent;
    }
}
