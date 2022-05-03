using System;
using TeaFramework.Impl.Utility;

namespace TeaFramework.API.Utility.TypeReflection
{
    /// <inheritdoc cref="IReflectedType"/>
    public interface IReflectedType<out TType> : IReflectedType
    {
        /// <inheritdoc cref="IReflectedType.TypeInstance"/>
        new TType? TypeInstance { get; }
    }
    
    /// <summary>
    ///     Represents a wrapper around a reflected object, opening access of hidden members.
    /// </summary>
    public interface IReflectedType
    {
        /// <summary>
        ///     The instance of the reflected type.
        /// </summary>
        object? TypeInstance { get; }

        /// <summary>
        ///     The reflected object's type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///     Gets a member stored in this type.
        /// </summary>
        /// <param name="type">The member type.</param>
        /// <param name="name">The member name.</param>
        /// <returns>The member instance.</returns>
        object? GetInstance(Reflection.CacheType type, string name);

        /// <inheritdoc cref="GetInstance"/>
        /// <typeparam name="T">The return type.</typeparam>
        T? GetInstance<T>(Reflection.CacheType type, string name);

        /// <summary>
        ///     Invokes the specified method.
        /// </summary>
        /// <param name="name">The method to invoke.</param>
        /// <param name="signature">The method's signature.</param>
        /// <param name="genericCount">The generic parameter count.</param>
        /// <param name="args">The method arguments.</param>
        /// <returns>The return value.</returns>
        object? InvokeMethod(string name, Type[] signature, int genericCount, object?[] args);
        
        /// <inheritdoc cref="InvokeMethod"/>
        /// <typeparam name="T">The return type.</typeparam>
        T? InvokeMethod<T>(string name, Type[] signature, int genericCount, object?[] args);
    }
}