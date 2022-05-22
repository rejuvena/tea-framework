using System.Collections.Generic;
using System.Reflection;
using TeaFramework.API;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.Features.Patching;
using TeaFramework.Features.Utility;
using TeaFramework.Utilities.Extensions;
using Terraria.ModLoader;

namespace TeaFramework.Features.CustomLoading
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
            
            IList<ILoadStep>? loadSteps = null;
            teaMod.GetService<TeaFrameworkApi.LoadStepsProvider>()?.Invoke(out loadSteps);

            if (loadSteps is null)
                return;
            
            LoadStepCollection collection = new(loadSteps);
            
            foreach (ILoadStep step in collection.GetReversed())
                step.Unload(teaMod);
        };
    }
}
