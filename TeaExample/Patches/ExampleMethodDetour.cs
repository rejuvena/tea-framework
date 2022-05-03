using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using TeaFramework.Patching;
using TeaFramework.Utility;
using Terraria;

namespace TeaExample.Patches
{
    // Method detours are created by passing a custom delegate as the generic parameter of a Patch<T>.
    public class ExampleMethodDetour : Patch<ExampleMethodDetour.DrawCursor>
    {
        // This delegate is not necessary, and ust defines the parameters of the original method.
        // This correlates to the _orig delegates provided by MonoMod's HookGen.
        public delegate void Orig(Vector2 bonus, bool smart);
        
        // This detour is the one used for our detouring, and is what is used as the generic parameter of Patch<T>.
        // The first parameter is Orig. If this detours an instanced method, the second parameter should be the original class.
        public delegate void DrawCursor(Orig orig, Vector2 bonus, bool smart);

        // Gets the method "Terraria.Main::DrawCursor".
        public override MethodInfo ModifiedMethod { get; } = typeof(Main).GetCachedMethod("DrawCursor");

        // PatchMethod will be a DrawCursor since that is the generic parameter which was passed.
        // We create an anonymous method here that serves as our detour.
        public override DrawCursor PatchMethod { get; } = static (orig, bonus, smart) =>
        {
            orig(bonus, !smart);
        };
    }
}