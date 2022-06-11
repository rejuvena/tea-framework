using System;

namespace TeaFramework.API.Exceptions
{
    public class ModCallInvalidTypeException : Exception
    {
        public ModCallInvalidTypeException(string? message = null, Exception? innerException = null) : base(message, innerException) { }
    }
}