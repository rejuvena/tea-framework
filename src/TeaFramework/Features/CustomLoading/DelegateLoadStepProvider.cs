using System;
using System.Collections.Generic;
using TeaFramework.API.Features.CustomLoading;

namespace TeaFramework.Features.CustomLoading
{
    public class DelegateLoadStepsProvider : ILoadStepsProvider
    {
        public Func<IEnumerable<ILoadStep>> GetLoadStepsFunc;

        public DelegateLoadStepsProvider(Func<IEnumerable<ILoadStep>> getLoadSteps) {
            GetLoadStepsFunc = getLoadSteps;
        }

        public IEnumerable<ILoadStep> GetLoadSteps() {
            return GetLoadStepsFunc();
        }
    }
}
