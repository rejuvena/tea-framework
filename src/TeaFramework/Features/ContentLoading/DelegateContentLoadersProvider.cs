using System;
using System.Collections.Generic;
using TeaFramework.API.Features.ContentLoading;

namespace TeaFramework.Features.ContentLoading
{
    public class DelegateContentLoadersProvider : IContentLoadersProvider
    {
        public Func<IEnumerable<IContentLoader>> GetContentLoadersFunc;

        public DelegateContentLoadersProvider(Func<IEnumerable<IContentLoader>> getContentLoadersFunc) {
            GetContentLoadersFunc = getContentLoadersFunc;
        }

        public IEnumerable<IContentLoader> GetContentLoaders() {
            return GetContentLoadersFunc();
        }
    }
}
