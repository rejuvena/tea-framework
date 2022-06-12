using System;

namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     A collection of overridable <see cref="IService"/>s.
    /// </summary>
    public interface IApiServiceProvider
    {
        /// <summary>
        ///     The mod that this API/Service provider belongs to.
        /// </summary>
        ITeaMod TeaMod { get; }

        /// <summary>
        ///     Sets a service by type.
        /// </summary>
        /// <typeparam name="T">The service type you wish to set.</typeparam>
        /// <param name="service">The service instance to use, or null to remove it.</param>
        void SetService<T>(T? service)
            where T : IService;

        /// <summary>
        ///     Sets a service by type.
        /// </summary>
        /// <param name="type">The type of the service you wish to set.</param>
        /// <param name="service">The service instance to use, or null to remove it.</param>
        void SetService(Type type, IService? service);

        /// <summary>
        ///     Gets a service by type.
        /// </summary>
        /// <typeparam name="T">The type of the service you wish to get.</typeparam>
        /// <param name="name">The API service's name.</param>
        /// <returns>The service, which is null if not present.</returns>
        T? GetService<T>()
            where T : IService;

        /// <summary>
        /// Gets a service by type.
        /// </summary>
        /// <param name="type">The type of the service you wish to get</param>
        /// <returns>The service, which is null if not present</returns>
        IService? GetService(Type type);

        /// <summary>
        ///     Adds an <see cref="IApi"/>s default <see cref="IService"/>s.
        /// </summary>
        /// <param name="api">The instance to add.</param>
        /// <typeparam name="T">The API type you wish to use.</typeparam>
        void AddApi<T>(T api)
            where T : IApi;

        /// <summary>
        ///     Gets an API by type.
        /// </summary>
        /// <typeparam name="T">The aforementioned lowest API service specified when adding an API.</typeparam>
        /// <returns>The API, which is null if not present.</returns>
        T? GetApi<T>()
            where T : IApi;

        /// <summary>
        ///     Gets an API through dependency-less means.
        /// </summary>
        /// <param name="name">The APIs name.</param>
        /// <returns>The API, which is null if not present.</returns>
        IApi? GetApi(string name);

        /// <summary>
        ///     Removes all services from this <see cref="IApiServiceProvider"/>.
        /// </summary>
        void RemoveAll();
    }
}
