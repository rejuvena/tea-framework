using System;
using System.Collections.Generic;
using System.Linq;
using TeaFramework.API;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.Features
{
    public class ApiServiceProvider : IApiServiceProvider
    {
        protected readonly Dictionary<Type, IApi> Apis = new();
        protected readonly Dictionary<Type, IService> Services = new();

        public ApiServiceProvider(ITeaMod teaMod) {
            TeaMod = teaMod;
        }

        public ITeaMod TeaMod { get; }

        public T? GetService<T>() where T : IService {
            return (T?) (Services.ContainsKey(typeof(T)) ? Services[typeof(T)] : null);
        }

        public IService? GetService(Type type) {
            return Services.ContainsKey(type) ? Services[type] : null;
        }

        public void SetService<T>(T? service)
            where T : IService {
            SetService(typeof(T), service);
        }

        public void SetService(Type type, IService? service) {
            if (service is null) {
                Services.Remove(type);
                return;
            }

            Services[type] = service;
        }

        public void AddApi<T>(T api)
            where T : IApi {
            (Apis[typeof(T)] = api).AddTo(this);
        }

        public T? GetApi<T>()
            where T : IApi {
            return (T?) (Apis.ContainsKey(typeof(T)) ? Apis[typeof(T)] : null);
        }

        public IApi? GetApi(string name) {
            return Apis.Values.FirstOrDefault(x => x.Name == name);
        }

        public void RemoveAll() {
            foreach (IService service in Services.Values)
            {
                service.OnRemoved();
            }

            Apis.Clear();
            Services.Clear();
        }
    }
}