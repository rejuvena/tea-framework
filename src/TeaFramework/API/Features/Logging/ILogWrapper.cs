using log4net;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.API.Features.Logging
{
    /// <summary>
    ///     An extensive wrapper around an <see cref="ILog"/> instance.
    /// </summary>
    public interface ILogWrapper : IService
    {
        /// <summary>
        ///     The wrapped <see cref="ILog"/> instance/
        /// </summary>
        ILog Logger { get; }

        /// <summary>
        ///     Boilerplate for patch logging.
        /// </summary>
        /// <param name="type">The patch failure type.</param>
        /// <param name="message">The message to log.</param>
        string LogPatchFailure(string type, string message);

        /// <summary>
        ///     Logs an op-code jump failure.
        /// </summary>
        /// <param name="typeName">The name of the patched type.</param>
        /// <param name="typeMethod">The name of the patched method.</param>
        /// <param name="opcode">The opcode which could not be jumped to.</param>
        /// <param name="value">The expected value of the opcode.</param>
        string LogOpCodeJumpFailure(string typeName, string typeMethod, string opcode, string? value);
    }
}
