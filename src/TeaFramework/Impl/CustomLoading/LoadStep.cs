using System;
using TeaFramework.API.CustomLoading;

namespace TeaFramework.Impl.CustomLoading
{
    /// <summary>
    ///     Default implementation of <see cref="ILoadStep" />.
    /// </summary>
    public class LoadStep : ILoadStep
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        private readonly Action<ITeaMod> _action;

        public LoadStep(string name, float weight, Action<ITeaMod> action)
        {
            Name = name;
            Weight = weight;
            _action = action;
        }

        public void Load(ITeaMod teaMod) => _action(teaMod);
    }
}
