using System.Collections.Generic;
using System.IO;
using TeaFramework.API;
using TeaFramework.API.Features.Localization;
using Terraria.ModLoader;

namespace TeaFramework.Features.Localization
{
    public class LangFileParser : ILocalizationFileParser
    {
        public void ParseFileStream(
            ITeaMod teaMod,
            string culture,
            Dictionary<string, ModTranslation> translations,
            Stream stream
        ) {
            using StreamReader reader = new(stream);
            using StringReader strReader = new(reader.ReadToEnd());
            string? text;

            while ((text = strReader.ReadLine()) != null) {
                int index = text.IndexOf('=');

                if (index < 0) continue;

                string key = text[..index].Trim().Replace(' ', '_');
                string value = text[(index + 1)..];

                if (value.Length == 0) continue;

                value = value.Replace("\\n", "\n");

                if (!translations.TryGetValue(key, out ModTranslation? translation))
                    translation = translations[key] = Utility.Localization.GetOrCreateTranslation(teaMod.ModInstance, key);

                translation.AddTranslation(culture, value);
            }
        }
    }
}