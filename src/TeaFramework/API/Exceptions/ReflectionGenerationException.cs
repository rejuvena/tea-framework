using System;

namespace TeaFramework.API.Exceptions
{
    /// <summary>
    ///     Thrown when an error occurs during the generation of dynamic methods.
    /// </summary>
    public class ReflectionGenerationException : Exception
    {
        public ReflectionGenerationException(string? message) : base(message) { }
    }
}