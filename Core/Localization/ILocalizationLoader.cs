#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections.Generic;
using TeaFramework.Common.Utilities.EnumerableMatching;
using Terraria.ModLoader;

namespace TeaFramework.Core.Localization
{
    /// <summary>
    ///     Provides an implementable localization loader for a mod.
    /// </summary>
    /// <remarks>
    ///     Does *not* auto-load.
    /// </remarks>
    [Autoload(false)]
    public interface ILocalizationLoader : ILoadable
    {
        /// <summary>
        ///     A dictionary of all extensions that should get parsed by an instance of <see cref="ILocalizationFileParser"/>.
        /// </summary>
        Dictionary<StringMacher, ILocalizationFileParser> ExtensionsToParsers { get; }
    }
}