using System;

namespace TeaFramework.Impl.Utility.TypeReflection
{
    /// <summary>
    ///     Static extension to <see cref="ReflectedType{TType}"/>.
    /// </summary>
    public abstract class StaticReflectedType<TType> : ReflectedType<TType>
    {
        public override Type Type => typeof(TType);

        protected StaticReflectedType() : base(default)
        {
        }
    }

    /// <summary>
    ///     Static extension to <see cref="ReflectedType"/>.
    /// </summary>
    public abstract class StaticReflectedType : ReflectedType
    {
        public override Type Type => throw new NullReferenceException("Static reflected types must override Type.");

        protected StaticReflectedType() : base(default)
        {
        }
    }
}