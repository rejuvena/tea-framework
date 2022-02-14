#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections.Generic;
using Terraria.ModLoader;
using Tomlet;
using Tomlet.Models;

namespace TeaFramework.Core.Localization.Implementation
{
    public class TomlFileParser : ILocalizationFileParser
    {
        IDictionary<string, ModTranslation> ILocalizationFileParser.ParseText(
            Mod mod,
            string culture,
            string text,
            Dictionary<string, ModTranslation> translations) => ParseText(mod, culture, text, translations);

        public static IDictionary<string, ModTranslation> ParseText(
            Mod mod,
            string culture,
            string text,
            Dictionary<string, ModTranslation> translations
        )
        {
            TomlDocument document = new TomlParser().Parse(text);

            foreach ((string tomlKey, TomlValue tomlValue) in document.Entries)
            {
                Dictionary<string, string> values = new();
                GetValues(tomlKey, tomlValue, values);

                foreach ((string key, string value) in values)
                {
                    if (!translations.TryGetValue(key, out ModTranslation? translation))
                        translation = translations[key] = LocalizationLoader.GetOrCreateTranslation(mod, key);

                    translation.AddTranslation(culture, value.Replace("\\n", "\n"));
                }
            }

            return translations;
        }

        public static void GetValues(string key, TomlValue toml, Dictionary<string, string> values)
        {
            if (toml is TomlTable table)
            {
                foreach ((string s, TomlValue value) in table.Entries)
                    // key + "." + kvp.Key is needed to the name of the table is added to the translations key
                    GetValues(key + "." + s, value, values);

                return;
            }

            values.Add(key, toml.StringValue);
        }
    }
}