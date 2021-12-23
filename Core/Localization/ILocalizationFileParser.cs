#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace TeaFramework.Core.Localization
{
    /// <summary>
    ///     Implementable file parsing for localization files.
    /// </summary>
    public interface ILocalizationFileParser
    {
        /// <summary>
        ///     Parses a file that isn't part of the <paramref name="mod"/>'s <c>.tmod</c> file.
        /// </summary>
        IDictionary<string, ModTranslation> ParseNonModFile(
            Mod mod,
            string culture,
            string filePath,
            Dictionary<string,
                ModTranslation> translations
        )
        {
            using Stream stream = File.OpenRead(filePath);
            return ParseStream(mod, culture, stream, translations);
        }

        /// <summary>
        ///     Parses a file present in the mod.
        /// </summary>
        IDictionary<string, ModTranslation> ParseModFile(
            Mod mod,
            string culture,
            string modFilePath,
            Dictionary<string, ModTranslation> translations
        )
        {
            using Stream stream = mod.GetFileStream(modFilePath);
            return ParseStream(mod, culture, stream, translations);
        }

        /// <summary>
        ///     Reads the text from the <paramref name="stream"/> and parses it through <see cref="ParseText"/>.
        /// </summary>
        IDictionary<string, ModTranslation> ParseStream(
            Mod mod,
            string culture,
            Stream stream,
            Dictionary<string, ModTranslation> translations
        )
        {
            using StreamReader reader = new(stream);
            return ParseText(mod, culture, reader.ReadToEnd(), translations);
        }

        /// <summary>
        ///     Parses text to create localization translations.
        /// </summary>
        IDictionary<string, ModTranslation> ParseText(
            Mod mod,
            string culture,
            string text,
            Dictionary<string, ModTranslation> translations);
    }
}