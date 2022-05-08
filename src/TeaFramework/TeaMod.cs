using System.Linq;
using System;
using System.Collections.Generic;
using TeaFramework.API.ContentLoading;
using TeaFramework.API.Events;
using TeaFramework.API.Logging;
using TeaFramework.API.Patching;
using TeaFramework.Impl.Events;
using TeaFramework.Impl.Logging;
using TeaFramework.API.CustomLoading;
using TeaFramework.Impl.CustomLoading;
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
            MonoModHooks.RequestNativeAccess();
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
        public virtual void GetLoadSteps(out IList<ILoadStep> steps)
        {
            steps = new List<ILoadStep>();

            steps.Add(DefaultLoadSteps.LoadMonoModHooks);
        }

        #endregion

        #region Mod Hooks

        // Made sealed to facilitate the use of load hooks.
        public sealed override void Load() { }

        public override void Unload()
        {
            base.Unload();

            foreach (IEventListener listener in EventBus.Listeners.Values.SelectMany(listeners => listeners))
                EventBus.Unsubscribe(listener);

            foreach (IMonoModPatch patch in Patches)
                patch.Unapply();
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
