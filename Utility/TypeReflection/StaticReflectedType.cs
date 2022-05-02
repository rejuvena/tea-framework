namespace TeaFramework.Utility.TypeReflection
{
    /// <summary>
    ///     Static extension to <see cref="ReflectedType{TType}"/>.
    /// </summary>
    public abstract class StaticReflectedType<TType> : ReflectedType<TType>
    {
        protected StaticReflectedType() : base(default)
        {
        }
    }
    
    /// <summary>
    ///     Static extension to <see cref="ReflectedType"/>.
    /// </summary>
    public abstract class StaticReflectedType : ReflectedType
    {
        protected StaticReflectedType() : base(default)
        {
        }
    }
}