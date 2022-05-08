using System;

namespace TeaFramework.Exceptions
{
    public class TeaModLoadException : Exception
    {
        public TeaModLoadException(string? message = null, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
