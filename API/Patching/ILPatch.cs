using System;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;

namespace TeaFramework.API.Patching
{
    /// <summary>
    ///     A readonly struct that manages the application of IL edits.
    /// </summary>
    public readonly struct ILPatch : IMonoModPatch
    {
        /// <summary>
        ///     The method being IL edited.
        /// </summary>
        public readonly MethodInfo BaseMethod;

        /// <summary>
        ///     The method performing the IL edit.
        /// </summary>
        public readonly MethodInfo PatchMethod;

        /// <summary>
        ///     The resulting <see cref="ILContext.Manipulator"/> delegate.
        /// </summary>
        public readonly Delegate PatchDelegate;

        public ILPatch(MethodInfo baseMethod, MethodInfo patchMethod)
        {
            BaseMethod = baseMethod;
            PatchMethod = patchMethod;

            PatchDelegate = Delegate.CreateDelegate(typeof(ILContext.Manipulator), PatchMethod);
        }

        /// <summary>
        ///     Applies the IL edit.
        /// </summary>
        public void Apply() => HookEndpointManager.Modify(BaseMethod, PatchDelegate);

        /// <summary>
        ///     Unapplies the IL edit.
        /// </summary>
        public void Unapply() => HookEndpointManager.Modify(BaseMethod, PatchDelegate);
    }
}