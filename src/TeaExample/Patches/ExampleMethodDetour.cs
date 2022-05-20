using System.Reflection;
using Microsoft.Xna.Framework;
using TeaFramework.Features.Patching;
using TeaFramework.Features.Utility;
using Terraria;

namespace TeaExampleMod.Patches
{
    // Method detours are created by passing a custom delegate as the generic parameter of a Patch<T>.
    // For most detours on Terraria methods, you should use delegates from the On.Terraria namespace,
    // but for the sake of showing it off we use a custom delegate here. Note that no delegates exist
    // for the Terraria.ModLoader namespace and thus you must make your own for methods in there.
    public class ExampleMethodDetour : Patch<ExampleMethodDetour.DrawCursor>
    {
        // As this method is not within the Terraria.ModLoader namespace, this delegate is not
        //  necessary and just defines the parameters of the original method.
        // This correlates to the _orig delegates provided by MonoMod's HookGen.
        //
        // If the method this delegate represents is instanced (not static), the first parameter
        // should be the class the method belongs to. The following parameters should mirror the
        // parameters of the original method.
        //
        // Example: public void MyMethod(int a) becomes Orig(MyClass self, int a)
        // Example: public static void OtherMethod(bool b, int c) becomes Orig(bool b, int c)
        public delegate void Orig(Vector2 bonus, bool smart);
        
        // This delegate is the one used for our detouring, and is what is used as the generic parameter
        // of Patch<T>.
        // The first parameter is Orig.
        //
        // The first parameter will always be the delegate that represents the original method.
        // The following parameters will mirror the parameters of the delegate.
        public delegate void DrawCursor(Orig orig, Vector2 bonus, bool smart);

        // Gets the method "Terraria.Main::DrawCursor".
        public override MethodInfo ModifiedMethod { get; } = typeof(Main).GetCachedMethod("DrawCursor");

        // PatchMethod will be a DrawCursor since that is the generic parameter which was passed.
        // We create an anonymous method here that serves as our detour.
        public override DrawCursor PatchMethod { get; } = static (orig, bonus, smart) =>
        {
            // This simple detour just reverses whether the cursor is drawn as a smart cursor.
            orig(bonus, !smart);
        };
    }
}
