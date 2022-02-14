using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace TeaFramework.Common.Utilities.Extensions
{
    /// <summary>
    ///     Numerous methods to extend Mono.Cecil and MonoMod functionality.
    /// </summary>
    public static class IntermediateLanguageExtensions
    {
        public static int AddVariable<TType>(this ILContext context) => context.AddVariable(typeof(TType));

        public static int AddVariable(this ILContext context, Type type)
        {
            context.Body.Variables.Add(new VariableDefinition(context.Import(type)));
            return context.Body.Variables.Count - 1;
        }

        public static int AddVariable<TType>(this ILCursor cursor) => cursor.Context.AddVariable<TType>();

        public static int AddVariable(this ILCursor cursor, Type type) => cursor.Context.AddVariable(type);
    }
}