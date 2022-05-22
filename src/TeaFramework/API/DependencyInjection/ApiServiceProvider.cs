using System;
using System.Collections.Generic;
using System.Linq;

namespace TeaFramework.API.DependencyInjection
{
    public class ApiServiceProvider : IApiServiceProvider
    {
        protected readonly Dictionary<Type, object?> Singletons = new();
        protected readonly Dictionary<Type, IApiService> ApiServices = new();

        public ITeaMod TeaMod { get; }
        
        public ApiServiceProvider(ITeaMod teaMod)
        {
            TeaMod = teaMod;
        }

        public T? GetServiceSingleton<T>() => (T?)(Singletons.ContainsKey(typeof(T)) ? Singletons[typeof(T)] : null);

        public object? GetServiceSingleton(Type type) => Singletons.ContainsKey(type) ? Singletons[type] : null;

        public void SetServiceSingleton<T>(T? singleton) => SetServiceSingleton(typeof(T), singleton);

        public void SetServiceSingleton(Type type, object? singleton)
        {
            if (singleton is null)
            {
                Singletons.Remove(type);
                return;
            }
            
            Singletons[type] = singleton;
        }

        public void InstallApi<T>(T apiService) where T : IApiService =>
            (ApiServices[typeof(T)] = apiService).Install(this);

        public void UninstallApi<T>() where T : IApiService
        {
            IApiService? service = ApiServices.ContainsKey(typeof(T)) ? ApiServices[typeof(T)] : null;

            if (service is null)
                return;
            
            service.Uninstall(this);
            ApiServices.Remove(typeof(T));
        }

        public T? GetApiService<T>() where T : IApiService =>
            (T?)(ApiServices.ContainsKey(typeof(T)) ? ApiServices[typeof(T)] : null);

        public IApiService? GetApiService(string name) => ApiServices.Values.FirstOrDefault(x => x.Name == name);
    }
}
