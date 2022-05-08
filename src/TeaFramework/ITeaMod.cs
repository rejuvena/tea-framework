using System.Collections.Generic;
using log4net;
using TeaFramework.API.ContentLoading;
using TeaFramework.API.Logging;
using Terraria.ModLoader;

namespace TeaFramework
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
        ///     A wrapped <see cref="ILog"/> instance.
        /// </summary>
        ILogWrapper LogWrapper { get; }

        /// <summary>
        ///     Retrieves a collection of content loaders.
        /// </summary>
        /// <returns>The content loaders to retrieve.</returns>
        IEnumerable<IContentLoader> ContentLoaders { get; }

        /// <summary>
        ///     Returns a collection of default content loaders.
        /// </summary>
        public static IEnumerable<IContentLoader> GetDefaultContentLoaders() => new IContentLoader[] { };
    }
}
