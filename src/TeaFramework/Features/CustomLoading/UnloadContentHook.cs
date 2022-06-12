using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeaFramework.API;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.Features.Patching;
using TeaFramework.Utilities;
using TeaFramework.Utilities.Extensions;
using Terraria.ModLoader;

namespace TeaFramework.Features.CustomLoading
{
    public class UnloadContentHook : Patch<UnloadContentHook.UnloadContent>
    {
        public delegate void Orig(Mod self);

        public delegate void UnloadContent(Orig orig, Mod self);

        public override MethodInfo ModifiedMethod { get; } = typeof(Mod).GetCachedMethod("UnloadContent");

        protected override UnloadContent PatchMethod { get; } = (orig, self) =>
        {
            if (self is not ITeaMod teaMod) {
                orig(self);
                return;
            }

            ILoadStepsProvider? provider = teaMod.GetService<ILoadStepsProvider>();

            if (provider is null) return;
            
            IEnumerable<ILoadStep>? loadSteps = provider.GetLoadSteps();

            if (loadSteps is null) return;

            LoadStepCollection collection = new(loadSteps.ToList());

            foreach (ILoadStep step in collection.GetReversed()) step.Unload(teaMod);

            teaMod.ClearApiServiceProvider();
        };
    }
}