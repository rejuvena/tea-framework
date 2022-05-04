using System;

namespace TeaFramework.Exceptions
{
    public class ReflectionInvocationException : Exception
    {
        public ReflectionInvocationException(string? message) : base(message)
        {
        }
    }
}
