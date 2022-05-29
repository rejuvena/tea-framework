using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TeaFramework.Utilities.CWT
{
    public static class CwtManager<TKey, TValue>
        where TKey : class
        where TValue : class, new()
    {
        public static readonly Dictionary<string, ConditionalWeakTable<TKey, TValue>> Data = new();

        public static TValue GetField(TKey key, string fieldName) {
            ConditionalWeakTable<TKey, TValue> fieldTable = GetFieldTable(fieldName);

            if (fieldTable.TryGetValue(key, out TValue? value)) return value;

            TValue newVal = new();
            fieldTable.Add(key, newVal);
            return newVal;
        }

        public static ConditionalWeakTable<TKey, TValue> GetFieldTable(string field) {
            if (Data.ContainsKey(field)) return Data[field];

            return Data[field] = new ConditionalWeakTable<TKey, TValue>();
        }
    }
}