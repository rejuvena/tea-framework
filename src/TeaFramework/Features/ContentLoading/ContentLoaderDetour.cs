using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeaFramework.API;
using TeaFramework.API.Exceptions;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.Features.Patching;
using TeaFramework.Features.Utility;
using TeaFramework.Utilities.Extensions;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TeaFramework.Features.ContentLoading
{
    public class ContentLoaderDetour : Patch<ContentLoaderDetour.AddContent>
    {
        public delegate void Orig(Mod self, ILoadable instance);

        public delegate void AddContent(Orig orig, Mod self, ILoadable instance);

        public override MethodInfo ModifiedMethod { get; } =
            typeof(Mod).GetCachedMethod("AddContent", new[] {typeof(ILoadable)});

        public override AddContent PatchMethod { get; } = (orig, self, instance) => {
            if (self is not ITeaMod teaMod)
            {
                orig(self, instance);
                return;
            }

            IEnumerable<IContentLoader>? contentLoaders = null;
            teaMod.GetService<TeaFrameworkApi.ContentLoadersProvider>()?.Invoke(out contentLoaders);

            if (contentLoaders is null)
            {
                orig(self, instance);
                return;
            }

            IContentLoader? contentLoader = contentLoaders.FirstOrDefault(x => x.AppliesTo(instance));

            if (contentLoader is null)
            {
                orig(self, instance);
                return;
            }
            
            bool? loading = (bool?) Reflection<Mod>.InvokeFieldGetter("loading", self);
            
            if (!loading.HasValue || !loading.Value)
                throw new TeaModLoadException(Language.GetTextValue("tModLoader.LoadErrorNotLoading"));

            IContentLoader.LoadContext context = new(instance, self);
            bool isLoadingEnabled = instance.IsLoadingEnabled(self);

            if (!contentLoader.OverrideIsLoadingEnabled(context, isLoadingEnabled))
                return;

            IList<ILoadable>? content = (IList<ILoadable>?) Reflection<Mod>.InvokeFieldGetter("content", self);

            if (content is null)
                throw new TeaModLoadException(
                    "Could not obtain field \"content\" of type \"IList<ILoadable>\" from \"Mod\"!"
                );
                
            contentLoader.LoadLoadable(context, (loadable, mod) => loadable.Load(mod));
            contentLoader.AddContent(context, loadable => content.Add(loadable));
            contentLoader.RegisterInstance(context, ContentInstance.Register);
        };
    }
}
