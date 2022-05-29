using System.Collections.Generic;
using TeaFramework.API.DependencyInjection;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Logging;
using TeaFramework.Features.CustomLoading;
using TeaFramework.Features.Events;
using TeaFramework.Features.Logging;

namespace TeaFramework.API
{
    /// <summary>
    ///     The Tea Framework API service.
    /// </summary>
    public class TeaFrameworkApi : IApiService
    {
        public delegate void ContentLoadersProvider(out IEnumerable<IContentLoader> loaders);

        public delegate void LoadStepsProvider(out IList<ILoadStep> steps);

        public string Name => nameof(TeaFrameworkApi);

        public void Install(IApiServiceProvider apiServiceProvider) {
            apiServiceProvider.SetServiceSingleton<ILogWrapper>(new LogWrapper(apiServiceProvider.TeaMod.ModInstance));
            apiServiceProvider.SetServiceSingleton<IEventBus>(new EventBus());
            apiServiceProvider.SetServiceSingleton<ContentLoadersProvider>(
                (out IEnumerable<IContentLoader> loaders) => loaders = GetContentLoaders()
            );
            apiServiceProvider.SetServiceSingleton<LoadStepsProvider>(
                (out IList<ILoadStep> steps) => steps = GetLoadSteps()
            );
        }

        public void Uninstall(IApiServiceProvider apiServiceProvider) {
            apiServiceProvider.SetServiceSingleton<ILogWrapper>(null);
            apiServiceProvider.SetServiceSingleton<IEventBus>(null);
            apiServiceProvider.SetServiceSingleton<ContentLoadersProvider>(null);
            apiServiceProvider.SetServiceSingleton<LoadStepsProvider>(null);
        }

        /// <summary>
        ///     Returns a collection of default content loaders.
        /// </summary>
        public static IEnumerable<IContentLoader> GetContentLoaders() {
            return new IContentLoader[] {new EventListenerLoader()};
        }

        /// <summary>
        ///     Returns a collection of default load steps.
        /// </summary>
        /// <returns></returns>
        public static IList<ILoadStep> GetLoadSteps() {
            return DefaultLoadSteps.GetDefaultLoadSteps();
        }
    }
}