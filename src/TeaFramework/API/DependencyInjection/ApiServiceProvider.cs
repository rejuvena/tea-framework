using System;
using System.Collections.Generic;
using System.Linq;

namespace TeaFramework.API.DependencyInjection
{
    public class ApiServiceProvider : IApiServiceProvider
    {
        protected readonly Dictionary<Type, IApiService> ApiServices = new();
        protected readonly Dictionary<Type, object?> Singletons = new();

        public ApiServiceProvider(ITeaMod teaMod) {
            TeaMod = teaMod;
        }

        public ITeaMod TeaMod { get; }

        public T? GetServiceSingleton<T>() {
            return (T?) (Singletons.ContainsKey(typeof(T)) ? Singletons[typeof(T)] : null);
        }

        public object? GetServiceSingleton(Type type) {
            return Singletons.ContainsKey(type) ? Singletons[type] : null;
        }

        public void SetServiceSingleton<T>(T? singleton) {
            SetServiceSingleton(typeof(T), singleton);
        }

        public void SetServiceSingleton(Type type, object? singleton) {
            if (singleton is null) {
                Singletons.Remove(type);
                return;
            }

            Singletons[type] = singleton;
        }

        public void InstallApi<T>(T apiService)
            where T : IApiService {
            (ApiServices[typeof(T)] = apiService).Install(this);
        }

        public void UninstallApi<T>()
            where T : IApiService {
            IApiService? service = ApiServices.ContainsKey(typeof(T)) ? ApiServices[typeof(T)] : null;

            if (service is null) return;

            service.Uninstall(this);
            ApiServices.Remove(typeof(T));
        }

        public T? GetApiService<T>()
            where T : IApiService {
            return (T?) (ApiServices.ContainsKey(typeof(T)) ? ApiServices[typeof(T)] : null);
        }

        public IApiService? GetApiService(string name) {
            return ApiServices.Values.FirstOrDefault(x => x.Name == name);
        }
    }
}