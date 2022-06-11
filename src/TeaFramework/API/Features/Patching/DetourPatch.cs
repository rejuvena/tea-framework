using System.Reflection;
using MonoMod.RuntimeDetour;

namespace TeaFramework.API.Features.Patching
{
    /// <summary>
    ///     A readonly struct that manages the application of method detours.
    /// </summary>
    public readonly struct DetourPatch : IMonoModPatch
    {
        /// <summary>
        ///     The method being detoured.
        /// </summary>
        public readonly MethodBase BaseMethod;

        /// <summary>
        ///     The method performing the detour.
        /// </summary>
        public readonly MethodInfo PatchMethod;

        /// <summary>
        ///     The resulting detour hook.
        /// </summary>
        public readonly Hook PatchHook;

        public DetourPatch(MethodBase baseMethod, MethodInfo patchMethod, IPatch patch) {
            BaseMethod = baseMethod;
            PatchMethod = patchMethod;

            PatchHook = new Hook(BaseMethod, PatchMethod, patch);
        }

        /// <summary>
        ///     Applies the method detour.
        /// </summary>
        public void Apply() {
            PatchHook.Apply();
        }

        /// <summary>
        ///     Unapplies the method detour.
        /// </summary>
        public void Unapply() {
            if (PatchHook.IsApplied) PatchHook.Undo();
        }
    }
}