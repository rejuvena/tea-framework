using System;
using System.Collections.Generic;
using System.Linq;
using TeaFramework.API;
using TeaFramework.API.DependencyInjection;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Patching;
using TeaFramework.Features.CustomLoading;
using TeaFramework.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace TeaFramework
{
    /// <summary>
    ///     The <see cref="Mod" /> instance used by tModLoader for Tea Framework. Your mods will typically inherit this.
    /// </summary>
    /// <remarks>
    ///     If you do not want to inherit this class, you can implement functionality directly with <see cref="ITeaMod" />.
    /// </remarks>
    public class TeaMod : Mod, ITeaMod, IPatchRepository
    {
        public TeaMod() {
            // Privately request native access to perform our early bird edits.
            ExecutePrivately(MonoModHooks.RequestNativeAccess);
            ServiceProvider = new ApiServiceProvider(this);
            // Our APIs aren't loaded by our hooks, normally.
            ExecutePrivately(InstallApis);
            ExecutePrivately(() =>
            {
                IPatch patch = new LoadModContentEdit();
                patch.Load(this);
                // patch.Apply(this);
            });
        }

        #region IPatchRepository Impl

        public List<IMonoModPatch> Patches { get; } = new();

        #endregion

        #region Internal

        /// <summary>
        ///     Executes a tasks only intended to be done by Tea Framework. Used as a workaround for a tModLoader issue (TML-2332).
        /// </summary>
        private bool ExecutePrivately(Action task) {
            if (!GetType().Assembly.FullName?.StartsWith("TeaFramework, ") ?? false) return false;

            task();
            return true;
        }

        #endregion

        #region ITeaMod Impl

        Mod ITeaMod.ModInstance => this;

        public IApiServiceProvider ServiceProvider { get; }

        public virtual void InstallApis() {
            ServiceProvider.InstallApi<TeaFrameworkApi>();
            ServiceProvider.SetServiceSingleton<TeaFrameworkApi.ContentLoadersProvider>(GetContentLoaders);
            ServiceProvider.SetServiceSingleton<TeaFrameworkApi.LoadStepsProvider>(GetLoadSteps);
        }

        public virtual void UninstallApis() {
            ServiceProvider.UninstallApi<TeaFrameworkApi>();
        }

        protected virtual void GetContentLoaders(out IEnumerable<IContentLoader> loaders) {
            loaders = TeaFrameworkApi.GetContentLoaders();
        }

        protected virtual void GetLoadSteps(out IList<ILoadStep> steps) {
            steps = TeaFrameworkApi.GetLoadSteps();
        }

        #endregion

        #region Mod Hooks

        // Make Load and Unload sealed to facilitate the use of load hooks.
        public sealed override void Load() {
            // Even though this shouldn't matter much, better safe than sorry.
            // ExecutePrivately(InstallApis);

            if (!ExecutePrivately(() =>
                {
                    Logger.Info($"tModLoader loaded with Tea Framework v{Version}"
                                + "\nGet support on Discord @ discord.gg/tomat"
                                + "\nReport issues on GitHub @ https://github.com/rejuvena/tea-framework"
                    );
                }))
                Logger.Info($"Loaded mod \"{DisplayName}\" (\"{Name}\") under Tea Framework."
                            + "\nTo anyone attempting to offer support: be aware of external errors and other Tea Framework nuances.");
        }

        public sealed override void Unload() {
            base.Unload();

            // Re-create the default unload steps here since they're removed once TeaMod.Unload is ran for the base instance.
            ExecutePrivately(() =>
            {
                Main.QueueMainThreadAction(() =>
                {
                    IEventBus? bus = this.GetService<IEventBus>();

                    if (bus is not null) {
                        Dictionary<Type, List<IEventListener>>.ValueCollection listenerValues = bus.Listeners.Values;
                        IEventListener[] listeners = listenerValues.SelectMany(listeners => listeners).ToArray();

                        foreach (IEventListener listener in listeners) bus.Unsubscribe(listener);
                    }

                    foreach (IMonoModPatch patch in Patches) patch.Unapply();

                    UninstallApis();
                });
            });
        }

        #endregion
    }
}