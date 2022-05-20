using System;

namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     Simple singleton service provider.
    /// </summary>
    public interface ISingletonServiceProvider
    {
        T? GetSingletonService<T>();

        object? GetSingletonService(Type type);

        void SetSingletonService<T>(T? singleton);

        void SetSingletonService(Type type, object? singleton);
    }
}
