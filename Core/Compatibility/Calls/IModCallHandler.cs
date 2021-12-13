#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using Terraria.ModLoader;

namespace TeaFramework.Core.Compatibility.Calls
{
    /// <summary>
    ///     Handles a <see cref="Mod.Call"/> that matches the <see cref="Accessor"/>.
    /// </summary>
    public interface IModCallHandler
    {
        /// <summary>
        ///     The first string in a passed argument array to match.
        /// </summary>
        string Accessor { get; }

        /// <summary>
        ///     What the handler does if the <see cref="Mod.Call"/> points to it.
        /// </summary>
        /// <param name="mod">The mod instance.</param>
        /// <param name="args">The arguments passed by the user.</param>
        /// <returns>Whatever you want.</returns>
        object Action(Mod mod, params object[] args);
    }
}