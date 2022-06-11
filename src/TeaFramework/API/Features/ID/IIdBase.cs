using ReLogic.Reflection;

namespace TeaFramework.API.Features.ID
{
    /// <summary>
    ///     Serves as a consistent base for ID classes in a nearly-identical vein.
    /// </summary>
    /// <typeparam name="TIdBase">The class, should be itself.</typeparam>
    /// <typeparam name="TType">The type each ID uses. Often an integer.</typeparam>
    public interface IIdBase<TIdBase, TType>
        where TIdBase : IIdBase<TIdBase, TType>
    {
        public static IdDictionary Search { get; } = IdDictionary.Create<TIdBase, TType>();
    }
}