using System.Collections.Generic;
using TeaFramework.API.Features.ModCall;
using TeaFramework.Features.ModCall;

namespace TeaExampleMod.ModCall
{
    public class ExampleModCallHandler : ModCallHandler
    {
        public override IEnumerable<string> HandleableMessages {
            get { yield return "hello"; }
        }

        public override bool ValidateArgs(List<object> parsedArgs, out IModCallManager.ArgParseFailureType failureType) {
            if (parsedArgs.Count != 1) {
                failureType = IModCallManager.ArgParseFailureType.ArgLength;
                return false;
            }

            if (parsedArgs[0] is not string) {
                failureType = IModCallManager.ArgParseFailureType.ArgType;
                return false;
            }

            failureType = IModCallManager.ArgParseFailureType.None;
            return true;
        }

        public override object? Call(string message, List<object> args) {
            // Since we only handle "hello", not point in checking for the message.
            // If your handler handles multiple messages with different logic, you should intelligently manage them.

            // We verify the argument types in ValidateArgs, so this is safe.
            Mod?.Logger.Info($"Hello, {args[0]}!");
            return null;
        }
    }
}