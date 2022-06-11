using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TeaFramework.API.Exceptions;
using TeaFramework.API.Features.ModCall;

namespace TeaFramework.Features.ModCall
{
    public class ModCallManager : IModCallManager
    {
        public Dictionary<Type, IModCallHandler> HandlersByType { get; } = new();

        public Dictionary<string, IModCallHandler> HandlersByMessage { get; } = new();

        public void RegisterHandler<T>(T handler)
            where T : IModCallHandler {
            RegisterHandler(typeof(T), handler);
        }

        public void RegisterHandler(Type type, IModCallHandler handler) {
            HandlersByType[type] = handler;
            foreach (string message in handler.HandleableMessages) HandlersByMessage[message] = handler;
        }

        public object? Call(object[] args) {
            if (args.Length == 0) throw new ModCallLengthException("Expected an argument length of least one.");

            if (TryParseArgs(args, out IModCallManager.ArgParseFailureType failureType, out string? message, out List<object>? parsedArgs)) {
                IModCallHandler? handler = GetCallHandler(message);

                if (handler is null)
                    throw new NullReferenceException(
                        $"Could not retrieve handler with message key \"{HandlersByMessage}\", how is this possible? (Should be handled and aborted.)"
                    );

                return handler.Call(message, parsedArgs);
            }

            throw failureType switch
            {
                IModCallManager.ArgParseFailureType.None => new ModCallNoHandlerException(
                    "Call invocation somehow failed due to a lack of an applicable handler, parse error?"
                ),
                IModCallManager.ArgParseFailureType.NoHandler => new ModCallNoHandlerException("No handler could be resolved from the passed key."),
                IModCallManager.ArgParseFailureType.Message => new ModCallInvalidTypeException(
                    "The message key could not be extracted as the first argument was not a string."
                ),
                IModCallManager.ArgParseFailureType.ArgType => new ModCallInvalidTypeException("An argument with an invalid type was passed."),
                IModCallManager.ArgParseFailureType.ArgLength => new ModCallLengthException(
                    "The arguments passed resulted in an invalid collection length."
                ),
                _ => new ArgumentOutOfRangeException(null, "Invalid failure type: " + failureType)
            };
        }

        public bool TryParseArgs(
            object[] args,
            out IModCallManager.ArgParseFailureType failureType,
            [NotNullWhen(true)]
            out string? message,
            [NotNullWhen(true)]
            out List<object>? parsedArgs
        ) {
            // We verified that args > 0 earlier in the call stack.

            failureType = IModCallManager.ArgParseFailureType.None;
            message = null;
            parsedArgs = null;

            if (args[0] is not string m) {
                failureType = IModCallManager.ArgParseFailureType.Message;
                return false;
            }

            message = m;
            parsedArgs = args.ToList();
            parsedArgs.RemoveAt(0);

            IModCallHandler? handler = GetCallHandler(message);

            if (handler is null) {
                failureType = IModCallManager.ArgParseFailureType.NoHandler;
                return false;
            }

            if (handler.ValidateArgs(parsedArgs, out IModCallManager.ArgParseFailureType localFailureType)) return true;

            failureType = localFailureType;
            return false;
        }

        public IModCallHandler? GetCallHandler<T>()
            where T : IModCallHandler {
            return GetCallHandler(typeof(T));
        }

        public IModCallHandler? GetCallHandler(Type type) {
            return HandlersByType.ContainsKey(type) ? HandlersByType[type] : null;
        }

        public IModCallHandler? GetCallHandler(string message) {
            return HandlersByMessage.ContainsKey(message) ? HandlersByMessage[message] : null;
        }
    }
}