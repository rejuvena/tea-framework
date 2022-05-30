using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.Localization
{
    /// <summary>
    ///     Parses a localization file for translations.
    /// </summary>
    public interface ILocalizationFileParser
    {
        /// <summary>
        ///     Parses a stream into a dictionary of keys to translations.
        /// </summary>
        /// <param name="teaMod">The tea mod instance.</param>
        /// <param name="culture">The culture to parse for.</param>
        /// <param name="translations">The parser translation map.</param>
        /// <param name="stream">The stream to parse.</param>
        void ParseFileStream(ITeaMod teaMod, string culture, Dictionary<string, ModTranslation> translations, Stream stream);
    }
}