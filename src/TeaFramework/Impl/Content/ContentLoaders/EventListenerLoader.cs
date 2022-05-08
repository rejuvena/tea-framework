using System;
using TeaFramework.API.ContentLoading;
using TeaFramework.API.Events;
using Terraria.ModLoader;

namespace TeaFramework.Impl.Content.ContentLoaders
{
    public class EventListenerLoader : IContentLoader
    {
        public bool AppliesTo(ILoadable loadable) => loadable is IEventListener;

        public bool OverrideIsLoadingEnabled(IContentLoader.LoadContext context, bool isLoadingEnabled) =>
            isLoadingEnabled;

        public void LoadLoadable(IContentLoader.LoadContext context, Action<ILoadable, Mod> loadLoadable)
        {
            loadLoadable(context.Loadable, context.Mod);
            
            if (context.Mod is ITeaMod teaMod)
                teaMod.EventBus.Subscribe((IEventListener) context.Loadable);
        }

        public void AddContent(IContentLoader.LoadContext context, Action<ILoadable> addContent) =>
            addContent(context.Loadable);

        public void RegisterInstance(IContentLoader.LoadContext context, Action<ILoadable> registerInstance) =>
            registerInstance(context.Loadable);
    }
}
