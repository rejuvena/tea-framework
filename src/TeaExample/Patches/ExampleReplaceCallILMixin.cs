using System;
using System.Reflection;
using MonoMod.Cil;
using TeaFramework.API.Utility;
using TeaFramework.Features.Patching;
using TeaFramework.Features.Utility;
using Terraria;
using Terraria.DataStructures;

namespace TeaExampleMod.Patches
{
    public class ExampleReplaceCallILMixin : Patch<ILContext.Manipulator>
    {
        public override MethodInfo ModifiedMethod { get; } = typeof(TitleLinkButton).GetCachedMethod("DrawTooltip");

        protected override ILContext.Manipulator PatchMethod { get; } = il =>
        {
            ILCursor c = new(il);
            ILMixin m = new(c);

            m.ReplaceCallvirts<Item, Action<Item, string>>(
                "SetNameOverride",
                (item, s) => item.SetNameOverride("Hello from Tea Example Mod: " + s)
            );
        };
    }
}