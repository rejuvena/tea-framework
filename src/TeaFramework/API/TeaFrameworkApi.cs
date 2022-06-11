using System.Collections.Generic;
using TeaFramework.API.DependencyInjection;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Localization;
using TeaFramework.API.Features.Logging;
using TeaFramework.API.Features.ModCall;
using TeaFramework.Features.CustomLoading;
using TeaFramework.Features.Events;
using TeaFramework.Features.Localization;
using TeaFramework.Features.Logging;
using TeaFramework.Features.ModCall;

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
            ILocalizationLoader localizationLoader = new DefaultLocalizationLoader();
            apiServiceProvider.SetServiceSingleton(localizationLoader);
            localizationLoader.Parsers.Add("lang", new LangFileParser());
            localizationLoader.Parsers.Add("toml", new TomlFileParser());
            apiServiceProvider.SetServiceSingleton<IModCallManager>(new ModCallManager());
        }

        public void Uninstall(IApiServiceProvider apiServiceProvider) {
            apiServiceProvider.SetServiceSingleton<ILogWrapper>(null);
            apiServiceProvider.SetServiceSingleton<IEventBus>(null);
            apiServiceProvider.SetServiceSingleton<ContentLoadersProvider>(null);
            apiServiceProvider.SetServiceSingleton<LoadStepsProvider>(null);
            apiServiceProvider.SetServiceSingleton<ILocalizationLoader>(null);
            apiServiceProvider.SetServiceSingleton<IModCallManager>(null);
        }

        /// <summary>
        ///     Returns a collection of default content loaders.
        /// </summary>
        public static IEnumerable<IContentLoader> GetContentLoaders() {
            return new IContentLoader[] {new EventListenerLoader(), new ModCallHandlerLoader()};
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