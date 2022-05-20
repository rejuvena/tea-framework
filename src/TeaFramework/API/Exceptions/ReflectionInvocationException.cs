using System;

namespace TeaFramework.API.Exceptions
{
    public class ReflectionInvocationException : Exception
    {
        public ReflectionInvocationException(string? message) : base(message)
        {
        }
    }
}
