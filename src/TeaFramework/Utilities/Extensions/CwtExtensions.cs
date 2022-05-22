using TeaFramework.Utilities.CWT;

namespace TeaFramework.Utilities.Extensions
{
    public static class CwtExtensions
    {
        public static TValue GetDynamicField<TKey, TValue>(this TKey key, string fieldName)
            where TKey : class where TValue : class, new() => CwtManager<TKey, TValue>.GetField(key, fieldName);
    }
}
