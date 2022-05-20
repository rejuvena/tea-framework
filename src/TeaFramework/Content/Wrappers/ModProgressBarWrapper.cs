using System;
using TeaFramework.Features.Utility;
using Terraria.ModLoader;

namespace TeaFramework.Content.Wrappers
{
    public readonly struct ModProgressBarWrapper
    {
        private static readonly Type UIProgressType = typeof(ModLoader).Assembly
            .GetCachedType("Terraria.ModLoader.UI.UIProgress");
        
        private readonly object UIProgress;

        public string DisplayText
        {
            get => (string) Reflection.InvokeFieldGetter(UIProgressType, nameof(DisplayText), UIProgress)!;
            set => Reflection.InvokeFieldSetter(UIProgressType, nameof(DisplayText), UIProgress, value);
        }

        public float Progress
        {
            get => (float) Reflection.InvokePropertyGetter(UIProgressType, nameof(Progress), UIProgress)!;
            set => Reflection.InvokePropertySetter(UIProgressType, nameof(Progress), UIProgress, value);
        }

        public string SubProgressText
        {
            set => Reflection.InvokePropertySetter(UIProgressType, nameof(SubProgressText), UIProgress, value);
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
