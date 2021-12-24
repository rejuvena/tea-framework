using System;
using System.Reflection;
using System.Text;
using TeaFramework.Common.Utilities.Extensions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.UI;

namespace TeaFramework
{
    partial class TeaMod
    {
        private void ApplyEdits()
        {
            void Detour(MethodInfo method, string name) => CreateDetour(method, GetType().GetCachedMethod(name));
            
            Assembly terrariaAssembly = typeof(Main).Assembly;

            Detour(
                terrariaAssembly.GetCachedType("Terraria.ModLoader.UI.UIModInfo").GetCachedMethod("Show"),
                nameof(ShowLocalizedModDescription)
            );
        }

        public static void ShowLocalizedModDescription(
            Action<UIState, string, string, int, object, string, string, bool, string> orig,
            UIState self,
            string modName,
            string displayName,
            int gotoMenu,
            object localMod,
            string description = "",
            string url = "",
            bool loadFromWeb = false,
            string publishedFileId = ""
        )
        {
            if (!loadFromWeb)
            {
                TmodFile file = localMod.GetType().GetCachedField("modFile").GetValue<TmodFile>(localMod);
                string discriminator = LanguageManager.Instance.ActiveCulture.Name;
                string fileName = $"description-{discriminator}.txt";
                string fileToUse = "";
                const string englishFileName = "description-en-US.txt";

                if (file.HasFile(fileName))
                    fileToUse = fileName;
                else if (discriminator != "en-US" && file.HasFile(englishFileName)) 
                    fileToUse = englishFileName;

                if (!string.IsNullOrEmpty(fileName)) 
                    description = Encoding.UTF8.GetString(file.GetBytes(fileToUse)).Remove(0, 1); // encoding adds a * to the start
            }
            
            orig(self, modName, displayName, gotoMenu, localMod, description, url, loadFromWeb, publishedFileId);
        }
    }
}