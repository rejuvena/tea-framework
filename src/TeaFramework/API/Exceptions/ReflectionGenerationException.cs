using System;

namespace TeaFramework.API.Exceptions
{
    public class ReflectionGenerationException : Exception
    {
        public ReflectionGenerationException(string? message) : base(message)
        {
        }
    }
}
