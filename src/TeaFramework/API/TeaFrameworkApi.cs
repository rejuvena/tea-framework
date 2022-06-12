using System.Collections.Generic;
using TeaFramework.API.DependencyInjection;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.API.Features.Events;
using TeaFramework.API.Features.Localization;
using TeaFramework.API.Features.Logging;
using TeaFramework.API.Features.ModCall;
using TeaFramework.API.Features.Packets;
using TeaFramework.Features.ContentLoading;
using TeaFramework.Features.CustomLoading;
using TeaFramework.Features.Events;
using TeaFramework.Features.Localization;
using TeaFramework.Features.Logging;
using TeaFramework.Features.ModCall;
using TeaFramework.Features.Packets;

namespace TeaFramework.API
{
    /// <summary>
    ///     The Tea Framework API service.
    /// </summary>
    public class TeaFrameworkApi : IApi
    {
        public string Name => nameof(TeaFrameworkApi);

        public void AddTo(IApiServiceProvider apiServiceProvider) {
            apiServiceProvider.SetService<ILogWrapper>(new LogWrapper(apiServiceProvider.TeaMod.ModInstance));
            apiServiceProvider.SetService<IEventBus>(new EventBus());

            apiServiceProvider.SetService<IContentLoadersProvider>(new ContentLoadersProvider());
            apiServiceProvider.SetService<ILoadStepsProvider>(new LoadStepsProvider());

            ILocalizationLoader localizationLoader = new DefaultLocalizationLoader();
            apiServiceProvider.SetService(localizationLoader);
            localizationLoader.Parsers.Add("lang", new LangFileParser());
            localizationLoader.Parsers.Add("toml", new TomlFileParser());
            
            apiServiceProvider.SetService<IModCallManager>(new ModCallManager());
            apiServiceProvider.SetService<IPacketManager>(new PacketManager(apiServiceProvider.TeaMod));
        }

        public static IList<IContentLoader> GetContentLoaders() {
            return new IContentLoader[] { new EventListenerLoader(), new ModCallHandlerLoader(), new PacketHandlerLoader() };
        }

        public static IList<ILoadStep> GetLoadSteps() {
            return DefaultLoadSteps.GetDefaultLoadSteps();
        }
    }
}