#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace TeaFramework.Core.Localization.Implementation
{
    /// <summary>
    ///     Parses <c>.lang</c> files for localization.
    /// </summary>
    public class LangFileParser : ILocalizationFileParser
    {
        public IDictionary<string, ModTranslation> ParseText(Mod mod, string culture, string text,
            Dictionary<string, ModTranslation> translations)
        {
            using StringReader reader = new(text);

            while ((text = reader.ReadLine() ?? "") != null)
            {
                int index = text.IndexOf('=');

                if (index < 0)
                    continue;

                string key = text[..index].Trim().Replace(' ', '_');
                string value = text[(index + 1)..];

                if (value.Length == 0)
                    continue;

                value = value.Replace("\\n", "\n");

                if (!translations.TryGetValue(key, out ModTranslation? translation))
                    translation = translations[key] = LocalizationLoader.GetOrCreateTranslation(mod, key);

                translation.AddTranslation(culture, value);
            }

            return translations;
        }
    }
}