#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeaFramework.Core.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace TeaFramework.Core.Localization.Implementation
{
    /// <summary>
    ///     Default implementation of <see cref="ILocalizationLoader"/>.
    /// </summary>
    public class LocalizationLoader : ILocalizationLoader
    {
        public virtual Dictionary<string, ILocalizationFileParser> ExtensionsToParsers { get; }

        public LocalizationLoader(bool populateWithDefaults = true)
        {
            Dictionary<string, ILocalizationFileParser> extensionsToParsers = new();

            if (populateWithDefaults)
            {
                extensionsToParsers.Add("lang", new LangFileParser());
                extensionsToParsers.Add("toml", new TomlFileParser());
            }

            ExtensionsToParsers = extensionsToParsers;
        }

        public virtual void Load(Mod mod)
        {
            IEnumerable<KeyValuePair<string, TmodFile.FileEntry>> files = mod.GetPropertyValue<Mod, TmodFile>("File")
                .GetFieldValue<TmodFile, IDictionary<string, TmodFile.FileEntry>>("files");


            files = files.Where(x =>
            {
                string[] splitText = SplitFilePath(x.Key);

                return splitText.Length == 2 && ExtensionsToParsers.Keys.Any(y => y.Equals(splitText[1]));
            });

            Dictionary<string, ModTranslation> translations = new();

            foreach ((string key, TmodFile.FileEntry _) in files)
            {
                using Stream stream = mod.GetFileStream(key);
                string[] splitText = SplitFilePath(key);
                string culture = splitText[0];
                string extension = splitText[1];

                ILocalizationFileParser parser = ExtensionsToParsers.First(x => x.Key.Equals(extension)).Value;
                parser.ParseStream(mod, culture, stream, translations);
            }

            foreach (ModTranslation translation in translations.Values)
                Terraria.ModLoader.LocalizationLoader.AddTranslation(translation);
        }

        public virtual void Unload()
        {
        }

        public static string[] SplitFilePath(string path) => path.Split('/').Last().Split('.');

        public static ModTranslation GetOrCreateTranslation(Mod mod, string key, bool defaultEmpty = false)
        {
            key = $"Mods.{mod.Name}.${key}".Replace(' ', '_');

            return typeof(LocalizationLoader)
                .GetCachedField("translations")
                .GetValue<Dictionary<string, ModTranslation>>()
                .TryGetValue(key, out ModTranslation? modTranslation)
                ? modTranslation
                : (ModTranslation) typeof(ModTranslation).GetCachedConstructor(
                    typeof(string),
                    typeof(bool)
                ).Invoke(new object[] {key, defaultEmpty});
        }
    }
}