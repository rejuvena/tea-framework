using log4net;
using TeaFramework.API.Logging;

namespace TeaFramework.Impl.Logging
{
    public readonly struct LogWrapper : ILogWrapper
    {
        public ILog Logger { get; }

        public LogWrapper(ILog logger)
        {
            Logger = logger;
        }

        public string LogPatchFailure(string type, string message)
        {
            message = $"PATCH FAILURE {type} @ " + message;
            Logger.Error(message);
            return message;
        }

        public string LogOpCodeJumpFailure(string typeName, string typeMethod, string opcode, string? value) =>
            LogPatchFailure(
                "OpCode Jump Failure",
                $"{typeName}::{typeMethod} -> {opcode}{(value is not null ? $" {value}" : "")}"
            );
    }
}
