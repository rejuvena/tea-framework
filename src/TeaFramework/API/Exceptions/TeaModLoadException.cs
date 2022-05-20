using System;

namespace TeaFramework.API.Exceptions
{
    public class TeaModLoadException : Exception
    {
        public TeaModLoadException(string? message = null, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
