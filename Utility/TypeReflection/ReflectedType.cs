using System;

namespace TeaFramework.Utility.TypeReflection
{
    /// <summary>
    ///     Standard implementation of <see cref="IReflectedType{TType}"/>.
    /// </summary>
    public abstract class ReflectedType<TType> : ReflectedType, IReflectedType<TType>
    {
        public new TType? TypeInstance => (TType?) base.TypeInstance;

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

        protected ReflectedType(object? typeInstance)
        {
            TypeInstance = typeInstance;
        }

        public object GetInstance(Reflection.CacheType type, string name)
        {
        }

        public T GetInstance<T>(Reflection.CacheType type, string name)
        {
        }

        public object? InvokeMethod(string name, Type[] signature, object?[] args)
        {
        }

        public T? InvokeMethod<T>(string name, Type[] signature, object?[] args)
        {
        }
    }
}