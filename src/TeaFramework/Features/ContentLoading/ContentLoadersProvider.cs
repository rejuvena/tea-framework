using System.Collections.Generic;
using TeaFramework.API.Features.ContentLoading;
using TeaFramework.Features.Events;
using TeaFramework.Features.ModCall;
using TeaFramework.Features.Packets;

namespace TeaFramework.Features.ContentLoading
{
    public class ContentLoadersProvider : IContentLoadersProvider
    {
        public IEnumerable<IContentLoader> GetContentLoaders() {
            return new IContentLoader[] { new EventListenerLoader(), new ModCallHandlerLoader(), new PacketHandlerLoader() };
        }
    }
}
