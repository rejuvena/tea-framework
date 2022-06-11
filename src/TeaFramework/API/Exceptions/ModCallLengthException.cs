using System;

namespace TeaFramework.API.Exceptions
{
    public class ModCallLengthException : Exception
    {
        public ModCallLengthException(string? message = null, Exception? innerException = null) : base(message, innerException) { }
    }
}