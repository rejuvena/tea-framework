namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     An API service.
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        ///     This API service's unique name.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        ///     Installs this service to an API service provider.
        /// </summary>
        /// <param name="apiServiceProvider">The API service provider.</param>
        void Install(IApiServiceProvider apiServiceProvider);

        /// <summary>
        ///     Uninstalls this service from an API service provider.
        /// </summary>
        /// <param name="apiServiceProvider">The API service provider.</param>
        void Uninstall(IApiServiceProvider apiServiceProvider);
    }
}
