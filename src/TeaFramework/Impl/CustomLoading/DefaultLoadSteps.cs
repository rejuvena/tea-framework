using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using TeaFramework.API.CustomLoading;
using TeaFramework.API.Events;
using TeaFramework.API.Patching;
using TeaFramework.Impl.Utility;
using Terraria;
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
        public const float ClearEquipTexturesWeight = 5f;
        public const float ClearContentWeight = 6f;
        public const float AutoloadWeight = 7f;
        public const float UnsubscribeEventsWeight = 8f;
        public const float LoadWeight = 9f;
        public const float OnModLoadWeight = 10f;
        public const float LoadingFalseWeight = 11f;

        /// <summary>
        ///     When loading: registers the mod instance using <see cref="ContentInstance.Register"/>. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep ContentInstanceRegister = new LoadStep(
            nameof(ContentInstanceRegister),
            ContentInstanceRegisterWeight,
            teaMod => ContentInstance.Register(teaMod.ModInstance),
            teaMod => { }
        );

        /// <summary>
        ///     When loading: sets <see cref="Mod.Load"/> to true. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep LoadingTrue = new LoadStep(
            nameof(LoadingTrue),
            LoadingTrueWeight,
            teaMod => Reflection<Mod>.InvokeFieldSetter("loading", teaMod, true),
            teaMod => { }
        );

        /// <summary>
        ///     When loading: requests native access to MonoMod. <br />
        ///     When unloading: unloads native MonoMod patches.
        /// </summary>
        public static readonly ILoadStep LoadMonoModHooks = new LoadStep(
            "LoadMonoModHooks",
            LoadMonoModHooksWeight,
            _ => MonoModHooks.RequestNativeAccess(),
            teaMod => {
                if (teaMod is not IPatchRepository repo)
                    return;

                Main.QueueMainThreadAction(() => {
                    foreach (IMonoModPatch patch in repo.Patches)
                        patch.Unapply();
                });
            });

        /// <summary>
        ///     When loading: invokes <see cref="Mod.AutoloadConfig"/>. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep AutoloadConfig = new LoadStep(
            nameof(AutoloadConfig),
            AutoloadConfigWeight,
            teaMod => typeof(Mod).GetCachedMethod("AutoloadConfig").Invoke(teaMod.ModInstance, null),
            teaMod => { }
        );

        /// <summary>
        ///     When loading: invokes <see cref="Mod.PrepareAssets"/>. <br />
        ///     When unloading: disposes of the mod's asset repository.
        /// </summary>
        public static readonly ILoadStep PrepareAssets = new LoadStep(
            nameof(PrepareAssets),
            PrepareAssetsWeight,
            teaMod => typeof(Mod).GetCachedMethod("PrepareAssets").Invoke(teaMod.ModInstance, null),
            teaMod => {
                AssetRepository? assetRepository = (AssetRepository?) Reflection<Mod>.InvokePropertyGetter("Assets", teaMod.ModInstance);
                assetRepository?.Dispose();
            }
        );
        
        /// <summary>
        ///     When loading: N/A. <br />
        ///     When unloading: Clears the equipTextures dictionary.
        /// </summary>
        public static ILoadStep ClearEquipTextures = new LoadStep(
            nameof(ClearEquipTextures),
            ClearEquipTexturesWeight,
            teaMod => { },
            teaMod => {
                IDictionary<Tuple<string, EquipType>, EquipTexture>? equipTextures =
                    (IDictionary<Tuple<string, EquipType>, EquipTexture>?)Reflection<Mod>.InvokeFieldGetter(
                        "equipTextures", teaMod.ModInstance);

                equipTextures?.Clear();
            }
        );

        /// <summary>
        ///     When loading: N/A. <br />
        ///     When unloading: Clears the content list.
        /// </summary>
        public static ILoadStep ClearContent = new LoadStep(
            nameof(ClearContent),
            ClearContentWeight,
            teaMod => { },
            teaMod => {
                IList<ILoadable>? content = (IList<ILoadable>?) Reflection<Mod>.InvokeFieldGetter("content", teaMod.ModInstance);
                content?.Clear();
            }
        );

        /// <summary>
        ///     When loading: invokes <see cref="Mod.Autoload"/>. <br />
        ///     When unloading: unloads all loadables in reverse.
        /// </summary>
        public static readonly ILoadStep Autoload = new LoadStep(
            nameof(Autoload),
            AutoloadWeight,
            teaMod => typeof(Mod).GetCachedMethod("Autoload").Invoke(teaMod.ModInstance, null),
            teaMod => {
                IList<ILoadable>? content = (IList<ILoadable>?) Reflection<Mod>.InvokeFieldGetter("content", teaMod.ModInstance);

                if (content is null)
                    return;
                
                foreach (ILoadable loadable in content.Reverse())
                    loadable.Unload();
            }
        );

        public static readonly ILoadStep UnsubscribeEvents = new LoadStep(
            nameof(UnsubscribeEvents),
            UnsubscribeEventsWeight,
            teaMod => { },
            teaMod => {
                Main.QueueMainThreadAction(() => {
                    IEventListener[] listeners = teaMod.EventBus.Listeners.Values.SelectMany(listeners => listeners)
                        .ToArray();

                    foreach (IEventListener listener in listeners)
                        teaMod.EventBus.Unsubscribe(listener);
                });
            }
        );

        // ATTENTION: TeaMod seals off Load and Unload, but ITeaMod does not. This is relevant.
        /// <summary>
        ///     When loading: invokes <see cref="Mod.Load"/>. <br />
        ///     When unloading: invokes <see cref="Mod.Unload"/>.
        /// </summary>
        public static readonly ILoadStep Load = new LoadStep(
            nameof(Load),
            LoadWeight,
            teaMod => teaMod.ModInstance.Load(),
            teaMod => teaMod.ModInstance.Unload()
        );

        /// <summary>
        ///     When loading: invokes <see cref="SystemLoader.OnModLoad"/>. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep OnModLoad = new LoadStep(
            nameof(OnModLoad),
            OnModLoadWeight,
            teaMod => typeof(SystemLoader).GetCachedMethod("OnModLoad")
                .Invoke(null, new object?[] {teaMod.ModInstance}),
            teaMod => { }
        );

        /// <summary>
        ///     When loading: sets <see cref="Mod.Load"/> to false. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep LoadingFalse = new LoadStep(
            nameof(LoadingFalse),
            LoadingFalseWeight,
            teaMod => Reflection<Mod>.InvokeFieldSetter("loading", teaMod, false),
            teaMod => { }
        );

        /// <summary>
        ///     The default steps used by Tea mods.
        /// </summary>
        public static List<ILoadStep> GetDefaultSteps() => new() {
            ContentInstanceRegister,
            LoadingTrue,
            LoadMonoModHooks,
            AutoloadConfig,
            PrepareAssets,
            ClearEquipTextures,
            ClearContent,
            Autoload,
            UnsubscribeEvents,
            Load,
            OnModLoad,
            LoadingFalse
        };
    }
}
