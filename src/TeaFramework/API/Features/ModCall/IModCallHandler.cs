using System.Collections.Generic;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.ModCall
{
    /// <summary>
    ///     Describes an object capable of handling a <see cref="Mod.Call" /> invocation handled by a
    ///     <see cref="IModCallManager" />.
    /// </summary>
    /// <remarks>
    ///     This interface extends <see cref="ILoadable" />.
    /// </remarks>
    public interface IModCallHandler : ILoadable
    {
        /// <summary>
        ///     Retrieves a collection of message keys that this object may reliably handle.
        /// </summary>
        IEnumerable<string> HandleableMessages { get; }

        /// <summary>
        ///     Validates incoming arguments before they're passed to <see cref="Call" />.
        /// </summary>
        /// <param name="parsedArgs">The parsed arguments to validate.</param>
        /// <param name="failureType">The failure type, if applicable.</param>
        /// <returns>True if the arguments are valid.</returns>
        bool ValidateArgs(List<object> parsedArgs, out IModCallManager.ArgParseFailureType failureType);

        /// <summary>
        ///     Handles the delegated call invocation.
        /// </summary>
        /// <param name="message">The message key key.</param>
        /// <param name="args">The validated parsed arguments.</param>
        /// <returns>The return value, if any.</returns>
        object? Call(string message, List<object> args);
    }
}