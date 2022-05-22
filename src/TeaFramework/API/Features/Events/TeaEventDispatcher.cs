using System.Linq;
using TeaFramework.Utilities.Extensions;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.Events
{
    public static class TeaEventDispatcher
    {
        public static void DispatchEvent<TEvent>(TEvent @event) where TEvent : TeaEvent
        {
            foreach (ITeaMod teaMod in ModLoader.Mods.OfType<ITeaMod>())
            {
                IEventBus? bus = teaMod.GetService<IEventBus>();
                bus?.Post(@event);
            }
        }
    }
}
