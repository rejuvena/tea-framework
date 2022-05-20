using System.Collections.Generic;
using log4net;
using TeaFramework.API.DependencyInjection;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Logging;
using TeaFramework.Features.Events;
using Terraria.ModLoader;

namespace TeaFramework.API
{
    /// <summary>
    ///     Represents the core data stored in a Tea Framework mod.
    /// </summary>
    public interface ITeaMod
    {
        /// <summary>
        ///     The associated tModLoader <see cref="Mod"/>.
        /// </summary>
        Mod ModInstance { get; }

        /// <summary>
        ///     This Tea Framework Mod's service provider.
        /// </summary>
        IApiServiceProvider ApiServiceProvider { get; }

        /// <summary>
        ///     A wrapped <see cref="ILog"/> instance.
        /// </summary>
        ILogWrapper LogWrapper { get; }

        /// <summary>
        ///     Retrieves a collection of content loaders.
        /// </summary>
        /// <returns>The content loaders to retrieve.</returns>
        IEnumerable<IContentLoader> ContentLoaders { get; }

        /// <summary>
        ///     The mod's <see cref="IEventBus"/> for handling events.
        /// </summary>
        IEventBus EventBus { get; }

        void GetLoadSteps(out IList<ILoadStep> steps);

        /// <summary>
        ///     Returns a collection of default content loaders.
        /// </summary>
        public static IEnumerable<IContentLoader> GetDefaultContentLoaders() =>
            new IContentLoader[] {new EventListenerLoader()};
    }
}
