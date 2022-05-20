namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     A singleton service provider built around the installation of API services.
    /// </summary>
    public interface IApiServiceProvider : ISingletonServiceProvider
    {
        void InstallApi<T>(T apiService) where T : IApiService;

        void UninstallApi<T>() where T : IApiService;

        T? GetApiService<T>() where T : IApiService;
        
        IApiService? GetApiService(string name);
    }
}
