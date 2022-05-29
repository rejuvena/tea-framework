namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     A singleton service provider built around the installation of API services.
    /// </summary>
    public interface IApiServiceProvider : ISingletonServiceProvider
    {
        /// <summary>
        ///     The mod that this API service provider belongs to.
        /// </summary>
        ITeaMod TeaMod { get; }

        /// <summary>
        ///     Installs an API.
        /// </summary>
        /// <param name="apiService">The instance to install.</param>
        /// <typeparam name="T">The lowest API service type you wish to use.</typeparam>
        void InstallApi<T>(T apiService)
            where T : IApiService;

        /// <summary>
        ///     Uninstalls an API.
        /// </summary>
        /// <typeparam name="T">The aforementioned lowest API service specified when installing an API.</typeparam>
        void UninstallApi<T>()
            where T : IApiService;

        /// <summary>
        ///     Gets an API service by type.
        /// </summary>
        /// <typeparam name="T">The aforementioned lowest API service specified when installing an API.</typeparam>
        /// <returns>The API service, which is null if not present.</returns>
        T? GetApiService<T>()
            where T : IApiService;

        /// <summary>
        ///     Gets an API service through dependency-less means.
        /// </summary>
        /// <param name="name">The API service's name.</param>
        /// <returns>The API service, which is null if not present.</returns>
        IApiService? GetApiService(string name);
    }
}
