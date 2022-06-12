using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeaFramework.API;
using TeaFramework.API.Exceptions;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.Features.Patching;
using TeaFramework.Utilities;
using TeaFramework.Utilities.Extensions;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TeaFramework.Features.ContentLoading
{
    /// <summary>
    ///     Handles content loaders through a detour patch..
    /// </summary>
    public class ContentLoaderDetour : Patch<ContentLoaderDetour.AddContent>
    {
        /// <summary>
        ///     The delegate used for the detour.
        /// </summary>
        public delegate void AddContent(Orig orig, Mod self, ILoadable instance);

        /// <summary>
        ///     Represents <see cref="Mod.AddContent" />.
        /// </summary>
        public delegate void Orig(Mod self, ILoadable instance);

        public override MethodInfo ModifiedMethod { get; } =
            typeof(Mod).GetCachedMethod("AddContent", new[] {typeof(ILoadable)});

        protected override AddContent PatchMethod { get; } = (orig, self, instance) =>
        {
            // Do normal loading if this mod doesn't utilize Tea Framework's loading API.
            if (self is not ITeaMod teaMod) {
                orig(self, instance);
                return;
            }

            // Retrieve the collection of content loaders as a service.
            IContentLoadersProvider? provider = teaMod.GetService<IContentLoadersProvider>();
            if (provider is null) return;

            IEnumerable<IContentLoader> contentLoaders = provider.GetContentLoaders();

            if (contentLoaders is null) return;

            // If this mod provides no content loaders, load content as normal.
            if (contentLoaders is null) {
                orig(self, instance);
                return;
            }

            // Get the first content loader that applies to this loadable object.
            IContentLoader? contentLoader = contentLoaders.FirstOrDefault(x => x.AppliesTo(instance));

            // If no content loaders are found, load content as normal.
            if (contentLoader is null) {
                orig(self, instance);
                return;
            }

            // BEGIN: Reconstruct tModLoader load steps.
            // https://github.com/tModLoader/tModLoader/blob/1.4/patches/tModLoader/Terraria/ModLoader/Mod.cs
            //
            // Step 1:
            // if (!loading)
            //     throw new Exception(Language.GetTextValue("tModLoader.LoadErrorNotLoading"));
            //
            // Step 2:
            // if (instance.IsLoadingEnabled(this)) {
            //     Step 3:
            //     instance.Load(this);

            //     Step 4:
            //     content.Add(instance);
            //
            //     Step 5:
            //     ContentInstance.Register(instance);
            // }

            // Step 1: Ensure the mod is loading.
            bool? loading = (bool?) Reflection<Mod>.InvokeFieldGetter("loading", self);

            if (!loading.HasValue || !loading.Value) throw new TeaModLoadException(Language.GetTextValue("tModLoader.LoadErrorNotLoading"));

            // Build a load content for various API methods.
            IContentLoader.LoadContext context = new(instance, self);

            // Step 2: check if loading is enabled, allow content loaders to override.
            bool isLoadingEnabled = instance.IsLoadingEnabled(self);

            if (!contentLoader.OverrideIsLoadingEnabled(context, isLoadingEnabled)) return;

            // Verify that we can continue with normal loading.
            // This should never actually be null, but better safe than sorry.
            IList<ILoadable>? content = (IList<ILoadable>?) Reflection<Mod>.InvokeFieldGetter("content", self);

            if (content is null)
                throw new TeaModLoadException(
                    "Could not obtain field \"content\" of type \"IList<ILoadable>\" from \"Mod\"!"
                );

            // Step 3.
            contentLoader.LoadLoadable(context, (loadable, mod) => loadable.Load(mod));

            // Step 4.
            contentLoader.AddContent(context, loadable => content.Add(loadable));

            // Step 5.
            contentLoader.RegisterInstance(context, ContentInstance.Register);
        };
    }
}