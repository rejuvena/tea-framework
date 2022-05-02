using System;
using TeaFramework.API.Utility.TypeReflection;

namespace TeaFramework.Utility.TypeReflection
{
    /// <summary>
    ///     Standard implementation of <see cref="IReflectedType{TType}"/>.
    /// </summary>
    public abstract class ReflectedType<TType> : ReflectedType, IReflectedType<TType>
    {
        public new TType? TypeInstance => (TType?) base.TypeInstance;

        public override Type Type => typeof(TType);

        protected ReflectedType(TType? typeInstance) : base(typeInstance)
        {
        }
    }

    /// <summary>
    ///     Standard implementation of <see cref="IReflectedType"/>.
    /// </summary>
    public abstract class ReflectedType : IReflectedType
    {
        public object? TypeInstance { get; }

        public virtual Type Type => TypeInstance!.GetType();

        protected ReflectedType(object? typeInstance)
        {
            TypeInstance = typeInstance;
        }

        public virtual object? GetInstance(Reflection.CacheType type, string name) =>
            type switch
            {
                Reflection.CacheType.Field => Type.GetCachedField(name).GetValue(TypeInstance),
                Reflection.CacheType.Method => throw new ArgumentException("Use InvokeMethod.", nameof(type)),
                Reflection.CacheType.Property => Type.GetCachedProperty(name).GetValue(TypeInstance),
                Reflection.CacheType.Constructor => throw new ArgumentException("Construct manually.", nameof(type)),
                Reflection.CacheType.Type =>
                    throw new ArgumentException("Types are not to be retrieved.", nameof(type)),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public virtual T? GetInstance<T>(Reflection.CacheType type, string name) => (T?) GetInstance(type, name);

        public virtual object? InvokeMethod(string name, Type[] signature, int genericCount, object?[] args) =>
            Type.GetCachedMethod(name, signature, genericCount).Invoke(TypeInstance, args);

        public virtual T? InvokeMethod<T>(string name, Type[] signature, int genericCount, object?[] args) =>
            (T?) InvokeMethod(name, signature, genericCount, args);
    }
}