using log4net;
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
    }
}
