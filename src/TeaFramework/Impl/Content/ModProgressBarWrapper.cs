using System;
using TeaFramework.Impl.Utility;
using Terraria.ModLoader;

namespace TeaFramework.Impl.Content
{
    public readonly struct ModProgressBarWrapper
    {
        private static readonly Type UIProgressType = typeof(ModLoader).Assembly
            .GetCachedType("Terraria.ModLoader.UI.UIProgress");
        
        private readonly object UIProgress;

        public string DisplayText
        {
            get => (string) UIProgress.GetFieldValue("DisplayText")!;
            set => UIProgress.SetFieldValue("DisplayText", value);
        }

        public float Progress
        {
            get => (float)UIProgress.GetPropertyValue("Progress")!;
            set => UIProgress.SetPropertyValue("Progress", value);
        }

        public string SubProgressText
        {
            set => UIProgress.SetPropertyValue("SubProgressText", value);
        }
        
        public ModProgressBarWrapper(object uiProgress)
        {
            UIProgress = uiProgress;
        }

        public static ModProgressBarWrapper MakeDefault() => new(typeof(ModLoader).Assembly
            .GetCachedType("Terraria.ModLoader.UI.Interface")
            .GetCachedField("loadMods")
            .GetValue(null)!);
    }
}
