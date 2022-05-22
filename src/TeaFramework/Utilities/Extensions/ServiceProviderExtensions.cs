using TeaFramework.API;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.Utilities.Extensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        ///     Directly installs an API from the generic type, assuming a parameterless constructor.
        /// </summary>
        public static void InstallApi<T>(this IApiServiceProvider provider) where T : IApiService, new() =>
            provider.InstallApi(new T());

        /// <summary>
        ///     Super-short shorthand for calling <see cref="ISingletonServiceProvider.GetServiceSingleton{T}"/>.
        /// </summary>
        public static T? GetService<T>(this ITeaMod teaMod) => teaMod.ServiceProvider.GetServiceSingleton<T>();
    }
}
