using System.Reflection;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.Patching
{
    /// <summary>
    ///     Represents a method detour or IL edit.
    /// </summary>
    public interface IPatch : ILoadable
    {
        /// <summary>
        ///     The mod that owns this patch.
        /// </summary>
        Mod Mod { get; }

        /// <summary>
        ///     The method being patched.
        /// </summary>
        MethodInfo ModifiedMethod { get; }

        /// <summary>
        ///     The method being applied as a patch.
        /// </summary>
        MethodInfo ModifyingMethod { get; }

        /// <summary>
        ///     The delegate implementation to use as a patch.
        /// </summary>
        object PatchMethod { get; }

        /// <summary>
        ///     Applies this patch.
        /// </summary>
        /// <param name="patchRepository">The patch repository this patch originates from.</param>
        void Apply(IPatchRepository patchRepository);
    }
}
