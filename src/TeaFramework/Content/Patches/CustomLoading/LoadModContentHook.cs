using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.Cil;
using TeaFramework.API.CustomLoading;
using TeaFramework.Impl.CustomLoading;
using TeaFramework.Impl.Patching;
using TeaFramework.Impl.Utility;
using Terraria.ModLoader;

namespace TeaFramework.Content.Patches.CustomLoading
{
    internal class LoadModContentHook : Patch<ILContext.Manipulator>
    {
        public override MethodInfo ModifiedMethod { get; } = typeof(ModContent).GetCachedMethod("LoadModContent");

        private static readonly MethodInfo _methodToMatch = typeof(Action<Mod>).GetCachedMethod("Invoke");
        public override ILContext.Manipulator PatchMethod { get; } = il => {
            ILCursor c = new(il);

            if (!c.TryGotoNext(MoveType.Before, x => x.MatchCallvirt(_methodToMatch)))
                throw new Exception();

            c.Remove();
            c.EmitDelegate<Action<Action<Mod>, Mod>>((action, mod) => {
                // Only call this part during the first time LoadModContent is called
                bool? isLoading = (bool?)typeof(Mod).GetCachedField("loading").GetValue(mod);
                if (isLoading.HasValue && !isLoading.Value)
                    return;
                
                if (mod is ITeaMod teaMod)
                {
                    teaMod.GetLoadSteps(out IList<ILoadStep> rawSteps);
                    LoadStepCollection collection = new(rawSteps);

                    foreach (ILoadStep step in collection)
                        step.Load(teaMod);
                }
                else
                    action(mod);
            });
        };
    }
}
