using System;

namespace TeaFramework.API.Exceptions
{
    /// <summary>
    ///     Thrown when an error occurs during the invocation of dynamic methods.
    /// </summary>
    public class ReflectionInvocationException : Exception
    {
        public ReflectionInvocationException(string? message) : base(message) { }
    }
}