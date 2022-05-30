using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeaFramework.API;
using TeaFramework.API.Features.Localization;
using TeaFramework.Features.Utility;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace TeaFramework.Features.Localization
{
    public class DefaultLocalizationLoader : ILocalizationLoader
    {
        public Dictionary<string, ILocalizationFileParser> Parsers { get; } = new();

        public void ParseFilesFromMod(ITeaMod teaMod) {
            TmodFile modFile = teaMod.ModInstance.GetPropertyValue<Mod, TmodFile>("File")!;
            IEnumerable<KeyValuePair<string, TmodFile.FileEntry>> files = modFile.GetFieldValue<TmodFile, IDictionary<string, TmodFile.FileEntry>>("files")!;

            files = files.Where(x =>
            {
                string[] splitText = SplitFilePath(x.Key);

                return splitText.Length == 2 && Parsers.Keys.Any(y => y.Equals(splitText[1]));
            });

            Dictionary<string, ModTranslation> translations = new();

            foreach ((string key, TmodFile.FileEntry _) in files) {
                using Stream stream = teaMod.ModInstance.GetFileStream(key);
                string[] splitText = SplitFilePath(key);
                string culture = splitText[0];
                string extension = splitText[1];

                if (Parsers.TryGetValue(extension, out ILocalizationFileParser? parser)) parser.ParseFileStream(teaMod, culture, translations, stream);
            }

            foreach (KeyValuePair<string, ModTranslation> translation in translations) LocalizationLoader.AddTranslation(translation.Value);
        }

        public static string[] SplitFilePath(string path) {
            return path.Split('/').Last().Split('.');
        }
    }
}