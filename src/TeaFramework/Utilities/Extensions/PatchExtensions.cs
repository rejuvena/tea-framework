using TeaFramework.API;
using TeaFramework.API.Features.Logging;
using TeaFramework.API.Features.Patching;
using TeaFramework.Features.Logging;
using Terraria.ModLoader;

namespace TeaFramework.Utilities.Extensions
{
    public static class PatchExtensions
    {
        public static string LogOpCodeJumpFailure(
            this IPatch patch,
            string typeName,
            string typeMethod,
            string opcode,
            string? value
        )
        {
            ILogWrapper? logWrapper = (patch.Mod as ITeaMod)?.GetService<ILogWrapper>();
            logWrapper ??= new LogWrapper(patch.Mod);

            return logWrapper.LogOpCodeJumpFailure(typeName, typeMethod, opcode, value);
        }
    }
}
