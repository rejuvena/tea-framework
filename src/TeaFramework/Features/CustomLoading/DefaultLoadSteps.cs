using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReLogic.Content;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Patching;
using TeaFramework.Features.Utility;
using TeaFramework.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace TeaFramework.Features.CustomLoading
{
    /// <summary>
    ///     Load steps that every <see cref="TeaMod" /> uses by default, most of which are vital for replicating the default
    ///     loading of a tModLoader mod.
    /// </summary>
    public static class DefaultLoadSteps
    {
        public const float JitModWeight = 1f;
        public const float ContentInstanceRegisterWeight = 2f;

        public const float LoadingTrueWeight = 3f;

        //public const float HandleServiceProviderWeight = 4f;
        public const float LoadMonoModHooksWeight = 3f;
        public const float AutoloadConfigWeight = 4f;
        public const float PrepareAssetsWeight = 5f;
        public const float ClearEquipTexturesWeight = 6f;
        public const float ClearContentWeight = 7f;
        public const float AutoloadWeight = 8f;
        public const float UnsubscribeEventsWeight = 9f;
        public const float LoadWeight = 10f;
        public const float OnModLoadWeight = 11f;
        public const float LoadingFalseWeight = 12f;

        /// <summary>
        ///     When loading: JITs the mod assembly. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep JitMod = new LoadStep(
            nameof(JitMod),
            JitModWeight,
            teaMod =>
            {
                if (!teaMod.ModInstance.Code.GetName().Name!.StartsWith("tModLoader"))
                    AssemblyManager.JITAssemblies(
                        AssemblyManager.GetModAssemblies(teaMod.ModInstance.Name),
                        teaMod.ModInstance.PreJITFilter
                    );
            },
            _ => { }
        );

        /// <summary>
        ///     When loading: registers the mod instance using <see cref="ContentInstance.Register" />. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep ContentInstanceRegister = new LoadStep(
            nameof(ContentInstanceRegister),
            ContentInstanceRegisterWeight,
            teaMod => ContentInstance.Register(teaMod.ModInstance),
            _ => { }
        );

        /// <summary>
        ///     When loading: sets <see cref="Mod.Load" /> to true. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep LoadingTrue = new LoadStep(
            nameof(LoadingTrue),
            LoadingTrueWeight,
            teaMod => Reflection<Mod>.InvokeFieldSetter("loading", teaMod, true),
            _ => { }
        );

        /*/// <summary>
        ///     When loading: installs API services. <br />
        ///     When unloading: uninstalls API services.
        /// </summary>
        public static readonly ILoadStep HandleServiceProvider = new LoadStep(
            nameof(HandleServiceProvider),
            HandleServiceProviderWeight,
            teaMod => teaMod.InstallApis(),
            teaMod => teaMod.UninstallApis()
        );*/

        /// <summary>
        ///     When loading: requests native access to MonoMod. <br />
        ///     When unloading: unloads native MonoMod patches.
        /// </summary>
        public static readonly ILoadStep LoadMonoModHooks = new LoadStep(
            "LoadMonoModHooks",
            LoadMonoModHooksWeight,
            _ => MonoModHooks.RequestNativeAccess(),
            teaMod =>
            {
                if (teaMod is not IPatchRepository repo) return;

                Main.QueueMainThreadAction(() =>
                {
                    foreach (IMonoModPatch patch in repo.Patches) patch.Unapply();
                });
            });

        /// <summary>
        ///     When loading: invokes <see cref="Mod.AutoloadConfig" />. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep AutoloadConfig = new LoadStep(
            nameof(AutoloadConfig),
            AutoloadConfigWeight,
            teaMod => typeof(Mod).GetCachedMethod("AutoloadConfig").Invoke(teaMod.ModInstance, null),
            _ => { }
        );

        /// <summary>
        ///     When loading: invokes <see cref="Mod.PrepareAssets" />. <br />
        ///     When unloading: disposes of the mod's asset repository.
        /// </summary>
        public static readonly ILoadStep PrepareAssets = new LoadStep(
            nameof(PrepareAssets),
            PrepareAssetsWeight,
            teaMod => typeof(Mod).GetCachedMethod("PrepareAssets").Invoke(teaMod.ModInstance, null),
            teaMod =>
            {
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
            _ => { },
            teaMod =>
            {
                IDictionary<Tuple<string, EquipType>, EquipTexture>? equipTextures =
                    (IDictionary<Tuple<string, EquipType>, EquipTexture>?) Reflection<Mod>.InvokeFieldGetter(
                        "equipTextures",
                        teaMod.ModInstance
                    );

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
            _ => { },
            teaMod =>
            {
                IList<ILoadable>? content = (IList<ILoadable>?) Reflection<Mod>.InvokeFieldGetter("content", teaMod.ModInstance);
                content?.Clear();
            }
        );

        /// <summary>
        ///     When loading: invokes <see cref="Mod.Autoload" />. <br />
        ///     When unloading: unloads all loadables in reverse.
        /// </summary>
        public static readonly ILoadStep Autoload = new LoadStep(
            nameof(Autoload),
            AutoloadWeight,
            teaMod => typeof(Mod).GetCachedMethod("Autoload").Invoke(teaMod.ModInstance, null),
            teaMod =>
            {
                IList<ILoadable>? content = (IList<ILoadable>?) Reflection<Mod>.InvokeFieldGetter("content", teaMod.ModInstance);

                if (content is null) return;

                foreach (ILoadable loadable in content.Reverse()) loadable.Unload();
            }
        );

        public static readonly ILoadStep UnsubscribeEvents = new LoadStep(
            nameof(UnsubscribeEvents),
            UnsubscribeEventsWeight,
            _ => { },
            teaMod =>
            {
                Main.QueueMainThreadAction(() =>
                {
                    IEventBus? bus = teaMod.GetService<IEventBus>();

                    if (bus is null) return;

                    IEventListener[] listeners = bus.Listeners.Values.SelectMany(listeners => listeners)
                                                    .ToArray();

                    foreach (IEventListener listener in listeners) bus.Unsubscribe(listener);
                });
            }
        );

        // ATTENTION: TeaMod seals off Load and Unload, but ITeaMod does not. This is relevant.
        /// <summary>
        ///     When loading: invokes <see cref="Mod.Load" />. <br />
        ///     When unloading: invokes <see cref="Mod.Unload" />.
        /// </summary>
        public static readonly ILoadStep Load = new LoadStep(
            nameof(Load),
            LoadWeight,
            teaMod => teaMod.ModInstance.Load(),
            teaMod => teaMod.ModInstance.Unload()
        );

        /// <summary>
        ///     When loading: invokes <see cref="SystemLoader.OnModLoad" />. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep OnModLoad = new LoadStep(
            nameof(OnModLoad),
            OnModLoadWeight,
            teaMod =>
            {
                Type sysLoader = typeof(SystemLoader);
                MethodInfo modLoad = sysLoader.GetCachedMethod("OnModLoad");
                modLoad.Invoke(null, new object?[] {teaMod.ModInstance});
            },
            teaMod =>
            {
                Type sysLoader = typeof(SystemLoader);
                MethodInfo modUnload = sysLoader.GetCachedMethod("OnModUnload");
                modUnload.Invoke(null, new object?[] {teaMod.ModInstance});
            }
        );

        /// <summary>
        ///     When loading: sets <see cref="Mod.Load" /> to false. <br />
        ///     When unloading: N/A.
        /// </summary>
        public static readonly ILoadStep LoadingFalse = new LoadStep(
            nameof(LoadingFalse),
            LoadingFalseWeight,
            teaMod => Reflection<Mod>.InvokeFieldSetter("loading", teaMod, false),
            _ => { }
        );

        /// <summary>
        ///     The default steps used by Tea mods.
        /// </summary>
        public static List<ILoadStep> GetDefaultLoadSteps() {
            return new List<ILoadStep>
            {
                ContentInstanceRegister,
                LoadingTrue,
                //HandleServiceProvider,
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
}