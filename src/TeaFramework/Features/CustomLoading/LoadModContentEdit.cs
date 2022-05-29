using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.Cil;
using TeaFramework.API;
using TeaFramework.API.Exceptions;
using TeaFramework.API.Features.CustomLoading;
using TeaFramework.Features.Patching;
using TeaFramework.Features.Utility;
using TeaFramework.Utilities.CWT.Data;
using TeaFramework.Utilities.Extensions;
using Terraria.ModLoader;

namespace TeaFramework.Features.CustomLoading
{
    /// <summary>
    ///     IL edit done to adjust how mod content loading works.
    /// </summary>
    /// <remarks>
    ///     This patch is not autoloaded, and handled using special logic.
    /// </remarks>
    [Autoload(false)] public class LoadModContentEdit : Patch<ILContext.Manipulator>
    {
        private static readonly MethodInfo _methodToMatch = typeof(Action<Mod>).GetCachedMethod("Invoke");

        public override MethodInfo ModifiedMethod { get; } = typeof(ModContent).GetCachedMethod("LoadModContent");

        protected override ILContext.Manipulator PatchMethod =>
            il =>
            {
                ILCursor c = new(il);

                if (!c.TryGotoNext(MoveType.Before, x => x.MatchCallvirt(_methodToMatch)))
                    throw new TeaModLoadException(
                        this.LogOpCodeJumpFailure(
                            "Terraria.ModLoader.ModContent",
                            "LoadModContent",
                            "callvirt",
                            "instance void class [System.Runtime]System.Action`1<class Terraria.ModLoader.Mod>::Invoke(!0)"
                        )
                    );

                c.Remove();
                c.EmitDelegate<Action<Action<Mod>, Mod>>((action, mod) =>
                {
                    // Only call this part during the first time LoadModContent is called
                    // bool? isLoading = (bool?)typeof(Mod).GetCachedField("loading").GetValue(mod);
                    CwtModData modData = mod.GetDynamicField<Mod, CwtModData>("modData");

                    if (modData.HasDoneLoadingCycle) {
                        action(mod);
                        return;
                    }

                    modData.HasDoneLoadingCycle = true;

                    if (mod is ITeaMod teaMod) {
                        teaMod.InstallApis();

                        IList<ILoadStep>? loadSteps = null;
                        teaMod.GetService<TeaFrameworkApi.LoadStepsProvider>()?.Invoke(out loadSteps);

                        if (loadSteps is null) return;

                        LoadStepCollection collection = new(loadSteps);

                        foreach (ILoadStep step in collection) step.Load(teaMod);
                    }
                    else {
                        action(mod);
                    }
                });
            };
    }
}