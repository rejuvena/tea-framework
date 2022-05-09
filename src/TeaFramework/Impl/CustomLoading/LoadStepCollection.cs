﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TeaFramework.API.CustomLoading;

namespace TeaFramework.Impl.CustomLoading
{
    /// <summary>
    ///     Default implementation of <see cref="ILoadStepCollection"/>.
    /// </summary>
    public class LoadStepCollection : ILoadStepCollection
    {
        private readonly IDictionary<string, ILoadStep> _steps;

        public LoadStepCollection(IDictionary<string, ILoadStep>? steps = null)
        {
            _steps = steps ?? new Dictionary<string, ILoadStep>();
        }

        public LoadStepCollection(IList<ILoadStep> steps)
        {
            _steps = steps.ToDictionary(x => x.Name, x => x);
        }

        public void Add(ILoadStep step) => _steps.Add(step.Name, step);

        public ILoadStep Get(string name) => _steps[name];

        public bool TryGet(string name, out ILoadStep? step) => _steps.TryGetValue(name, out step);

        public ILoadStepCollection GetReversed()
        {
            Dictionary<string, ILoadStep> reversed = new();

            foreach ((string? key, ILoadStep? value) in _steps.Reverse()) 
                reversed.Add(key, value);

            return new LoadStepCollection(reversed);
        }

        public IEnumerator<ILoadStep> GetEnumerator() => _steps.Values.OrderBy(x => x.Weight).ThenBy(x => x.Name).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}