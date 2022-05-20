using System.Linq;
using System;
using System.Collections.Generic;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Logging;
using TeaFramework.API.Features.Patching;
using TeaFramework.Features.CustomLoading;
using TeaFramework.Features.Events;
using TeaFramework.Features.Logging;
using Terraria;
using Terraria.ModLoader;

namespace TeaFramework
{
    /// <summary>
    ///		The <see cref="Mod"/> instance used by tModLoader for Tea Framework. Your mods will typically inherit this.
    /// </summary>
    /// <remarks>
    ///		If you do not want to inherit this class, you can implement functionality directly with <see cref="ITeaMod"/>.
    /// </remarks>
    public class TeaMod : Mod, ITeaMod, IPatchRepository
    {
        public TeaMod()
        {
            // Privately request native access to perform our early bird edits.
            ExecutePrivately(MonoModHooks.RequestNativeAccess);
        }

        #region ITeaMod Impl

        Mod ITeaMod.ModInstance => this;

        public ILogWrapper LogWrapper => new LogWrapper(Logger);

        public IEnumerable<IContentLoader> ContentLoaders { get; } = ITeaMod.GetDefaultContentLoaders();

        public IEventBus EventBus { get; } = new EventBus();

        #endregion

        #region IPatchRepository Impl

        public List<IMonoModPatch> Patches { get; } = new();

        /// <summary>
        ///     Set up the list of steps that should be taken to load your mod.
        /// </summary>
        /// <param name="steps">The <see cref="IList{T}"/> of <see cref="ILoadStep"/>s you should add and modify.</param>
        public virtual void GetLoadSteps(out IList<ILoadStep> steps) => steps = DefaultLoadSteps.GetDefaultSteps();

        #endregion

        #region Mod Hooks

        // Made sealed to facilitate the use of load hooks.
        public sealed override void Load() { }

        public sealed override void Unload()
        {
            base.Unload();

            // Re-create the default unload steps here since they're removed once TeaMod.Unload is ran for the base instance.
            ExecutePrivately(() => {
                Main.QueueMainThreadAction(() => {
                    IEventListener[] listeners = EventBus.Listeners.Values.SelectMany(listeners => listeners)
                        .ToArray();

                    foreach (IEventListener listener in listeners)
                        EventBus.Unsubscribe(listener);

                    foreach (IMonoModPatch patch in Patches)
                        patch.Unapply();
                });
            });
        }

        #endregion

        #region Internal

        /// <summary>
        ///		Executes a tasks only intended to be done by Tea Framework. Used as a workaround for a tModLoader issue (TML-2332).
        /// </summary>
        private bool ExecutePrivately(Action task)
        {
            if (!GetType().Assembly.FullName?.StartsWith("TeaFramework, ") ?? false)
                return false;

            task();
            return true;
        }

        #endregion
    }
}
