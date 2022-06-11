using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.ModCall
{
    /// <summary>
    ///     Describes an object capable of handling <see cref="Mod.Call" /> invocations.
    /// </summary>
    public interface IModCallManager
    {
        /// <summary>
        ///     Describes what went wrong when parsing args.
        /// </summary>
        public enum ArgParseFailureType
        {
            /// <summary>
            ///     Nothing went wrong.
            /// </summary>
            None,

            /// <summary>
            ///     The message key does not correspond to any registered handler.
            /// </summary>
            NoHandler,

            /// <summary>
            ///     The first argument was not a string.
            /// </summary>
            Message,

            /// <summary>
            ///     The handler encountered an invalid argument type.
            /// </summary>
            ArgType,

            /// <summary>
            ///     The handler encountered an invalid argument collection length.
            /// </summary>
            ArgLength
        }

        /// <summary>
        ///     Map of types of handlers by the handlers' registration types.
        /// </summary>
        Dictionary<Type, IModCallHandler> HandlersByType { get; }

        /// <summary>
        ///     Map of messages to handlers by what handlers handle what methods.
        /// </summary>
        Dictionary<string, IModCallHandler> HandlersByMessage { get; }

        /// <summary>
        ///     Registers a handler.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <typeparam name="T">The type to register the handler under.</typeparam>
        void RegisterHandler<T>(T handler)
            where T : IModCallHandler;

        /// <summary>
        ///     Registers a handler.
        /// </summary>
        /// <param name="type">The type to register the handler under.</param>
        /// <param name="handler">The handler instance.</param>
        void RegisterHandler(Type type, IModCallHandler handler);

        /// <summary>
        ///     Delegation of <see cref="Terraria.ModLoader.Mod.Call" />.
        /// </summary>
        /// <param name="args">The passed arguments.</param>
        /// <returns>The return value.</returns>
        object? Call(object[] args);

        /// <summary>
        ///     Attempts to parse arguments.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="failureType">How parsing failed, if it did.</param>
        /// <param name="message">The extracted message key.</param>
        /// <param name="parsedArgs">The parsed argument list.</param>
        /// <returns>True if parsing was successful.</returns>
        bool TryParseArgs(
            object[] args,
            out ArgParseFailureType failureType,
            [NotNullWhen(true)]
            out string? message,
            [NotNullWhen(true)]
            out List<object>? parsedArgs
        );

        /// <summary>
        ///     Gets a call handler from the given type.
        /// </summary>
        /// <typeparam name="T">The type, as a generic parameter.</typeparam>
        /// <returns>The call handler, null if not found.</returns>
        IModCallHandler? GetCallHandler<T>()
            where T : IModCallHandler;

        /// <summary>
        ///     Gets a call handler from the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The call handler, null if not found.</returns>
        IModCallHandler? GetCallHandler(Type type);

        /// <summary>
        ///     Gets a call handler from a message key.
        /// </summary>
        /// <param name="message">The message key.</param>
        /// <returns>The call handler, null if not found.</returns>
        IModCallHandler? GetCallHandler(string message);
    }
}