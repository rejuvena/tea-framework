using System.Collections.Generic;
using TeaFramework.API.Features.ModCall;
using Terraria.ModLoader;

namespace TeaFramework.Features.ModCall
{
    public abstract class ModCallHandler : IModCallHandler
    {
        protected virtual Mod? Mod { get; set; }

        public abstract IEnumerable<string> HandleableMessages { get; }

        public abstract bool ValidateArgs(List<object> parsedArgs, out IModCallManager.ArgParseFailureType failureType);

        public abstract object? Call(string message, List<object> args);

        public virtual void Load(Mod mod) {
            Mod = mod;
        }

        public virtual void Unload() { }
    }
}