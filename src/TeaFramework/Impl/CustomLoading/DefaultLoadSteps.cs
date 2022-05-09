using System.Collections.Generic;
using TeaFramework.API.CustomLoading;
using TeaFramework.API.Patching;
using TeaFramework.Impl.Utility;
using Terraria.ModLoader;

namespace TeaFramework.Impl.CustomLoading
{
    /// <summary>
    /// Load steps that every <see cref="TeaMod"/> uses by default.
    /// </summary>
    public static class DefaultLoadSteps
    {
        public const float ContentInstanceRegisterWeight = 1f;
        public const float LoadingTrueWeight = 2f;
        public const float LoadMonoModHooksWeight = 3.5f;
        public const float AutoloadConfigWeight = 3f;
        public const float PrepareAssetsWeight = 4f;
        public const float AutoloadWeight = 5f;
        public const float LoadWeight = 6f;
        public const float OnModLoadWeight = 7f;
        public const float LoadingFalseWeight = 8f;

        public static readonly ILoadStep ContentInstanceRegister = new LoadStep(
            nameof(ContentInstanceRegister),
            ContentInstanceRegisterWeight,
            teaMod => ContentInstance.Register(teaMod.ModInstance),
            teaMod => { }
        );

        public static readonly ILoadStep LoadingTrue = new LoadStep(
            nameof(LoadingTrue),
            LoadingTrueWeight,
            teaMod => Reflection<Mod>.InvokeFieldSetter("loading", teaMod, true),
            teaMod => { }
        );

        public static readonly ILoadStep LoadMonoModHooks = new LoadStep(
            "LoadMonoModHooks",
            LoadMonoModHooksWeight,
            _ => MonoModHooks.RequestNativeAccess(),
            teaMod => {
                if (teaMod is IPatchRepository repo)
                    foreach (IMonoModPatch patch in repo.Patches)
                        patch.Unapply();
            });

        public static readonly ILoadStep AutoloadConfig = new LoadStep(
            nameof(AutoloadConfig),
            AutoloadConfigWeight,
            teaMod => typeof(Mod).GetCachedMethod("AutoloadConfig").Invoke(teaMod.ModInstance, null),
            teaMod => { }
        );

        public static readonly ILoadStep PrepareAssets = new LoadStep(
            nameof(PrepareAssets),
            PrepareAssetsWeight,
            teaMod => typeof(Mod).GetCachedMethod("PrepareAssets").Invoke(teaMod.ModInstance, null),
            teaMod => { }
        );

        public static readonly ILoadStep Autoload = new LoadStep(
            nameof(Autoload),
            AutoloadWeight,
            teaMod => typeof(Mod).GetCachedMethod("Autoload").Invoke(teaMod.ModInstance, null),
            teaMod => { }
        );

        public static readonly ILoadStep Load = new LoadStep(
            nameof(Load),
            LoadWeight,
            teaMod => teaMod.ModInstance.Load(),
            teaMod => { }
        );

        public static readonly ILoadStep OnModLoad = new LoadStep(
            nameof(OnModLoad),
            OnModLoadWeight,
            teaMod => typeof(SystemLoader).GetCachedMethod("OnModLoad")
                .Invoke(null, new object?[] {teaMod.ModInstance}),
            teaMod => { }
        );

        public static readonly ILoadStep LoadingFalse = new LoadStep(
            nameof(LoadingFalse),
            LoadingFalseWeight,
            teaMod => Reflection<Mod>.InvokeFieldSetter("loading", teaMod, false),
            teaMod => { }
        );

        public static List<ILoadStep> GetDefaultSteps() => new() {
            ContentInstanceRegister,
            LoadingTrue,
            AutoloadConfig,
            PrepareAssets,
            Autoload,
            Load,
            OnModLoad,
            LoadingFalse
        };
    }
}
