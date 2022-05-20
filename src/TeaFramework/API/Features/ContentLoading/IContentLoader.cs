using System;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.ContentLoading
{
    /// <summary>
    ///     Provides modders with a way to conditionally manipulate the loading of <see cref="ILoadable"/> objects.
    /// </summary>
    /// <remarks>
    ///     There should only be one content loader per loadable type. Collisions are not supported and are not planned to be. <br />
    ///     You may override content loaders used by Tea Framework if you so wish. <br />
    ///     Each mod uses their own defined content loaders. If you are making a library, properly inform modders to use your content loaders.
    /// </remarks>
    public interface IContentLoader
    {
        /// <summary>
        ///     Provides universal content for loadable steps.
        /// </summary>
        public readonly record struct LoadContext(ILoadable Loadable, Mod Mod);
        
        /// <summary>
        ///     Whether this content loader should apply to this <see cref="ILoadable"/> object.
        /// </summary>
        /// <param name="loadable">The <see cref="ILoadable"/> object.</param>
        /// <returns><see langkey="true"/> if this content loader should manipulate the loading of this object.</returns>
        bool AppliesTo(ILoadable loadable);

        /// <summary>
        ///     Allows you to manipulate whether loading is enabled for this <see cref="ILoadable"/>.
        /// </summary>
        /// <param name="context">Loading context.</param>
        /// <param name="isLoadingEnabled">The original value of <see cref="ILoadable.IsLoadingEnabled"/>.</param>
        /// <returns>The new value for <see cref="ILoadable.IsLoadingEnabled"/>.</returns>
        bool OverrideIsLoadingEnabled(LoadContext context, bool isLoadingEnabled);

        /// <summary>
        ///     Intercepts <see cref="ILoadable"/> loading.
        /// </summary>
        /// <param name="context">Loading context.</param>
        /// <param name="loadLoadable">The original method callback.</param>
        void LoadLoadable(LoadContext context, Action<ILoadable, Mod> loadLoadable);

        /// <summary>
        ///     Intercepts content addition.
        /// </summary>
        /// <param name="context">Loading context.</param>
        /// <param name="addContent">The original method callback.</param>
        void AddContent(LoadContext context, Action<ILoadable> addContent);

        /// <summary>
        ///     Intercepts content registration.
        /// </summary>
        /// <param name="context">Loading context.</param>
        /// <param name="registerInstance">The original method callback.</param>
        void RegisterInstance(LoadContext context, Action<ILoadable> registerInstance);
    }
}
