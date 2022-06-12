namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     An easy way to add a set of default service implementations.
    /// </summary>
    public interface IApi
    {
        /// <summary>
        ///     This APIs unique name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        ///     Adds all default <see cref="IService"/> implementations.
        /// </summary>
        /// <param name="apiServiceProvider">The API service provider.</param>
        void AddTo(IApiServiceProvider apiServiceProvider);
    }
}
