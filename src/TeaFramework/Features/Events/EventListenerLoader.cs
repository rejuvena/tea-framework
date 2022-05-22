using System;
using TeaFramework.API;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.Utilities.Extensions;
using Terraria.ModLoader;

namespace TeaFramework.Features.Events
{
    public class EventListenerLoader : IContentLoader
    {
        public bool AppliesTo(ILoadable loadable) => loadable is IEventListener;

        public bool OverrideIsLoadingEnabled(IContentLoader.LoadContext context, bool isLoadingEnabled) =>
            isLoadingEnabled;

        public void LoadLoadable(IContentLoader.LoadContext context, Action<ILoadable, Mod> loadLoadable)
        {
            loadLoadable(context.Loadable, context.Mod);

            if (context.Mod is not ITeaMod teaMod)
                return;

            IEventBus? bus = teaMod.GetService<IEventBus>();
            bus?.Subscribe((IEventListener) context.Loadable);
        }

        public void AddContent(IContentLoader.LoadContext context, Action<ILoadable> addContent) =>
            addContent(context.Loadable);

        public void RegisterInstance(IContentLoader.LoadContext context, Action<ILoadable> registerInstance) =>
            registerInstance(context.Loadable);
    }
}
