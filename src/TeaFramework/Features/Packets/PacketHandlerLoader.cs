using System;
using TeaFramework.API;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.Packets;
using TeaFramework.Utilities.Extensions;
using Terraria.ModLoader;

namespace TeaFramework.Features.Packets
{
    public class PacketHandlerLoader : IContentLoader
    {
        public bool AppliesTo(ILoadable loadable) {
            return loadable is IPacketHandler;
        }

        public bool OverrideIsLoadingEnabled(IContentLoader.LoadContext context, bool isLoadingEnabled) {
            return isLoadingEnabled;
        }

        public void LoadLoadable(IContentLoader.LoadContext context, Action<ILoadable, Mod> loadLoadable) {
            loadLoadable(context.Loadable, context.Mod);

            if (context.Mod is not ITeaMod teaMod) return;

            IPacketManager? manager = teaMod.GetService<IPacketManager>();
            manager?.RegisterPacketHandler((IPacketHandler) context.Loadable);
        }

        public void AddContent(IContentLoader.LoadContext context, Action<ILoadable> addContent) {
            addContent(context.Loadable);
        }

        public void RegisterInstance(IContentLoader.LoadContext context, Action<ILoadable> registerInstance) {
            registerInstance(context.Loadable);
        }
    }
}