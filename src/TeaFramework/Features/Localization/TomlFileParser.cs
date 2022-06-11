using System.Collections.Generic;
using System.IO;
using TeaFramework.API;
using TeaFramework.API.Features.Localization;
using Terraria.ModLoader;
using Tomlet;
using Tomlet.Models;

namespace TeaFramework.Features.Localization
{
    public class TomlFileParser : ILocalizationFileParser
    {
        public void ParseFileStream(
            ITeaMod teaMod,
            string culture,
            Dictionary<string, ModTranslation> translations,
            Stream stream
        ) {
            using StreamReader reader = new(stream);
            string tomlDocText = reader.ReadToEnd();
            TomlDocument doc = new TomlParser().Parse(tomlDocText);

            foreach ((string s, TomlValue val) in doc.Entries) {
                Dictionary<string, string> values = new();
                GetValues(s, val, values);

                foreach (KeyValuePair<string, string> tr in values) {
                    string key = tr.Key;
                    string value = tr.Value.Replace("\\n", "\n");

                    if (!translations.TryGetValue(key, out ModTranslation? translation))
                        translation = translations[key] = Utilities.Localization.GetOrCreateTranslation(teaMod.ModInstance, key);

                    translation.AddTranslation(culture, value);
                }
            }
        }

        public static void GetValues(string key, TomlValue val, Dictionary<string, string> values) {
            if (val is TomlTable table) {
                foreach ((string s, TomlValue value) in table.Entries) GetValues(key + "." + s, value, values);
                return;
            }

            values.Add(key, val.StringValue);
        }
    }
}