using System;

namespace TeaFramework.API.Exceptions
{
    /// <summary>
    ///     A generic loading exception thrown when an error occurs during Tea Framework's rewritten mod loading.
    /// </summary>
    public class TeaModLoadException : Exception
    {
        public TeaModLoadException(string? message = null, Exception? innerException = null) : base(message, innerException) { }
    }
}