using System;
using System.Reflection;
using MonoMod.Cil;
using TeaExampleMod.Events;
using TeaFramework.API.Events;
using TeaFramework.Impl.Patching;
using TeaFramework.Impl.Utility;
using Terraria;

namespace TeaExampleMod.Patches
{
    // This is an example of an IL edit using the Tea Framework patch system.
    // We use ILContext.Manipulator as the generic parameter of Patch<T> for IL edits.
    public class ExampleILEdit : Patch<ILContext.Manipulator>
    {
        // Resolve the method we're editing, which is "Terraria.Main::DrawVersionNumber".
        public override MethodInfo ModifiedMethod { get; } = typeof(Main).GetCachedMethod("DrawVersionNumber");

        // Create a delegate for applying our edit.
        public override ILContext.Manipulator PatchMethod { get; } = il => {
            ILCursor c = new(il);

            c.GotoNext(MoveType.After, x => x.MatchLdsfld<Main>("versionNumber"));
            c.EmitDelegate<Func<string, string>>(vers => {
                VersionDrawEvent @event = new() {VersionText = vers};
                TeaEventDispatcher.DispatchEvent<VersionDrawEvent>(@event);
                return @event.VersionText;
            });
        };
    }
}
