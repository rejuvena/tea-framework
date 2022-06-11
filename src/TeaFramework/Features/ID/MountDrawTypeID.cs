using TeaFramework.API.Features.ID;

namespace TeaFramework.Features.ID
{
    public class MountDrawTypeID : IIdBase<MountDrawTypeID, int>
    {
        public const int BackTexture = 0;
        public const int BackTextureExtra = 1;
        public const int FrontTexture = 2;
        public const int FrontTextureExtra = 3;
    }
}