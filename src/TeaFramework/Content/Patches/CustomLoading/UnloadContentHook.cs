using System.Collections.Generic;
using System.Reflection;
using TeaFramework.API.CustomLoading;
using TeaFramework.Impl.CustomLoading;
using TeaFramework.Impl.Patching;
using TeaFramework.Impl.Utility;
using Terraria.ModLoader;

namespace TeaFramework.Content.Patches.CustomLoading
{
    public class UnloadContentHook : Patch<UnloadContentHook.UnloadContent>
    {
        public delegate void Orig(Mod self);

        public delegate void UnloadContent(Orig orig, Mod self);

        public override MethodInfo ModifiedMethod { get; } = typeof(Mod).GetCachedMethod("UnloadContent");

        public override UnloadContent PatchMethod { get; } = (orig, self) => {
            if (self is not ITeaMod teaMod)
            {
                orig(self);
                return;
            }
            
            teaMod.GetLoadSteps(out IList<ILoadStep> rawSteps);
            LoadStepCollection collection = new(rawSteps);
            
            foreach (ILoadStep step in collection.GetReversed())
                step.Unload(teaMod);
        };
    }
}
