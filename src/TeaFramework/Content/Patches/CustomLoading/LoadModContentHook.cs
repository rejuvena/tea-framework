using System;
using System.Reflection;
using MonoMod.Cil;
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
                if (mod is ITeaMod)
                {

                }
                else
                    action(mod);
            });
        };
    }
}
