using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace TeaFramework.API.Patching
{
    public class ILMixin
    {
        public readonly ILCursor Cursor;

        public ILMixin(ILCursor cursor)
        {
            Cursor = cursor;
        }

        public virtual void ReplaceCalls<TType, TDelegate>(string name, TDelegate @delegate) where TDelegate : Delegate
        {
            while (Cursor.TryGotoNext(MoveType.Before, x => x.MatchCall<TType>(name))) 
                ReplaceCall(@delegate);
        }

        public virtual void ReplaceCalls<TDelegate>(string typeFullName, string name, TDelegate @delegate) where TDelegate : Delegate
        {
            while (Cursor.TryGotoNext(MoveType.Before, x => x.MatchCall(typeFullName, name))) 
                ReplaceCall(@delegate);
        }

        public virtual void ReplaceCallvirts<TType, TDelegate>(string name, TDelegate @delegate) where TDelegate : Delegate
        {
            while (Cursor.TryGotoNext(MoveType.Before, x => x.MatchCall<TType>(name))) 
                ReplaceCall(@delegate);
        }

        public virtual void ReplaceCallvirts<TDelegate>(string typeFullName, string name, TDelegate @delegate) where TDelegate : Delegate
        {
            while (Cursor.TryGotoNext(MoveType.Before, x => x.MatchCall(typeFullName, name))) 
                ReplaceCall(@delegate);
        }

        public virtual void ReplaceCallsOrCallvirts<TType, TDelegate>(string name, TDelegate @delegate) where TDelegate : Delegate
        {
            while (Cursor.TryGotoNext(MoveType.Before, x => x.MatchCall<TType>(name))) 
                ReplaceCall(@delegate);
        }

        public virtual void ReplaceCallsOrCallvirts<TDelegate>(string typeFullName, string name, TDelegate @delegate) where TDelegate : Delegate
        {
            while (Cursor.TryGotoNext(MoveType.Before, x => x.MatchCall(typeFullName, name))) 
                ReplaceCall(@delegate);
        }

        protected virtual void ReplaceCall<TDelegate>(TDelegate @delegate) where TDelegate : Delegate
        {
            // Remove original call(virt) from stack.
            Cursor.Emit(OpCodes.Pop);

            // Push new delegate.
            Cursor.EmitDelegate(@delegate);
        }
    }
}
