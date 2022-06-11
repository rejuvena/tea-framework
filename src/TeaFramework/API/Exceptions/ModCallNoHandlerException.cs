using System;

namespace TeaFramework.API.Exceptions
{
    public class ModCallNoHandlerException : Exception
    {
        public ModCallNoHandlerException(string? message = null, Exception? innerException = null) : base(message, innerException) { }
    }
}