#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using Terraria.ModLoader;

namespace TeaFramework.Core.Compatibility.Calls
{
    /// <summary>
    ///     Handles <see cref="Mod.Call"/> invocation/
    /// </summary>
    public interface IModCaller
    {
        /// <summary>
        ///     Responsible for directly handling a call.
        /// </summary>
        /// <param name="mod">The mod instance.</param>
        /// <param name="args">The arguments passed by the user.</param>
        /// <returns>Whatever object the invoked call returns.</returns>
        object HandleCall(Mod mod, params object[] args);

        /// <summary>
        ///     Handles adding a new <see cref="IModCallHandler"/> instance.
        /// </summary>
        /// <param name="handler">The instance to add.</param>
        void AddCaller(IModCallHandler handler);
    }
}