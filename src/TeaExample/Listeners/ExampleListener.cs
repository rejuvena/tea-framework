using TeaExampleMod.Events;
using TeaFramework.API.Features.Events;
using Terraria.ModLoader;

namespace TeaExampleMod.Listeners
{
    public class ExampleListener : IEventListener<VersionDrawEvent>
    {
        void ILoadable.Load(Mod mod)
        {
        }

        void ILoadable.Unload()
        {
        }

        public void HandleEvent(VersionDrawEvent @event) => @event.VersionText += ": Hello from Tea Example Mod!";
    }
}
