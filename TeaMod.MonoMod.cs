#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using TeaFramework.Core.Reflection;

namespace TeaFramework
{
    partial class TeaMod
    {
        /// <summary>
        ///     Creates a <see cref="Delegate"/> that gets hooked into the <see cref="HookEndpointManager"/> through <see cref="HookEndpointManager.Modify"/>. <br />
        ///     Used for IL editing.
        /// </summary>
        /// <param name="method">The method to modify.</param>
        /// <param name="modifyingType">The type that contains the modifying method.</param>
        /// <param name="methodName">The name of the modifying method.</param>
        public virtual void CreateEdit(MethodInfo method, Type modifyingType, string methodName)
        {
            Delegate callback = new ILContext.Manipulator(il =>
                modifyingType.GetCachedMethod(methodName).Invoke(null, new object[] {il})
            );
            HookEndpointManager.Modify(method, callback);
            EditsToRemove.Add((method, callback));
        }

        /// <summary>
        ///     Creates a <see cref="Hook"/> that gets applied to the specified <paramref name="modifiedMethod"/>. <br />
        ///     Used for method detouring. <br />
        ///     Inlined to log detouring correctly.
        /// </summary>
        /// <param name="modifiedMethod">The method to modify.</param>
        /// <param name="modifyingMethod">The method doing the modifying.</param>
        public virtual void CreateDetour(MethodInfo modifiedMethod, MethodInfo modifyingMethod)
        {
            Hook hook = new(modifiedMethod, modifyingMethod);
            hook.Apply();
            DetoursToRemove.Add(hook);
        }
    }
}