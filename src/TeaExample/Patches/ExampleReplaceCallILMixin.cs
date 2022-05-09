﻿using System;
using System.Reflection;
using MonoMod.Cil;
using TeaFramework.API.Patching;
using TeaFramework.Impl.Patching;
using TeaFramework.Impl.Utility;
using Terraria;
using Terraria.DataStructures;

namespace TeaExampleMod.Patches
{
    public class ExampleReplaceCallILMixin : Patch<ILContext.Manipulator>
    {
        public override MethodInfo ModifiedMethod { get; } = typeof(TitleLinkButton).GetCachedMethod("DrawTooltip");

        public override ILContext.Manipulator PatchMethod { get; } = il => {
            ILCursor c = new(il);
            ILMixin m = new(c);

            m.ReplaceCallvirts<Item, Action<Item, string>>(
                "SetNameOverride",
                (item, s) => item.SetNameOverride("Hello from Tea Framework: " + s)
            );
        };
    }
}