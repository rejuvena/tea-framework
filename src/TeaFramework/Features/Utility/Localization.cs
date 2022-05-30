using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TeaFramework.Features.Utility
{
    public static class Localization
    {
        public static ModTranslation GetOrCreateTranslation(Mod mod, string key, bool defaultEmpty = false) {
            Type locLoader = typeof(LocalizationLoader);
            MethodInfo getOrCreate = locLoader.GetCachedMethod("GetOrCreateTranslation", new[] {typeof(Mod), typeof(string), typeof(bool)});
            object translation = getOrCreate.Invoke(null, new object?[] {mod, key, defaultEmpty})!;
            return (ModTranslation) translation;
        }
    }
}