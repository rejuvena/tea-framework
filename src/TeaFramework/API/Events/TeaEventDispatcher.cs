using System.Linq;
using Terraria.ModLoader;

namespace TeaFramework.API.Events
{
    public static class TeaEventDispatcher
    {
        public static void DispatchEvent<TEvent>(TEvent @event) where TEvent : TeaEvent
        {
            foreach (ITeaMod teaMod in ModLoader.Mods.OfType<ITeaMod>()) 
                teaMod.EventBus.Post<TEvent>(@event);
        }
    }
}
