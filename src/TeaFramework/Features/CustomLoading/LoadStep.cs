using System;
using TeaFramework.API.Features.CustomLoading;

namespace TeaFramework.Features.CustomLoading
{
    /// <summary>
    ///     Default implementation of <see cref="ILoadStep" />.
    /// </summary>
    public class LoadStep : ILoadStep
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        private readonly Action<ITeaMod> _load;
        private readonly Action<ITeaMod> _unload;

        public LoadStep(string name, float weight, Action<ITeaMod> load, Action<ITeaMod> unload)
        {
            Name = name;
            Weight = weight;
            _load = load;
            _unload = unload;
        }

        public void Load(ITeaMod teaMod) => _load(teaMod);
        public void Unload(ITeaMod teaMod) => _unload(teaMod);
    }
}
