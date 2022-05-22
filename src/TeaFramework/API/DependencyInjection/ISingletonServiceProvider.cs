using System;

namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     Simple singleton service provider.
    /// </summary>
    public interface ISingletonServiceProvider
    {
        T? GetServiceSingleton<T>();

        object? GetServiceSingleton(Type type);

        void SetServiceSingleton<T>(T? singleton);

        void SetServiceSingleton(Type type, object? singleton);
    }
}
