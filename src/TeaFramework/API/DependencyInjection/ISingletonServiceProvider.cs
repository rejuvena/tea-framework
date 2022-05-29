using System;

namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     Simple singleton service provider.
    /// </summary>
    public interface ISingletonServiceProvider
    {
        /// <summary>
        ///     Retrieves a service with a singleton lifetime.
        /// </summary>
        /// <typeparam name="T">The type to register this singleton instance under.</typeparam>
        /// <returns>The service, which may be null.</returns>
        T? GetServiceSingleton<T>();

        /// <summary>
        ///     Retrieves a service with a singleton lifetime.
        /// </summary>
        /// <param name="type">The type to register this singleton instance under.</param>
        /// <returns>The service, which may be null.</returns>
        object? GetServiceSingleton(Type type);

        /// <summary>
        ///     Sets a service with a singleton lifetime.
        /// </summary>
        /// <param name="singleton">The singleton instance to use.</param>
        /// <typeparam name="T">The type to register this singleton instance under.</typeparam>
        void SetServiceSingleton<T>(T? singleton);

        /// <summary>
        ///     Sets a service with a singleton lifetime.
        /// </summary>
        /// <param name="type">The type to register this singleton under.</param>
        /// <param name="singleton">The singleton instance to use.</param>
        void SetServiceSingleton(Type type, object? singleton);
    }
}
