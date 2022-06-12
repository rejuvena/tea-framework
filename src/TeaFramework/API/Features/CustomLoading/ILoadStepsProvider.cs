using System.Collections.Generic;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.API.Features.CustomLoading
{
    public interface ILoadStepsProvider : IService
    {
        IEnumerable<ILoadStep> GetLoadSteps();
    }
}
