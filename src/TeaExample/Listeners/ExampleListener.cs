using TeaExampleMod.Events;
using TeaFramework.API.Features.Events;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TeaExampleMod.Listeners
{
    public class ExampleListener : IEventListener<VersionDrawEvent>
    {
        void ILoadable.Load(Mod mod) { }

        void ILoadable.Unload() { }

        public void HandleEvent(VersionDrawEvent @event) {
            string text = Language.GetTextValue("Mods.TeaExample.Lang.Key", @event.VersionText);
            text += '\n' + Language.GetTextValue("Mods.TeaExample.Toml.Key");
            @event.VersionText = text;
        }
    }
}