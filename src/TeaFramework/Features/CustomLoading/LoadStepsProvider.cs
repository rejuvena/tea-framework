using System.Collections.Generic;
using TeaFramework.API.Features.CustomLoading;

namespace TeaFramework.Features.CustomLoading
{
    public class LoadStepsProvider : ILoadStepsProvider
    {
        public IEnumerable<ILoadStep> GetLoadSteps() {
            return DefaultLoadSteps.GetDefaultLoadSteps();
        }
    }
}
