using log4net;
using TeaFramework.API.Features.Logging;
using Terraria.ModLoader;

namespace TeaFramework.Features.Logging
{
    public readonly struct LogWrapper : ILogWrapper
    {
        private Mod Mod { get; }

        public ILog Logger => Mod.Logger;

        public LogWrapper(Mod mod) {
            Mod = mod;
        }

        public string LogPatchFailure(string type, string message) {
            message = $"PATCH FAILURE {type} @ " + message;
            Logger.Error(message);
            return message;
        }

        public string LogOpCodeJumpFailure(string typeName, string typeMethod, string opcode, string? value) {
            return LogPatchFailure(
                "OpCode Jump Failure",
                $"{typeName}::{typeMethod} -> {opcode}{(value is not null ? $" {value}" : "")}"
            );
        }
    }
}