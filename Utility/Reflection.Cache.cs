using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TeaFramework.Utility
{
    partial class Reflection
    {
        /// <summary>
        ///     The type of reflected member.
        /// </summary>
        public enum CacheType
        {
            Field,
            Method,
            Property,
            Constructor,
            Type
        }

        #region Cache Definition

        private static readonly Dictionary<CacheType, Dictionary<string, object?>> Cache = new()
        {
            [CacheType.Field] = new Dictionary<string, object?>(),
            [CacheType.Method] = new Dictionary<string, object?>(),
            [CacheType.Property] = new Dictionary<string, object?>(),
            [CacheType.Constructor] = new Dictionary<string, object?>(),
            [CacheType.Type] = new Dictionary<string, object?>()
        };

        private static T? RetrieveFromCache<T>(CacheType cacheType, string key, Func<T?> provider)
        {
            if (Cache[cacheType].ContainsKey(key))
                return (T?) Cache[cacheType][key];

            T? value = provider();
            Cache[cacheType].Add(key, value);
            return value;
        }

        #endregion

        #region Name Suppliers

        public static string GetFieldName(Type type, string field) => 
            Cecil.GetTypeName(type) + " " + field;
        
        public static string GetPropertyName(Type type, string property) => 
            Cecil.GetTypeName(type) + " " + property;

        public static string GetMethodName(Type type, string method, Type[] signature, int genericCount) =>
            GetMethodSignature(Cecil.GetTypeName(type) + "::" + method, signature, genericCount);

        public static string GetConstructorName(Type type, Type[] signature) =>
            GetMethodSignature(Cecil.GetTypeName(type) + "::" + ".ctor", signature, 0);

        public static string GetMethodSignature(string name, Type[] signature, int genericCount)
        {
            StringBuilder builder = new();

            //builder.Append(Cecil.GetTypeName(returnType));
            //builder.Append(' ');
            builder.Append(name);

            if (genericCount != 0)
            {
                builder.Append('<');
                
                for (int i = 0; i < genericCount; i++) 
                    builder.Append($"T{i}");

                builder.Append('>');
            }

            builder.Append('(');

            for (int i = 0; i < signature.Length; i++)
            {
                if (i > 0)
                    builder.Append(',');

                builder.Append(Cecil.GetTypeName(signature[i]));
            }
            
            builder.Append(')');
            
            return builder.ToString();
        }

        #endregion

        #region Nullable Cache Retrieval

        public static Type? GetCachedTypeNullable(this Assembly assembly, string name) =>
            RetrieveFromCache(
                CacheType.Type,
                name,
                () => assembly.GetType(name)
            );

        public static FieldInfo? GetCachedFieldNullable(this Type type, string name) =>
            RetrieveFromCache(
                CacheType.Field,
                GetFieldName(type, name),
                () => type.GetField(name, UniversalFlags)
            );

        public static PropertyInfo? GetCachedPropertyNullable(this Type type, string name) =>
            RetrieveFromCache(
                CacheType.Property,
                GetPropertyName(type, name),
                () => type.GetProperty(name, UniversalFlags)
            );

        public static MethodInfo? GetCachedMethodNullable(
            this Type type,
            string name,
            Type[]? signature = null,
            int genericCount = 0
        )
        {
            bool hasSignature = signature is not null;
            bool hasGenerics = genericCount != 0;

            if (!hasSignature && !hasGenerics)
            {
                return RetrieveFromCache(
                    CacheType.Method,
                    GetMethodName(type, name, Array.Empty<Type>(), genericCount),
                    () => type.GetMethod(name, UniversalFlags)
                );
            }

            if (hasSignature && !hasGenerics)
            {
                return RetrieveFromCache(
                    CacheType.Method,
                    GetMethodName(type, name, signature!, genericCount),
                    () => type.GetMethod(name, UniversalFlags, null, signature!, null)
                );
            }

            if (!hasSignature && hasGenerics)
                throw new ArgumentNullException(nameof(signature), "Generic count cannot be used without signature.");

            return RetrieveFromCache(
                CacheType.Method,
                GetMethodName(type, name, signature!, genericCount),
                () => type.GetMethod(name, genericCount, UniversalFlags, null, signature!, null)
            );
        }

        public static ConstructorInfo? GetCachedConstructorNullable(this Type type, Type[] signature) =>
            RetrieveFromCache(CacheType.Constructor,
                GetConstructorName(type, signature),
                () => type.GetConstructor(UniversalFlags, null, signature, null)
            );

        #endregion

        #region Non-null Cache Retrieval

        public static Type GetCachedType(this Assembly assembly, string name) => GetCachedTypeNullable(assembly, name)!;

        public static FieldInfo GetCachedField(this Type type, string name) => GetCachedFieldNullable(type, name)!;

        public static PropertyInfo GetCachedProperty(this Type type, string name) =>
            GetCachedPropertyNullable(type, name)!;

        public static MethodInfo GetCachedMethod(
            this Type type,
            string name,
            Type[]? signature = null,
            int genericCount = 0
        ) => GetCachedMethodNullable(type, name, signature, genericCount)!;

        public static ConstructorInfo GetCachedConstructor(this Type type, Type[] signature) =>
            GetCachedConstructorNullable(type, signature)!;

        #endregion
    }
}