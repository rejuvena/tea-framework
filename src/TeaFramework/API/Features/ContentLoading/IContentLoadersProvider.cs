using System.Collections.Generic;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.API.Features.ContentLoading
{
    public interface IContentLoadersProvider : IService
    {
        IEnumerable<IContentLoader> GetContentLoaders();
    }
}
