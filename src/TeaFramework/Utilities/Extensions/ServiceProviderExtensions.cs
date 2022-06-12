using TeaFramework.API;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.Utilities.Extensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        ///     Directly adds an API from the generic type, assuming a parameterless constructor.
        /// </summary>
        public static void AddApi<T>(this IApiServiceProvider provider)
            where T : IApi, new() {
            provider.AddApi(new T());
        }

        /// <summary>
        ///     Super-short shorthand for calling <see cref="ISingletonServiceProvider.GetServiceSingleton{T}" />.
        /// </summary>
        public static T? GetService<T>(this ITeaMod teaMod)
            where T : IService {
            return teaMod.ServiceProvider.GetService<T>();
        }
    }
}